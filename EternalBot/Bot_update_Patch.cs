using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Planetbase;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;


namespace EternalBot {

    [HarmonyPatch(typeof(Bot), "update", MethodType.Normal)]
    public class EternalBot_update_Patch {

        [HarmonyPrefix]
        public static bool Prefix(Bot __instance, float timeStep) {
            Traverse t___instance = Traverse.Create(__instance);

            ((Character)__instance).update(timeStep);

            Indicator indicator = new Indicator(StringList.get("integrity"), ResourceList.StaticIcons.Bot, IndicatorType.Condition, 1f, 1f, SignType.Condition);
            indicator.setLevels(0.05f, 0.1f, 0.15f, 0.2f);
            t___instance.Field<Indicator[]>("mIndicators").Value[7] = indicator;

            if (t___instance.Method("shouldDecay").GetValue<bool>()) {
                __instance.decayIndicator(CharacterIndicator.Condition, timeStep / 480f);
            }

            Disaster stormInProgress = Singleton<DisasterManager>.getInstance().getStormInProgress();
            if (stormInProgress != null && !__instance.isProtected()) {
                __instance.decayIndicator(CharacterIndicator.Condition, timeStep * stormInProgress.getIntensity() / 600f);
            }

            SolarFlare solarFlare = Singleton<DisasterManager>.getInstance().getSolarFlare();
            if (solarFlare.isInProgress() && !__instance.isProtected()) {
                __instance.decayIndicator(CharacterIndicator.Condition, timeStep * solarFlare.getIntensity() / 180f);
            }

            t___instance.Method("updateDustParticles").GetValue(new object[] { timeStep });

            return false;
        }

        //private static MethodInfo mi_getStormInProgress = typeof(DisasterManager).GetMethod("getStormInProgress");
        //private static MethodInfo mi_getSolarFlare = typeof(DisasterManager).GetMethod("getSolarFlare");
        //private static MethodInfo mi_updateDustParticles = typeof(Bot).GetMethod("updateDustParticles");

        //[HarmonyTranspiler]
        //public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {

        //    bool found_getStormInProgress = false;
        //    bool found_getSolarFlare = false;
        //    bool found_pop = false;

        //    foreach (CodeInstruction instruction in instructions) {
        //        found_getStormInProgress |= instruction.Calls(mi_getStormInProgress);
        //        found_getSolarFlare |= instruction.Calls(mi_getSolarFlare);
        //        if (found_getStormInProgress && found_getSolarFlare ) {
        //            found_pop |= instruction.opcode.Equals(OpCodes.Pop);
        //            if (found_pop) {
        //                if (instruction.Calls(mi_updateDustParticles)) {
        //                    yield return new CodeInstruction(OpCodes.Pop);
        //                    yield return new CodeInstruction(OpCodes.Ldarg_0);
        //                    yield return new CodeInstruction(OpCodes.Ldarg_1);
        //                    yield return instruction;
        //                    yield return new CodeInstruction(OpCodes.Ret);
        //                    break;
        //                }
        //                else {
        //                    continue;
        //                }
        //            }
        //        }
        //        yield return instruction;
        //    }

        //}

    }

}
