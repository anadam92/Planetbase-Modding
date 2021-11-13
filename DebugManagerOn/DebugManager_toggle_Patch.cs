using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Planetbase;

namespace DebugManagerOn {

    [HarmonyPatch(typeof(DebugManager), "toggle", MethodType.Normal)]
    class DebugManager_toggle_Patch {

        [HarmonyPrefix]
        public static bool Prefix(DebugManager __instance) {
            Traverse<bool> t_mEnabled = Traverse.Create(__instance).Field<bool>("mEnabled");
            t_mEnabled.Value = !t_mEnabled.Value;
            System.Console.Beep();
            return false;
        }

    }
}
