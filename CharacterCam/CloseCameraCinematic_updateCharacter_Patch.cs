using Planetbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace CharacterCam {

    [HarmonyPatch(typeof(CloseCameraCinematic), "updateCharacter", MethodType.Normal)]
    static class GameGuiManager_setTimeScale_Patch {

        [HarmonyPrefix]
        static bool Prefix(CloseCameraCinematic __instance, Character character, float timeStep) {
            Traverse t___instance = Traverse.Create(__instance);
            Traverse<float> t_mLastRotation = t___instance.Field<float>("mLastRotation");

            Transform cameraTransform = CameraManager.getInstance().getTransform();
            Transform characterTransform = character.getTransform();

            float yAngle = characterTransform.eulerAngles.y;
            float horizontalBobbing = Mathf.Clamp((t_mLastRotation.Value - yAngle) * 0.25f, -0.5f, 0.5f);
            Vector3 newPos = characterTransform.position + Vector3.up * character.getHeight() + characterTransform.forward * 0.7f + horizontalBobbing * characterTransform.right;
            if (t___instance.Field<bool>("mFirstUpdate").Value) {
                cameraTransform.position = newPos;
                cameraTransform.rotation = characterTransform.rotation;
                t_mLastRotation.Value = yAngle;
            }
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, 0.1f);
            Vector3 lookAtDir = (characterTransform.forward * 1.4f + Vector3.up * (character.getHeight() * 0.85f)).normalized;
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, Quaternion.LookRotation(lookAtDir), timeStep * 120f);
            t_mLastRotation.Value = yAngle;

            return false;
        }
        
    }

}
