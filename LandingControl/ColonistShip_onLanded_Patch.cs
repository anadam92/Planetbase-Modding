using Planetbase;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace LandingControl {

    [HarmonyPatch(typeof(ColonistShip), "onLanded", MethodType.Normal)]
    public class ColonistShip_onLanded_Patch {

        [HarmonyPrefix]
        public static bool Prefix(ColonistShip __instance) {
            NavigationGraph.getExterior().addBlocker(__instance.getPosition() + __instance.getTransform().forward, __instance.getRadius());

            int numNewColonists = 2;

            float welfare = Colony.getInstance().getWelfareIndicator().getValue();
            if (welfare > 0.9f) {
                numNewColonists = Random.Range(3, 6);
            }
            else if (welfare > 0.7f) {
                numNewColonists = Random.Range(2, 5);
            }
            else if (welfare > 0.5f) {
                numNewColonists = Random.Range(2, 4);
            }
            else if (welfare < 0.2f) {
                numNewColonists = 1;
            }

            if (Traverse.Create(__instance).Field<LandingShip.Size>("mSize").Value == LandingShip.Size.Large) {
                numNewColonists++;
            }

            bool __instance_mIntruders = Traverse.Create(__instance).Field<bool>("mIntruders").Value;
            if (__instance_mIntruders) {
                numNewColonists += LandingShipManager.getExtraIntruders();
            }

            LandingPermissions landingPermissions = LandingShipManager.getInstance().getLandingPermissions();
            Dictionary<Specialization, RefInt> landingPermissions_mSpecializationPercentages = Traverse.Create(landingPermissions).Field<Dictionary<Specialization, RefInt>>("mSpecializationPercentages").Value;
            RefBool landingPermissions_mColonistsAllowed = Traverse.Create(landingPermissions).Field<RefBool>("mColonistsAllowed").Value;
            for (int i = 0; i < numNewColonists; i++) {
                Specialization specialization = (!__instance_mIntruders) ? GetSpecialiation(landingPermissions) : TypeList<Specialization, SpecializationList>.find<Intruder>();
                if (specialization != null) {
                    Character.create(specialization, Traverse.Create(__instance).Method("getSpawnPosition").GetValue<Vector3>(new object[]{i}), Location.Exterior);

                    if (!__instance_mIntruders) {
                        landingPermissions_mSpecializationPercentages[specialization].set(landingPermissions_mSpecializationPercentages[specialization].get() - 1);

                        bool anyAllowed = false;
                        foreach (Specialization spec in SpecializationList.getColonistSpecializations()) {
                            if (landingPermissions.getSpecializationPercentage(spec).get() > 0) {
                                anyAllowed = true;
                                break;
                            }
                        }

                        if (!anyAllowed) {
                            landingPermissions_mColonistsAllowed.set(false);
                        }
                    }
                }
            }

            return false;
        }

        private static Specialization GetSpecialiation(LandingPermissions landingPermissions) {
            List<Specialization> potentialChoices = new List<Specialization>();

            foreach (Specialization specialization in SpecializationList.getColonistSpecializations()) {
                if (landingPermissions.getSpecializationPercentage(specialization).get() > 0)
                    potentialChoices.Add(specialization);
            }

            if (potentialChoices.Count > 0)
                return potentialChoices[Random.Range(0, potentialChoices.Count)];

            return null;
        }
    }
}
