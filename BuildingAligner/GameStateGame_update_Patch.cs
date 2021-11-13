using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace BuildingAligner {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    public class GameStateGame_update_Patch {

        private static Type type_DebugRenderer = Assembly.GetAssembly(typeof(GameManager)).GetType("Planetbase.DebugRenderer");
        private static Traverse t_DebugRenderer = Traverse.Create(type_DebugRenderer);
        private static object GameStateGame_Mode_PlacingModule = Traverse.Create<GameStateGame>().Type("Mode").Field("PlacingModule").GetValue();

        [HarmonyPostfix]
        public static void Postfix() {
            GameStateGame gameStateGame = GameManager.getInstance().getGameState() as GameStateGame;
            if (gameStateGame != null && !object.Equals(Traverse.Create(gameStateGame).Field("mMode").GetValue(), GameStateGame_Mode_PlacingModule)) {
                GameStateGame_tryPlaceModule_Patch.rendering = false;
                MethodInvoker.GetHandler(AccessTools.DeclaredMethod(type_DebugRenderer, "clearGroup")).Invoke(null, new object[] { "Connections" });
            }
        }

    }

}
