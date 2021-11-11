using Planetbase;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace BetterHours {

    [HarmonyPatch(typeof(Indicator), "getTimeHour", MethodType.Normal)]
    class Indicator_getTimeHour_Patch {

        [HarmonyPostfix]
        public static bool Prefix(ref int __result, float value) {
            double dayHours = BetterHours.getDayHours();
            __result = (int)(((double)value * dayHours + 6.0) % dayHours);
            return false;
        }

    }

}
