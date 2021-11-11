using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using HarmonyLib;

namespace PowerSaver {

    public class PowerSaver {

        public static string PRIORITY_LIST_PATH = @"Mods\Settings\PowerSaver.xml";
        public static string CONSOLE_ICON_PATH = @"Mods\Textures\GridManagementConsoleIcon.png";

        public static List<Type> DEFAULT_POWER_PRIORITY_LIST = new Type[]
        {
            typeof(ModuleTypeBasePad),
            typeof(ModuleTypeSignpost),
            typeof(ModuleTypeStarport),
            typeof(ModuleTypeLandingPad),
            typeof(ModuleTypeRadioAntenna),
            typeof(ModuleTypeStorage),
            typeof(ModuleTypeRoboticsFacility),
            typeof(ModuleTypeMine),
            typeof(ModuleTypeFactory),
            typeof(ModuleTypeProcessingPlant),
            typeof(ModuleTypeLab),
            typeof(ModuleTypeWaterTank),
            typeof(ModuleTypeBar),
            typeof(ModuleTypeMultiDome),
            typeof(ModuleTypeAntiMeteorLaser),
            typeof(ModuleTypeTelescope),
            typeof(ModuleTypeControlCenter),
            typeof(ModuleTypeDorm),
            typeof(ModuleTypeCabin),
            typeof(ModuleTypeSickBay),
            typeof(ModuleTypeCanteen),
            typeof(ModuleTypeBioDome),
            typeof(ModuleTypeAirlock),
            typeof(ModuleTypeOxygenGenerator),
            typeof(ModuleTypeWaterExtractor)
        }.ToList();

        public static List<Type> DEFAULT_WATER_PRIORITY_LIST = new Type[]
        {
            typeof(ModuleTypeLab),
            typeof(ModuleTypeBar),
            typeof(ModuleTypeMultiDome),
            typeof(ModuleTypeCanteen),
            typeof(ModuleTypeBioDome),
            typeof(ModuleTypeOxygenGenerator)
        }.ToList();

        public static List<SavingMode> mPowerSavingModes;
        public static List<SavingMode> mWaterSavingModes;
        public static SavingMode mActivePowerSavingMode;
        public static SavingMode mActiveWaterSavingMode;

        public static List<Type> mPowerPriorityList;
        public static List<Type> mWaterPriorityList;

        public static List<ConstructionComponent> ConstructionComponent_mComponents = Traverse.Create(typeof(ConstructionComponent)).Field<List<ConstructionComponent>>("mComponents").Value;

        public static void SwitchSavingMode(SavingMode newSavingMode, GridResource resource) {
            Grid grid = Grid.getLargest();
            HashSet<Construction> grid_mConstructions = Traverse.Create(grid).Field<HashSet<Construction>>("mConstructions").Value;

            SavingMode currentSavingMode = resource == GridResource.Power ? PowerSaver.mActivePowerSavingMode : PowerSaver.mActiveWaterSavingMode;
            if (currentSavingMode != null) {
                // enable all types in this mode
                foreach (Type type in currentSavingMode.typesToShutDown) {
                    List<Construction> constructions = grid_mConstructions
                        .Where(
                                c => c is Planetbase.Module &&
                                Traverse.Create(c as Planetbase.Module).Field<ModuleType>("mModuleType").Value.GetType() == type
                        )
                        .ToList();
                    foreach (Construction construction in constructions) {
                        construction.setEnabled(true);
                    }
                }
            }

            if (newSavingMode != null) {
                // disable all types in the new mode
                bool skippedConsoleModule = false;
                foreach (Type type in newSavingMode.typesToShutDown) {
                    List<Construction> constructions = grid_mConstructions
                        .Where(
                                c => c is Planetbase.Module &&
                                Traverse.Create(c as Planetbase.Module).Field<ModuleType>("mModuleType").Value.GetType() == type
                        )
                        .ToList();
                    foreach (Construction construction in constructions) {
                        //if (module.mComponents.FirstOrDefault(c => c.mComponentType is GridManagementConsole) != null)
                        List<ConstructionComponent> construction_mComponents = Traverse.Create(construction).Field<List<ConstructionComponent>>("mComponents").Value;
                        if (
                                !skippedConsoleModule &&
                                type == typeof(ModuleTypeControlCenter) &&
                                construction_mComponents.FirstOrDefault(c => Traverse.Create(c).Field<ComponentType>("mComponentType").Value is GridManagementConsole) != null
                            ) {
                            skippedConsoleModule = true;
                            continue;
                        }
                        construction.setEnabled(false);
                    }
                }
            }

            if (resource == GridResource.Power)
                PowerSaver.mActivePowerSavingMode = newSavingMode;
            else
                PowerSaver.mActiveWaterSavingMode = newSavingMode;
        }

        public static float GetTotalConsumption(Grid grid, GridResource resource) {
            float total = 0;
            HashSet<Construction> grid_mConstructions = Traverse.Create(grid).Field<HashSet<Construction>>("mConstructions").Value;
            Traverse t = Traverse.Create(grid).Method("getGeneration");
            foreach (Construction construction in grid_mConstructions) {
                if (construction.isBuilt() && !construction.isExtremelyDamaged()) {
                    float generation = t.GetValue<float>(new object[] { construction, resource });
                    if (generation < 0f)
                        total -= generation;
                }
            }

            return total;
        }

    }

}
