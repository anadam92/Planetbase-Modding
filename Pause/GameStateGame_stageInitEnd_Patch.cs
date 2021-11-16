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

    [HarmonyPatch(typeof(GameStateGame), "stageInitEnd", MethodType.Normal)]
    public class GameStateGame_stageInitEnd_Patch {

        private static bool isPaused = false;

        [HarmonyPostfix]
        public static void Postfix(ref bool __result, GameStateGame __instance) {
            TimeManager tm = Planetbase.TimeManager.getInstance();
            Planetbase.ShortcutManager.getInstance().addShortcut(
                KeyCode.Space,
                (object parameter) => {
                    if (isPaused) {
                        tm.unpause();
                        isPaused = false;
                        string msg = String.Format("{0} x{1}", StringList.get("speed_set"), tm.getTimeScale());
                        Traverse.Create(__instance).Method("addToast").GetValue(new object[] { msg });
                    }
                    else {
                        tm.pause();
                        isPaused = true;
                        string msg = "Game paused";
                        Traverse.Create(__instance).Method("addToast").GetValue(new object[] { msg });
                    }
                },
                true
            );
        }

    }

}
