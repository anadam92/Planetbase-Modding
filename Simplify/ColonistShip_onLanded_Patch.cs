using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace Simplifiy {

    public partial class Simplify  {

        [HarmonyPatch(typeof(ColonistShip), "onLanded", MethodType.Normal)]
        public abstract class ColonistShip_onLanded_Patch  {

            [HarmonyPrefix]
            public static void Prefix(ColonistShip __instance) {
                Traverse t___instance = Traverse.Create(__instance);

                float value = Singleton<Colony>.getInstance().getWelfareIndicator().getValue();
                int num = vistors;
                if (value > 0.9f) {
                    num += Random.Range(2, 4);
                }
                else if (value > 0.7f) {
                    num += Random.Range(1, 3);
                }
                if (t___instance.Field<ColonistShip.Size>("mSize").Value == ColonistShip.Size.Large) {
                    num *= 2;
                }
                bool __instance_mIntruders = t___instance.Field<bool>("mIntruders").Value;
                if (__instance_mIntruders) {
                    num += LandingShipManager.getExtraIntruders();
                }
                for (int i = 0; i < num; i++) {
                    Specialization specialization = ((!__instance_mIntruders) ? t___instance.Method("calculateSpecialization").GetValue<Specialization>() : TypeList<Specialization, SpecializationList>.find<Intruder>());
                    if (specialization != null) {
                        Character.create(
                            specialization,
                            t___instance.Method("getSpawnPosition").GetValue<Vector3>(new object[] { i }),
                            Location.Exterior
                        );
                    }
                }
            }

        }

    }

}
