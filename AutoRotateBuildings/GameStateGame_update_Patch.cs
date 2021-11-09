using Planetbase;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace AutoRotateBuildings {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        /// <summary>
        /// mod specific:
        /// </summary>
        private static int connectionCount = 0;

        [HarmonyPostfix]
        public static void Postfix() {
            GameManager gameManager = GameManager.getInstance();
            GameManager.State mState = Traverse.Create(gameManager).Field<GameManager.State>("mState").Value;

            if (mState != GameManager.State.Updating)
                return;

            GameStateGame gameStateGame = GameManager.getInstance().getGameState() as GameStateGame;
            /* private enum GameStateGame.Mode */
            object mMode
                = Traverse.Create(gameStateGame).Field("mMode").GetValue();
            object GameStateGame_Mode_PlacingModule
                = Traverse.Create<GameStateGame>().Type("Mode").Field("PlacingModule").GetValue();
            Traverse t_gameStateGame_mActiveModule = Traverse.Create<GameStateGame>().Field("mActiveModule");
            Module activeModule = t_gameStateGame_mActiveModule.GetValue<Module>();

            if (
                object.Equals(gameStateGame, null) ||
                object.Equals(mMode, GameStateGame_Mode_PlacingModule) ||
                object.Equals(t_gameStateGame_mActiveModule.GetValue(), null)
                ) {
                return;
            }

            
            List<Vector3> connectionPositions = new List<Vector3>();
            List<Construction> Construction_mConstructions = Traverse.Create<Construction>().Field<List<Construction>>("mConstructions").Value;
            for (int i = 0; i < Construction_mConstructions.Count; ++i) {
                Module module = Construction_mConstructions[i] as Module;
                if (module != null && module != activeModule && Connection.canLink(activeModule, module)) {
                    connectionPositions.Add(module.getPosition());
                }
            }

            if (connectionPositions.Count == 0)
                return;

            connectionCount = Math.Min(connectionCount, connectionPositions.Count - 1);
            if (Input.GetKeyUp(KeyCode.R)) {
                connectionCount = ++connectionCount % connectionPositions.Count;
            }


            t_gameStateGame_mActiveModule.Field<GameObject>("mObject").Value.transform.localRotation = Quaternion.LookRotation((connectionPositions[connectionCount] - activeModule.getPosition()).normalized);
        }
    }
}
