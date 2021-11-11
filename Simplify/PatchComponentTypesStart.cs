using System.Collections.Generic;
using Planetbase;
using HarmonyLib;

namespace Simplifiy {

    internal static partial class ApplyPatches {

        public static void PatchComponentTypesStart() {
            int count = TypeList<ComponentType, ComponentTypeList>.getCount();
            for (int i = 0; i < count; i++) {
                ComponentType componentType = TypeList<ComponentType, ComponentTypeList>.get()[i];
                if (i == 0) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 3;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -300;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -190;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 1440f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 720f;
                }
                else if (i == 1) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -250;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -200;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 480f;
                }
                else if (i == 2) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 6;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -70;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -300;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 480f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 240f;
                }
                else if (i == 3) {
                    componentType.addResourceProduction<Starch>(ResourceSubtype.Peas);
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -200;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -210;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 480f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 240f;
                }
                else if (i == 4) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -280;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -250;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 480f;
                }
                else if (i == 5) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 3;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -300;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -270;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 480f;
                }
                else if (i == 6) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -230;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -460;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 480f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 240f;
                }
                else if (i == 7) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 1;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -380;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -350;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 480f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 240f;
                }
                else if (i == 8) {
                    componentType.addResourceProduction<Ore>(ResourceSubtype.Mushrooms);
                    componentType.addResourceProduction<Ore>(ResourceSubtype.Mushrooms);
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = -2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -110;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -220;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 2160f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 720f;
                }
                else if (i == 9) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    componentType.addResourceProduction<Ore>(ResourceSubtype.Radishes);
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 1;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -500;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -150;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 480f;
                }
                else if (i == 10) {
                    componentType.addResourceProduction<MedicinalPlants>();
                    componentType.addResourceProduction<Vegetables>();
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 2;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -250;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -250;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 640f;
                }
                else if (i == 11) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 3;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -390;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -370;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 480f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 240f;
                }
                else if (i == 37) {
                    componentType.addResourceProduction<Vegetables>(ResourceSubtype.Salad);
                    componentType.addResourceProduction<Starch>();
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -160;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -130;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 320f;
                }
                else if (i == 38) {
                    componentType.addResourceProduction<Vegetables>();
                    componentType.addResourceProduction<Ore>();
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -120;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -170;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 320f;
                }
                else if (i == 12) {
                    Traverse.Create(componentType).Field<int>("mOxygenGeneration").Value = 8;
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.PlantStorage;
                    Traverse.Create(componentType).Field<int>("mPowerGeneration").Value = -300;
                    Traverse.Create(componentType).Field<int>("mWaterGeneration").Value = -310;
                    Traverse.Create(componentType).Field<float>("mConditionDecayTime").Value = 960f;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 480f;
                }
                else if (i == 14) {
                    if (Simplify.CustomContainerStorage >= 1) {
                        Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.CustomContainerStorage;
                    }
                    else {
                        Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = 1;
                    }
                    Traverse.Create(componentType).Field<List<ResourceType>>("mStoredResources").Value = new List<ResourceType>();
                    Traverse.Create(componentType).Field<List<ResourceType>>("mStoredResources").Value.Add(TypeList<ResourceType, ResourceTypeList>.find<MedicalSupplies>());
                }
                else if (i == 23) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.Bioplasticproccesor_And_MetalproccesorStorage;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 90f;
                }
                else if (i == 24) {
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 90f;
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.Bioplasticproccesor_And_MetalproccesorStorage;
                }
                else if (i == 25) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.MealDrinksMedsStorage;
                }
                else if (i == 30) {
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.MealDrinksMedsStorage;
                    Traverse.Create(componentType).Field<float>("mResourceProductionPeriod").Value = 20f;
                }
                else if (i > 20 && i < 22) {
                    componentType.addResourceConsumption<AlcoholicDrink>();
                    for (int j = 1; j < Simplify.medkits; j++) {
                        componentType.addResourceProduction<MedicalSupplies>();
                    }
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = 15;
                }
                else if (i == 34) {
                    for (int k = 1; k < Simplify.gunz; k++) {
                        componentType.addResourceProduction<Gun>();
                    }
                    Traverse.Create(componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.Bioplasticproccesor_And_MetalproccesorStorage;
                }
            }
        }

    }
}
