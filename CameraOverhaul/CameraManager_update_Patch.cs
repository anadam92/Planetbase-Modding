using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Planetbase;
using UnityEngine;

namespace CameraOverhaul {

    [HarmonyPatch(typeof(CameraManager), "update", MethodType.Normal)]
    public static class CameraManager_update_Patch {

        [HarmonyPrefix]
        public static bool Prefix(CameraManager __instance, float timeStep) {
            CameraManagerProxy cmp = CameraManagerProxy.get(__instance);

            if (cmp.mZoomAxis == 0f) {
                cmp.mZoomAxis = Input.GetAxis("Zoom");
            }

            GameState gameState = GameManager.getInstance().getGameState();
            if (gameState != null && !gameState.isCameraFixed()) {
                if (cmp.mCinematic.Value == null) {
                    GameStateGame gameStateGame = gameState as GameStateGame;
                    Traverse t_gameStateGame = Traverse.Create(gameStateGame);

                    if (gameStateGame != null && t_gameStateGame.Field("mMode").GetValue() == CameraOverhaul.GameStateGame_Mode_PlacingModule && cmp.mIsPlacingModule) {
                        t_gameStateGame.Field("mCurrentModuleSize").SetValue(cmp.mModulesize);
                    }

                    Transform transform = cmp.mMainCamera.Value.transform;

                    float xAxis = cmp.mAcceleration.Value.x;
                    float yAxis = cmp.mAcceleration.Value.y;
                    float zAxis = cmp.mAcceleration.Value.z;
                    float absXAxis = Mathf.Abs(xAxis);
                    float absYAxis = Mathf.Abs(yAxis);
                    float absZAxis = Mathf.Abs(zAxis);

                    if (!cmp.mLocked.Value) {
                        // if zooming
                        if (absYAxis > 0.01f) {
                            float speed = Mathf.Clamp(60f * timeStep, 0.01f, 100f);
                            float newHeight = Mathf.Clamp(cmp.mCurrentHeight.Value + yAxis * speed, CameraOverhaul.MIN_HEIGHT, CameraOverhaul.MAX_HEIGHT);

                            if (transform.eulerAngles.x < 86f) {
                                zAxis += (cmp.mCurrentHeight.Value - newHeight) / speed;
                                absZAxis = Mathf.Abs(zAxis);
                            }

                            cmp.mCurrentHeight.Value = newHeight;
                            cmp.mTargetHeight.Value = cmp.mCurrentHeight.Value;
                        }

                        // Move forwards
                        if (absZAxis > 0.001f) {
                            transform.position = transform.position + new Vector3(transform.forward.x, 0f, transform.forward.z).normalized * zAxis * timeStep * 80f;
                        }

                        // Move sideways
                        if (absXAxis > 0.001f) {
                            transform.position = transform.position + new Vector3(transform.right.x, 0f, transform.right.z).normalized * xAxis * timeStep * 80f;
                        }

                        // rotate around cam
                        Vector3 eulerAngles = transform.eulerAngles;
                        if (Mathf.Abs(cmp.mRotationAcceleration) > 0.01f) {
                            eulerAngles.y = eulerAngles.y + cmp.mRotationAcceleration * timeStep * 120f;
                        }

                        if (Mathf.Abs(cmp.mVerticalRotationAcceleration) > 0.01f) {
                            eulerAngles.x = Mathf.Clamp(eulerAngles.x - cmp.mVerticalRotationAcceleration * timeStep * 120f, 20f, 87f);
                        }
                        transform.eulerAngles = eulerAngles;
                    }
                    else if (absYAxis > 0.01f) {
                        float speed = Mathf.Clamp(60f * timeStep, 0.01f, 100f);
                        Vector3 movement = transform.forward * speed * -yAxis;

                        Vector3 planePoint = Selection.getSelectedConstruction().getPosition();
                        planePoint.y = yAxis < 0f ? 4f : Selection.getSelectedConstruction().getRadius() + 10f;
                        Plane plane = new Plane(Vector3.up, planePoint);

                        Ray ray = new Ray(transform.position, yAxis < 0f ? transform.forward : -transform.forward);
                        float dist;
                        if (plane.Raycast(ray, out dist)) {
                            if (dist < movement.magnitude)
                                movement *= dist / movement.magnitude;

                            transform.position = transform.position + movement;
                        }
                    }

                    // rotate around world
                    if (Mathf.Abs(cmp.mAlternateRotationAcceleration) > 0.01f) {
                        Ray ray = new Ray(transform.position, transform.forward);
                        float dist;
                        if (CameraOverhaul.mGroundPlane.Raycast(ray, out dist)) {
                            transform.RotateAround(transform.position + transform.forward * dist, Vector3.up, cmp.mAlternateRotationAcceleration * timeStep * 120f);
                        }
                    }

                    // if we moved, set the correct height
                    if (!cmp.mLocked.Value && (absZAxis > 0.001f || absXAxis > 0.001f || absYAxis > 0.01f)) {
                        cmp.placeOnFloor(__instance, new object[] { cmp.mCurrentHeight.Value });
                    }

                    // Calc map center and distance
                    Vector3 mapCenter = new Vector3(CameraOverhaul.TerrainGenerator_TotalSize, 0f, CameraOverhaul.TerrainGenerator_TotalSize) * 0.5f;
                    Vector3 mapCenterToCam = transform.position - mapCenter;
                    float distToMapCenter = mapCenterToCam.magnitude;

                    // limit cam to 375 units from center
                    if (distToMapCenter > 375f) {
                        cmp.mMainCamera.Value.transform.position = mapCenter + mapCenterToCam.normalized * 375f;
                    }
                }
                else {
                    cmp.updateCinematic(__instance, new object[] { timeStep });
                }
            }

            // interpolate the position when the game moves the camera to a specific location (e.g when editing a building)
            if (cmp.mCameraTransition.Value < 1f) {
                if (cmp.mTransitionTime.Value == 0f) {
                    cmp.mCameraTransition.Value = 1f;
                }
                else {
                    float num4 = timeStep / cmp.mTransitionTime.Value;
                    cmp.mCameraTransition.Value = cmp.mCameraTransition.Value + num4;
                }
                cmp.mCurrentTransform.Value.interpolate(cmp.mSourceTransform.Value, cmp.mTargetTransform.Value, cmp.mCameraTransition.Value);
                cmp.mCurrentTransform.Value.apply(cmp.mMainCamera.Value.transform);
            }

            cmp.mSkydomeCamera.Value.transform.rotation = cmp.mMainCamera.Value.transform.rotation;

            return false;
        }

    }

}
