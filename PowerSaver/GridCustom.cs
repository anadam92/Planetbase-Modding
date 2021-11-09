using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Planetbase;
using HarmonyLib;

namespace PowerSaver {

    [HarmonyPatch(typeof(Grid), "calculateBalance", MethodType.Normal)]
    public class Grid_calculateBalance_Patch {

        [HarmonyPrefix]
        public bool Prefix(Grid __instance, GridResource gridResource) {
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

            Traverse t___instance = Traverse.Create(__instance);
            if (consoleExists)
                advancedCalculateBalance(t___instance, gridResource);
            else
                basicCalculateBalance(t___instance, gridResource);

            return false;
        }

        private void advancedCalculateBalance(Traverse t___instance, GridResource gridResource) {
            HashSet<Construction> __instance_mConstructions = t___instance.Field<HashSet<Construction>>("mConstructions").Value;
            Traverse t___instance_getGeneration = t___instance.Method("getGeneration");
            Traverse t___instance_setResourceAvailable = t___instance.Method("setResourceAvailable");
            Traverse t___instance_isResourceAvailable = t___instance.Method("isResourceAvailable");
            Dictionary<Type, List<Module>> constructionsByType = new Dictionary<Type, List<Module>>();
            float resourceBalance = 0f;
            float amountCreated = 0f;
            float amountConsumed = 0f;

            foreach (Construction construction in __instance_mConstructions) {
                if (construction.isBuilt() && construction.isEnabled() && !construction.isExtremelyDamaged()) {
                    // amountGenerated can be either created or consumed
                    float amountGenerated = t___instance_getGeneration.GetValue<float>(new object[] { construction, gridResource });
                    resourceBalance += amountGenerated;

                    if (amountGenerated > 0f)
                        amountCreated += amountGenerated;
                    else
                        amountConsumed -= amountGenerated;
                    
                    t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, true });

                    if (construction is Connection)
                        continue;

                    Module module = construction as Module;
                    ModuleType module_mModuleType = Traverse.Create(module).Field<ModuleType>("mModuleType").Value;
                    List<Module> list;
                    if (constructionsByType.TryGetValue(module_mModuleType.GetType(), out list)) {
                        list.Add(module);
                    }
                    else {
                        list = new List<Module>();
                        list.Add(module);
                        constructionsByType[module_mModuleType.GetType()] = list;
                    }
                }
                else {
                    t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, false });
                }
            }

            GridResourceData resourceData = t___instance.Method("getData").GetValue<GridResourceData>(new object[] { gridResource });
            resourceData.setCollector(t___instance.Method("findCollector").GetValue<Construction>(new object[] { gridResource, resourceBalance }));
            resourceData.setBalance(resourceBalance);
            resourceData.setGeneration(amountCreated);
            resourceData.setConsumption(amountConsumed);

            if (resourceBalance < 0f && resourceData.getCollector() == null) {
                Module consoleModule = null;
                List<Type> priorityList = gridResource == GridResource.Power ? PowerSaver.mPowerPriorityList : PowerSaver.mWaterPriorityList;
                foreach (Type type in priorityList) {
                    List<Module> constructions;
                    if (constructionsByType.TryGetValue(type, out constructions)) {
                        foreach (Module module in constructions) {
                            ModuleType module_mModuleType = Traverse.Create(module).Field<ModuleType>("mModuleType").Value;
                            List<ConstructionComponent> module_mComponents = Traverse.Create(module).Field<List<ConstructionComponent>>("mComponents").Value;
                            if (consoleModule == null && module_mModuleType is ModuleTypeControlCenter) {
                                if (module_mComponents.FirstOrDefault(c => Traverse.Create(c).Field<ComponentType>("mComponentType").Value is GridManagementConsole) != null) {
                                    consoleModule = module;
                                    continue;
                                }
                            }

                            float generation = t___instance_getGeneration.GetValue<float>(new object[] { module, gridResource });
                            if (generation < 0f && module.isEnabled()) {
                                resourceBalance -= generation;
                                t___instance_setResourceAvailable.GetValue(new object[] { module, gridResource, false });

                                if (resourceBalance > 0f)
                                    return;

                                foreach (Construction connection in module.getLinks()) {
                                    if (t___instance.Method("setResourceisResourceAvailableAvailable").GetValue<bool>(new object[] { connection, gridResource })) {
                                        generation = t___instance_getGeneration.GetValue<float>(new object[] { connection, gridResource });
                                        resourceBalance -= generation;
                                        t___instance_setResourceAvailable.GetValue(new object[] { connection, gridResource, false });

                                        if (resourceBalance > 0f)
                                            return;
                                    }
                                }
                            }
                        }
                    }
                }

                // if we reach this point, we still don't have a positive balance
                // and the only module active is the control center with the grid management console
                if (consoleModule != null) {
                    t___instance_setResourceAvailable.GetValue(new object[] { consoleModule, gridResource, false });
                }

            }
        }

        private void basicCalculateBalance(Traverse t___instance, GridResource gridResource) {
            HashSet<Construction> __instance_mConstructions = t___instance.Field<HashSet<Construction>>("mConstructions").Value;
            Traverse t___instance_getGeneration = t___instance.Method("getGeneration");
            Traverse t___instance_setResourceAvailable = t___instance.Method("setResourceAvailable");
            Traverse t___instance_isResourceAvailable = t___instance.Method("isResourceAvailable");
            HashSet<Construction> constructionsLackingResource = new HashSet<Construction>();
            GridResourceData resourceData = t___instance.Method("getData").GetValue<GridResourceData>(new object[] { gridResource });
            float resourceBalance = 0f;
            float amountCreated = 0f;
            float amountConsumed = 0f;

            foreach (Construction construction in __instance_mConstructions) {
                if (construction.isBuilt() && construction.isEnabled() && !construction.isExtremelyDamaged()) {
                    // amountGenerated can be either created or consumed
                    float amountGenerated = t___instance_getGeneration.GetValue<float>(new object[] { construction, gridResource });
                    resourceBalance += amountGenerated;

                    if (amountGenerated > 0f)
                        amountCreated += amountGenerated;
                    else
                        amountConsumed -= amountGenerated;
                    
                    if (!t___instance_isResourceAvailable.GetValue<bool>(new object[] { construction, gridResource }))
                        constructionsLackingResource.Add(construction);

                    t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, true });
                }
                else {
                    t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, false });
                }
            }

            // if resourceBalance is positive, returns the first collector that is not full.
            // Otherwise, returns the first collector that has available resource
            Construction collector = t___instance.Method("findCollector").GetValue<Construction>(new object[] { gridResource, resourceBalance });

            if (resourceBalance < 0f && collector == null) {
                HashSet<Construction> constructionsToShutDown = new HashSet<Construction>();
                foreach (Construction construction in __instance_mConstructions) {
                    if (t___instance_getGeneration.GetValue<float>(new object[] { construction, gridResource }) < 0f && construction.isEnabled()) {
                        t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, false });
                        if (!constructionsToShutDown.Contains(construction))
                            constructionsToShutDown.Add(construction);
                    }
                }

                float amountAvailable = amountCreated;
                bool somethingChanged = true;
                while (somethingChanged) {
                    somethingChanged = false;
                    foreach (Construction construction in __instance_mConstructions) {
                        if (
                                t___instance_getGeneration.GetValue<float>(new object[] { construction, gridResource }) < 0f && construction.isEnabled() &&
                                !t___instance_isResourceAvailable.GetValue<bool>(new object[] { construction, gridResource })
                            ) {
                            foreach (Construction linkedConstruction in construction.getLinks()) {
                                if (linkedConstruction.isPowered() || t___instance_getGeneration.GetValue<float>(new object[] { linkedConstruction, gridResource }) > 0f) {
                                    float absAmountUsed = -t___instance_getGeneration.GetValue<float>(new object[] { construction, gridResource });
                                    if (absAmountUsed < amountAvailable) {
                                        t___instance_setResourceAvailable.GetValue(new object[] { construction, gridResource, true });
                                        if (constructionsToShutDown.Contains(construction)) {
                                            constructionsToShutDown.Remove(construction);
                                        }
                                        amountAvailable -= absAmountUsed;
                                        somethingChanged = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                if ((gridResource == GridResource.Water && !MessageLog.getInstance().contains(Message.StructuresNoWater)) ||
                    (gridResource == GridResource.Power && !MessageLog.getInstance().contains(Message.StructuresNoPower))) {
                    constructionsToShutDown.UnionWith(constructionsLackingResource);
                    t___instance.Method("addMessage").GetValue(new object[] { constructionsToShutDown, gridResource });
                }
            }

            resourceData.setCollector(collector);
            resourceData.setBalance(resourceBalance);
            resourceData.setGeneration(amountCreated);
            resourceData.setConsumption(amountConsumed);
        }
    }

}
