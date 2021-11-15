using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace Pause {

    [HarmonyPatch(typeof(TimeManager), "isPaused", MethodType.Normal)]
    public class TimeManager_isPaused_Patch {

        [HarmonyPostfix]
        public static bool Prefix(ref bool __result ) {
            __result = false;
            return false;
        }

    }

}
