using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Planetbase;

namespace DebugManagerOn {

    [HarmonyPatch(typeof(GameManager), "onGui", MethodType.Normal)]
    class GameManager_onGui_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            Planetbase.DebugManager.getInstance().onGui();
        }

    }
}
