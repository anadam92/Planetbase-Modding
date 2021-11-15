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

        [HarmonyPostfix]
        public static void Postfix(ref bool __result ) {
            Planetbase.ShortcutManager.getInstance().addShortcut(
                KeyCode.Space,
                (object parameter) => {
                    if (Planetbase.TimeManager.getInstance().isPaused()) {
                        Planetbase.TimeManager.getInstance().unpause();
                    }
                    else {
                        Planetbase.TimeManager.getInstance().pause();
                    }
                },
                true
            );
        }

    }

}
