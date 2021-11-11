using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace Simplifiy {

    public partial class Simplify {

        [HarmonyPatch(typeof(VisitorShip), "onLandedGeneric", MethodType.Normal)]
        public abstract class VisitorShip_onLandedGeneric_Patch{

            [HarmonyPrefix]
            public static bool Prefix(VisitorShip __instance) {
                Traverse t___instance = Traverse.Create(__instance);
                Traverse<int> t___instance_mPendingVisitors = t___instance.Field<int>("mPendingVisitors");
                bool __instance_mIntruders = t___instance.Field<bool>("mIntruders").Value;

                float value = Singleton<Colony>.getInstance().getWelfareIndicator().getValue();
                int num = 10;
                if (boyboy != 0) {
                    num = boyboy;
                }
                if (value > 0.9f) {
                    num += Random.Range(2, 4);
                }
                else if (value > 0.7f) {
                    num += Random.Range(1, 3);
                }
                if (t___instance.Field<VisitorShip.Size>("mSize").Value == VisitorShip.Size.Large) {
                    num *= 2;
                }
                CodeInstruction ci;
                System.Reflection.Emit.Label l;
                if (__instance_mIntruders) {
                    num += LandingShipManager.getExtraIntruders();
                    for (int i = 0; i < num; i++) {
                        Character.create(TypeList<Specialization, SpecializationList>.find<Intruder>(), __instance.getPosition(), Location.Exterior);
                        t___instance_mPendingVisitors.Value = 0;
                    }
                    return false;
                }
                t___instance_mPendingVisitors.Value = num;
                for (int j = 0; j < num; j++) {
                    Guest guest = (Guest)Character.create(
                        TypeList<Specialization, SpecializationList>.find<Visitor>(),
                        t___instance.Method("getSpawnPosition").GetValue<Vector3>(new object[] { j }),
                        Location.Exterior
                    );
                    guest.decayIndicator(CharacterIndicator.Nutrition, Random.Range(0f, 0.75f));
                    guest.decayIndicator(CharacterIndicator.Morale, Random.Range(0f, 1f));
                    guest.decayIndicator(CharacterIndicator.Hydration, Random.Range(0f, 0.75f));
                    guest.decayIndicator(CharacterIndicator.Sleep, Random.Range(0f, 0.75f));
                    guest.setFee(5 * Random.Range(2, 5));
                    guest.setOwnedShip(__instance);
                    if (Random.Range(0, 20) == 0) {
                        guest.setCondition(TypeList<ConditionType, ConditionTypeList>.find<ConditionFlu>());
                    }
                }

                return false;
            }

        }

    }

}
