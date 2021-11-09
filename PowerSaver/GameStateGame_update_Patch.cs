using Planetbase;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System;

namespace PowerSaver {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        [HarmonyPostfix]
        public static void Postfix(GameStateGame __instance) {

            bool consoleExists = false;
            foreach (ConstructionComponent component in PowerSaver.ConstructionComponent_mComponents) {
                Indicator component_mConditionIndicator = Traverse.Create(component).Field<Indicator>("mConditionIndicator").Value;
                Construction component_mParentConstruction = Traverse.Create(component).Field<Construction>("mParentConstruction").Value;
                bool lowCondition = component_mConditionIndicator.isValidValue() && component_mConditionIndicator.isExtremelyLow();
                if (component.getComponentType().GetType() == typeof(GridManagementConsole) && component.isBuilt() && !lowCondition && component.isEnabled() &&
                    component_mParentConstruction.isBuilt() && component_mParentConstruction.isEnabled() && !component_mParentConstruction.isExtremelyDamaged()) {
                    consoleExists = true;
                    break;
                }
            }

            if (!consoleExists)
                return;

            Grid grid = Grid.getLargest();
            if (grid == null)
                return;

            if (PowerSaver.mPowerSavingModes.Count > 0 && Module.getOverallPowerStorageCapacity() > 0f && (PowerSaver.GetTotalConsumption(grid, GridResource.Power) > grid.getTotalPowerGeneration() || PowerSaver.mActivePowerSavingMode != null)) {
                float powerPercentage = (float)Module.getOverallPowerStorage() / Module.getOverallPowerStorageCapacity() * 100f;

                SavingMode newSavingMode = PowerSaver.mPowerSavingModes.FirstOrDefault(m => powerPercentage <= m.trigger);
                if (newSavingMode != PowerSaver.mActivePowerSavingMode) {
                    bool dontSwitch = false;
                    if (PowerSaver.mActivePowerSavingMode != null && (newSavingMode == null || newSavingMode.trigger > PowerSaver.mActivePowerSavingMode.trigger))
                        dontSwitch = powerPercentage < Mathf.Min(PowerSaver.mActivePowerSavingMode.trigger * 1.2f, 100f);

                    if (!dontSwitch)
                        PowerSaver.SwitchSavingMode(newSavingMode, GridResource.Power);
                }
            }

            if (PowerSaver.mWaterSavingModes.Count > 0 && Module.getOverallWaterStorageCapacity() > 0f) {
                GridResourceData grid_getData_GridResource_Water = Traverse.Create(grid).Method("getData").GetValue<GridResourceData>(GridResource.Water);
                if (PowerSaver.GetTotalConsumption(grid, GridResource.Water) > grid_getData_GridResource_Water.getGeneration() || PowerSaver.mActiveWaterSavingMode != null) {
                    float waterPercentage = (float)Module.getOverallWaterStorage() / Module.getOverallWaterStorageCapacity() * 100;

                    SavingMode newSavingMode = PowerSaver.mWaterSavingModes.FirstOrDefault(m => waterPercentage <= m.trigger);
                    if (newSavingMode != PowerSaver.mActiveWaterSavingMode) {
                        bool dontSwitch = false;
                        if (PowerSaver.mActiveWaterSavingMode != null && (newSavingMode == null || newSavingMode.trigger > PowerSaver.mActiveWaterSavingMode.trigger))
                            dontSwitch = waterPercentage < Mathf.Min(PowerSaver.mActiveWaterSavingMode.trigger * 1.2f, 100f);

                        if (!dontSwitch)
                            PowerSaver.SwitchSavingMode(newSavingMode, GridResource.Water);
                    }
                }
            }

        }

    }

}
