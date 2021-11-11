using Planetbase;
using HarmonyLib;

namespace Simplifiy {

    internal static partial class ApplyPatches {

        public static void PatchSpecialisations() {
            int count = TypeList<ComponentType, ComponentTypeList>.getCount();
            for (int i = 0; i < count; i++) {
                ComponentType componentType = TypeList<ComponentType, ComponentTypeList>.get()[i];
                if (i < 13) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                        = TypeList<Specialization, SpecializationList>.find<Medic>();
                    continue;
                }
                if (i > 20 && i < 23) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value 
                        = TypeList<Specialization, SpecializationList>.find<Medic>();
                    continue;
                }
                if (i > 22 && i < 25) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                        = TypeList<Specialization, SpecializationList>.find<Engineer>();
                    continue;
                }
                if (i > 31 && i < 36) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                        = TypeList<Specialization, SpecializationList>.find<Engineer>();
                    continue;
                }
                int num;
                switch (i) {
                    case 36:
                        componentType.addResourceProduction<Vegetables>(ResourceSubtype.Basic);
                        Traverse.Create( componentType).Field<int>("mWaterGeneration").Value= -70;
                        Traverse.Create( componentType).Field<int>("mPowerGeneration").Value = -80;
                        Traverse.Create( componentType).Field<int>("mOxygenGeneration").Value = 1;
                        Traverse.Create( componentType).Field<float>("mConditionDecayTime").Value = 2160f;
                        Traverse.Create( componentType).Field<float>("mResourceProductionPeriod").Value = 180f;
                        Traverse.Create( componentType).Field<int>("mEmbeddedResourceCount").Value = Simplify.CustomContainerStorage;
                        Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                            = TypeList<Specialization, SpecializationList>.find<Biologist>();
                        continue;
                    default:
                        num = ((i == 37) ? 1 : 0);
                        break;
                    case 38:
                        num = 1;
                        break;
                }
                if (num != 0) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                        = TypeList<Specialization, SpecializationList>.find<Medic>();
                }
                else if (i > 38 && i < 42) {
                    Traverse.Create( componentType).Field<Specialization>("mOperatorSpecialization").Value
                        = TypeList<Specialization, SpecializationList>.find<Guard>();
                }
            }
        }

    }
}
