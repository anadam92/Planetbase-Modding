using Planetbase;
using HarmonyLib;

namespace Simplifiy {

    internal static partial class ApplyPatches {

        public static void PatchModuleTypes() {
            int count = TypeList<ModuleType, ModuleTypeList>.getCount();
            for (int i = 0; i < count; i++) {
                ModuleType moduleType = TypeList<ModuleType, ModuleTypeList>.get()[i];
                if (moduleType != null) {
                    Traverse t_moduleType = Traverse.Create(moduleType);
                    switch (i) {
                        case 0:
                            t_moduleType.Field<int>("mOxygenGeneration").Value = 5;
                            t_moduleType.Field<int>("mPowerGeneration").Value = -2000;
                            break;
                        case 1:
                            t_moduleType.Field<ModuleTypeRef>("mRequiredStructure").Value.set<ModuleTypeBioDome>();
                            break;
                        case 2:
                            t_moduleType.Field<ComponentType[]>("mComponentTypes").Value = new ComponentType[7]
						{
							TypeList<ComponentType, ComponentTypeList>.find<MealMaker>(),
							TypeList<ComponentType, ComponentTypeList>.find<TableSmall>(),
							TypeList<ComponentType, ComponentTypeList>.find<Table>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinkingFountain>(),
							TypeList<ComponentType, ComponentTypeList>.find<VideoScreen>(),
							TypeList<ComponentType, ComponentTypeList>.find<DecorativePlant>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinksMachine>()
						};
                            moduleType.initStrings();
                            break;
                        case 5:
                            t_moduleType.Field<int>("mMinSize").Value = 1;
                            t_moduleType.Field<int>("mMaxSize").Value = 4;
                            t_moduleType.Field<int>("mExtraSize").Value = 4;
                            t_moduleType.Field<int[]>("mBaseCosts").Value = new int[5] { 0, 2, 5, 7, 10 };
                            t_moduleType.Field<ComponentType[]>("mComponentTypes").Value = new ComponentType[43]
						{
							TypeList<ComponentType, ComponentTypeList>.find<Bunk>(),
							TypeList<ComponentType, ComponentTypeList>.find<Bed>(),
							TypeList<ComponentType, ComponentTypeList>.find<SickBayBed>(),
							TypeList<ComponentType, ComponentTypeList>.find<Workbench>(),
							TypeList<ComponentType, ComponentTypeList>.find<MedicinalPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MealMaker>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinksMachine>(),
							TypeList<ComponentType, ComponentTypeList>.find<GmTomatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<LettucePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<TissueSynthesizer>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinkingFountain>(),
							TypeList<ComponentType, ComponentTypeList>.find<BarTableNoChairs>(),
							TypeList<ComponentType, ComponentTypeList>.find<BarTable>(),
							TypeList<ComponentType, ComponentTypeList>.find<TableSmall>(),
							TypeList<ComponentType, ComponentTypeList>.find<Table>(),
							TypeList<ComponentType, ComponentTypeList>.find<MetalProcessor>(),
							TypeList<ComponentType, ComponentTypeList>.find<RadishPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MushroomPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<OakTree>(),
							TypeList<ComponentType, ComponentTypeList>.find<ExerciseBar>(),
							TypeList<ComponentType, ComponentTypeList>.find<BioplasticProcessor>(),
							TypeList<ComponentType, ComponentTypeList>.find<RicePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<WheatPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<PineTree>(),
							TypeList<ComponentType, ComponentTypeList>.find<Bench>(),
							TypeList<ComponentType, ComponentTypeList>.find<RadioConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<TelescopeConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<TomatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<GmOnionPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<OnionPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<SparesWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<SemiconductorFoundry>(),
							TypeList<ComponentType, ComponentTypeList>.find<BotAutoRepair>(),
							TypeList<ComponentType, ComponentTypeList>.find<BotWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<ArmsWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<Treadmill>(),
							TypeList<ComponentType, ComponentTypeList>.find<DecorativePlant>(),
							TypeList<ComponentType, ComponentTypeList>.find<SecurityConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<Armory>(),
							TypeList<ComponentType, ComponentTypeList>.find<MedicalCabinet>(),
							TypeList<ComponentType, ComponentTypeList>.find<PotatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MaizePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<PeaPad>()
						};
                            t_moduleType.Field<ModuleTypeRef>("mRequiredStructure").Value.set<ModuleTypeSolarPanel>();
                            moduleType.initStrings();
                            break;
                        case 14:
                            t_moduleType.Field<ComponentType[]>("mComponentTypes").Value = new ComponentType[43]
						{
							TypeList<ComponentType, ComponentTypeList>.find<Bunk>(),
							TypeList<ComponentType, ComponentTypeList>.find<Bed>(),
							TypeList<ComponentType, ComponentTypeList>.find<SickBayBed>(),
							TypeList<ComponentType, ComponentTypeList>.find<Workbench>(),
							TypeList<ComponentType, ComponentTypeList>.find<MedicinalPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MealMaker>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinksMachine>(),
							TypeList<ComponentType, ComponentTypeList>.find<GmTomatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<LettucePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<TissueSynthesizer>(),
							TypeList<ComponentType, ComponentTypeList>.find<DrinkingFountain>(),
							TypeList<ComponentType, ComponentTypeList>.find<BarTableNoChairs>(),
							TypeList<ComponentType, ComponentTypeList>.find<BarTable>(),
							TypeList<ComponentType, ComponentTypeList>.find<TableSmall>(),
							TypeList<ComponentType, ComponentTypeList>.find<Table>(),
							TypeList<ComponentType, ComponentTypeList>.find<MetalProcessor>(),
							TypeList<ComponentType, ComponentTypeList>.find<RadishPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MushroomPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<OakTree>(),
							TypeList<ComponentType, ComponentTypeList>.find<ExerciseBar>(),
							TypeList<ComponentType, ComponentTypeList>.find<BioplasticProcessor>(),
							TypeList<ComponentType, ComponentTypeList>.find<RicePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<WheatPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<PineTree>(),
							TypeList<ComponentType, ComponentTypeList>.find<Bench>(),
							TypeList<ComponentType, ComponentTypeList>.find<RadioConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<TelescopeConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<TomatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<GmOnionPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<OnionPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<SparesWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<SemiconductorFoundry>(),
							TypeList<ComponentType, ComponentTypeList>.find<BotAutoRepair>(),
							TypeList<ComponentType, ComponentTypeList>.find<BotWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<ArmsWorkshop>(),
							TypeList<ComponentType, ComponentTypeList>.find<Treadmill>(),
							TypeList<ComponentType, ComponentTypeList>.find<DecorativePlant>(),
							TypeList<ComponentType, ComponentTypeList>.find<SecurityConsole>(),
							TypeList<ComponentType, ComponentTypeList>.find<Armory>(),
							TypeList<ComponentType, ComponentTypeList>.find<MedicalCabinet>(),
							TypeList<ComponentType, ComponentTypeList>.find<PotatoPad>(),
							TypeList<ComponentType, ComponentTypeList>.find<MaizePad>(),
							TypeList<ComponentType, ComponentTypeList>.find<PeaPad>()
						};
                            t_moduleType.Field<ModuleTypeRef>("mRequiredStructure").Value.set<ModuleTypeSolarPanel>();
                            moduleType.initStrings();
                            break;
                    }
                    if (t_moduleType.Field<float>("mCondicionDecayTime").Value > 0f) {
                        t_moduleType.Field<float>("mCondicionDecayTime").Value = 7200f;
                    }
                }
            }
        }

    }
}
