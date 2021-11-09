using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using System.Xml;

namespace PowerSaver {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {


        [HarmonyPostfix]
        public static void Postfix() {

            string settingsPath = Path.Combine(Util.getFilesFolder(), PowerSaver.PRIORITY_LIST_PATH);
            string iconPath = Path.Combine(Util.getFilesFolder(), PowerSaver.CONSOLE_ICON_PATH);
            if (!File.Exists(settingsPath)) {
                Debug.Log("[MOD] PowerManager couldn't find the settings file.");
                return;
            }

            if (!File.Exists(iconPath)) {
                Debug.Log("[MOD] PowerManager couldn't find the new console's icon.");
                return;
            }

            PowerSaver.mPowerSavingModes = new List<SavingMode>();
            PowerSaver.mWaterSavingModes = new List<SavingMode>();
            PowerSaver.mPowerPriorityList = new List<Type>();
            PowerSaver.mWaterPriorityList = new List<Type>();

            try {
                System.Reflection.Assembly gameAssembly = System.Reflection.Assembly.GetCallingAssembly();
                using (XmlReader reader = XmlReader.Create(settingsPath)) {

                    // Read Power saving modes
                    reader.ReadToFollowing("PowerSavingModes");
                    XmlReader powerSavingModes = reader.ReadSubtree();
                    while (powerSavingModes.ReadToFollowing("SavingMode")) {
                        powerSavingModes.MoveToFirstAttribute();
                        powerSavingModes.ReadAttributeValue();
                        if (!powerSavingModes.HasValue)
                            continue;

                        int trigger = Int32.Parse(powerSavingModes.Value);
                        if (powerSavingModes.ReadToFollowing("Module")) {
                            SavingMode mode = new SavingMode(trigger);
                            do {
                                Type type = gameAssembly.GetType("Planetbase.ModuleType" + powerSavingModes.ReadElementContentAsString(), false, true);
                                if (type != null)
                                    mode.typesToShutDown.Add(type);
                            } while (powerSavingModes.ReadToNextSibling("Module"));

                            PowerSaver.mPowerSavingModes.Add(mode);
                        }
                    }

                    // Read water saving modes
                    reader.ReadToFollowing("WaterSavingModes");
                    XmlReader waterSavingModes = reader.ReadSubtree();
                    while (waterSavingModes.ReadToFollowing("SavingMode")) {
                        waterSavingModes.MoveToFirstAttribute();
                        waterSavingModes.ReadAttributeValue();
                        if (!waterSavingModes.HasValue)
                            continue;

                        int trigger = Int32.Parse(waterSavingModes.Value);
                        if (waterSavingModes.ReadToFollowing("Module")) {
                            SavingMode mode = new SavingMode(trigger);
                            do {
                                Type type = gameAssembly.GetType("Planetbase.ModuleType" + waterSavingModes.ReadElementContentAsString(), false, true);
                                if (type != null)
                                    mode.typesToShutDown.Add(type);
                            } while (waterSavingModes.ReadToFollowing("Module"));

                            PowerSaver.mWaterSavingModes.Add(mode);
                        }
                    }

                    // Read power priority list
                    reader.ReadToFollowing("PowerList");
                    XmlReader powerList = reader.ReadSubtree();
                    while (powerList.ReadToFollowing("Module")) {
                        Type type = gameAssembly.GetType("Planetbase.ModuleType" + powerList.ReadElementContentAsString(), false, true);
                        if (type != null)
                            PowerSaver.mPowerPriorityList.Add(type);
                    }

                    // Read water priority list
                    reader.ReadToFollowing("WaterList");
                    XmlReader waterList = reader.ReadSubtree();
                    while (waterList.ReadToFollowing("Module")) {
                        Type type = gameAssembly.GetType("Planetbase.ModuleType" + waterList.ReadElementContentAsString(), false, true);
                        if (type != null)
                            PowerSaver.mWaterPriorityList.Add(type);
                    }
                }
            }
            catch (Exception e) {
                Debug.Log("<MOD> PowerManager failed to load the settings file. Exception: " + e.Message);
                return;
            }

            PowerSaver.mPowerSavingModes = PowerSaver.mPowerSavingModes.OrderBy(m => m.trigger).ToList();
            PowerSaver.mWaterSavingModes = PowerSaver.mWaterSavingModes.OrderBy(m => m.trigger).ToList();
            PowerSaver.mActivePowerSavingMode = null;
            PowerSaver.mActiveWaterSavingMode = null;

            foreach (Type type in PowerSaver.DEFAULT_POWER_PRIORITY_LIST) {
                if (!PowerSaver.mPowerPriorityList.Contains(type))
                    PowerSaver.mPowerPriorityList.Insert(0, type);
            }

            foreach (Type type in PowerSaver.DEFAULT_WATER_PRIORITY_LIST) {
                if (!PowerSaver.mWaterPriorityList.Contains(type))
                    PowerSaver.mWaterPriorityList.Insert(0, type);
            }

            Traverse.Create(TypeList<ComponentType, ComponentTypeList>.getInstance()).Method("add").GetValue(new GridManagementConsole());
            ModuleTypeControlCenter controlCenter = TypeList<ModuleType, ModuleTypeList>.find<ModuleTypeControlCenter>() as ModuleTypeControlCenter;
            Traverse<ComponentType[]> t_controlCenter_mComponentTypes = Traverse.Create(controlCenter).Field<ComponentType[]>("mComponentTypes");
            List<ComponentType> components = t_controlCenter_mComponentTypes.Value.ToList();
            components.Insert(3, TypeList<ComponentType, ComponentTypeList>.find<GridManagementConsole>());
            t_controlCenter_mComponentTypes.Value = components.ToArray();



        }

    }

}
