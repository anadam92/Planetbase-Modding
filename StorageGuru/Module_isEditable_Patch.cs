using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Planetbase;

namespace StorageGuru {

    [HarmonyPatch(typeof(Module), "isEditable", MethodType.Normal)]
    class Module_isEditable_Patch {

        [HarmonyPrefix]
        public static bool Prefix(Module __instance, bool __result) {
            __result = StorageMapping.isEditable(__instance.getModuleType());
            return false;
        }

    }

}
