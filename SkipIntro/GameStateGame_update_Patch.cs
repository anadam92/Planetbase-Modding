
using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace SkipIntro {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    public class GameStateGame_update_Patch {

        private static IntroCinemetic m_intro = null;
        private static float CameraManager_VerticalAngle = Traverse.Create<CameraManager>().Field<float>("VerticalAngle").Value;

        [HarmonyPostfix]
        public static void Postfix() {
            if (m_intro == null) {
                m_intro = CameraManager.getInstance().getCinematic() as IntroCinemetic;
                if (m_intro == null)
                    return;
            }

            Traverse<ColonyShip> t_mColonyShip = Traverse.Create(m_intro).Field<ColonyShip>("mColonyShip");
            if (t_mColonyShip.Value.isDone()) {
                m_intro = null;
                return;
            }

            // Disable menu
            GameStateGame gameStateGame = GameManager.getInstance().getGameState() as GameStateGame;
            GameGui gameStateGame_mGameGui = Traverse.Create(gameStateGame).Field<GameGui>("mGameGui").Value;
            if (gameStateGame_mGameGui.getWindow() is GuiGameMenu)
                gameStateGame_mGameGui.setWindow(null);

            if (Input.GetKeyUp(KeyCode.Escape)) {
                if (CameraManager.getInstance().getCinematic() != null) {
                    Vector3 shipLandingPosition;
                    PhysicsUtil.findFloor(t_mColonyShip.Value.getPosition(), out shipLandingPosition, 256);
                    shipLandingPosition.y += CameraManager.DefaultHeight;

                    Transform transform = CameraManager.getInstance().getTransform();
                    transform.position = shipLandingPosition + t_mColonyShip.Value.getDirection().flatDirection() * 50f;
                    transform.LookAt(shipLandingPosition);

                    Vector3 euler = transform.eulerAngles;
                    euler.x = CameraManager_VerticalAngle;
                    transform.rotation = Quaternion.Euler(euler);

                    Traverse.Create(m_intro).Field<float>("mBlackBars").Value = 0f;
                    CameraManager.getInstance().setCinematic(null);
                }
            }
        }

    }

}
