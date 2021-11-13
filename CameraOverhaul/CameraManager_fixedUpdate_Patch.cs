using Planetbase;
using HarmonyLib;
using UnityEngine;

namespace CameraOverhaul {

    [HarmonyPatch(typeof(CameraManager), "fixedUpdate", MethodType.Normal)]
    public static class CameraManager_fixedUpdate_Patch {

        [HarmonyPrefix]
        public static bool Prefix(CameraManager __instance, float timeStep, int frameIndex) {
            CameraManagerProxy cmp = CameraManagerProxy.get(__instance);

            if (cmp.mCinematic.Value == null) {
                float lateralMoveSpeed = timeStep * 6f;
                float zoomAndRotationSpeed = timeStep * 10f;

                GameState gameState = GameManager.getInstance().getGameState();

                // this only happens when placing a module and only if current height < 21
                if (cmp.mTargetHeight.Value != cmp.mCurrentHeight.Value) {
                    cmp.mCurrentHeight.Value += Mathf.Sign(cmp.mTargetHeight.Value - cmp.mCurrentHeight.Value) * timeStep * 30f;
                    if (Mathf.Abs(cmp.mCurrentHeight.Value - cmp.mTargetHeight.Value) < 0.5f) {
                        cmp.mCurrentHeight.Value = cmp.mTargetHeight.Value;
                    }
                }

                if (gameState != null && !gameState.isCameraFixed() && !TimeManager.getInstance().isPaused()) {
                    KeyBindingManager keyBindingManager = KeyBindingManager.getInstance();
                    GameStateGame gameStateGame = gameState as GameStateGame;
                    Traverse t_gameStateGame = Traverse.Create(gameStateGame);
                    if (gameStateGame != null && t_gameStateGame.Field("mMode").GetValue() == CameraOverhaul.GameStateGame_Mode_PlacingModule) {

                        Traverse<int> t_mCurrentModuleSize = t_gameStateGame.Field<int>("mCurrentModuleSize");

                        if (!cmp.mIsPlacingModule) {
                            cmp.mIsPlacingModule = true;
                            cmp.mModulesize = t_mCurrentModuleSize.Value;
                        }

                        // we're zooming
                        if (Mathf.Abs(cmp.mZoomAxis) > 0.001f || Mathf.Abs(keyBindingManager.getCompositeAxis(ActionType.CameraZoomOut, ActionType.CameraZoomIn)) > 0.001f) {
                            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                                ModuleType mPlacedModuleType = t_gameStateGame.Field<ModuleType>("mPlacedModuleType").Value;
                                if (cmp.mZoomAxis <= -0.1f || keyBindingManager.getBinding(ActionType.CameraZoomOut).justUp()) {
                                    if (cmp.mModulesize > mPlacedModuleType.getMinSize()) {
                                        cmp.mModulesize--;
                                    }
                                }
                                else if ((cmp.mZoomAxis >= 0.1f || keyBindingManager.getBinding(ActionType.CameraZoomIn).justUp()) && cmp.mModulesize < mPlacedModuleType.getMaxSize()) {
                                    cmp.mModulesize++;
                                }
                            }
                        }

                        t_mCurrentModuleSize.Value = cmp.mModulesize;
                    }
                    else {
                        cmp.mIsPlacingModule = false;
                    }

                    Vector3 oldAcceleration = cmp.mAcceleration.Value;
                    float newAccelerationX = oldAcceleration.x;
                    float newAccelerationY = oldAcceleration.y;
                    float newAccelerationZ = oldAcceleration.z;

                    newAccelerationX += keyBindingManager.getCompositeAxis(ActionType.CameraMoveLeft, ActionType.CameraMoveRight) * lateralMoveSpeed;
                    newAccelerationZ += keyBindingManager.getCompositeAxis(ActionType.CameraMoveBack, ActionType.CameraMoveForward) * lateralMoveSpeed;

                    if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) {
                        newAccelerationY -= cmp.mZoomAxis * zoomAndRotationSpeed;
                        newAccelerationY -= keyBindingManager.getCompositeAxis(ActionType.CameraZoomOut, ActionType.CameraZoomIn) * zoomAndRotationSpeed;
                    }

                    cmp.mAlternateRotationAcceleration -= keyBindingManager.getCompositeAxis(ActionType.CameraRotateLeft, ActionType.CameraRotateRight) * zoomAndRotationSpeed;

                    // Rotate with middle mouse button
                    if (Input.GetMouseButton(2)) {
                        float mouseDeltaX = Input.mousePosition.x - cmp.mPreviousMouseX;
                        if (Mathf.Abs(mouseDeltaX) > Mathf.Epsilon)
                            cmp.mRotationAcceleration += zoomAndRotationSpeed * mouseDeltaX * 0.1f;

                        float mouseDeltaY = Input.mousePosition.y - cmp.mPreviousMouseY;
                        if (Mathf.Abs(mouseDeltaY) > Mathf.Epsilon)
                            cmp.mVerticalRotationAcceleration += zoomAndRotationSpeed * mouseDeltaY * 0.1f;
                    }

                    // Move with mouse on screen borders
                    if (!Application.isEditor) {
                        float screenBorder = Screen.height * 0.01f;
                        if (Input.mousePosition.x < screenBorder) {
                            newAccelerationX -= lateralMoveSpeed;
                        }
                        else if (Input.mousePosition.x > Screen.width - screenBorder) {
                            newAccelerationX += lateralMoveSpeed;
                        }
                        if (Input.mousePosition.y < screenBorder) {
                            newAccelerationZ -= lateralMoveSpeed;
                        }
                        else if (Input.mousePosition.y > Screen.height - screenBorder) {
                            newAccelerationZ += lateralMoveSpeed;
                        }
                    }

                    float clampSpeed = !Input.GetKey(KeyCode.LeftShift) ? 1f : 0.25f;

                    newAccelerationX = Mathf.Clamp(newAccelerationX * (1 - lateralMoveSpeed), -clampSpeed, clampSpeed);
                    newAccelerationY = Mathf.Clamp(newAccelerationY * (1 - zoomAndRotationSpeed), -clampSpeed, clampSpeed);
                    newAccelerationZ = Mathf.Clamp(newAccelerationZ * (1 - lateralMoveSpeed), -clampSpeed, clampSpeed);

                    cmp.mAcceleration.Value = new Vector3(newAccelerationX, newAccelerationY, newAccelerationZ);
                    cmp.mRotationAcceleration = Mathf.Clamp(cmp.mRotationAcceleration * (1 - zoomAndRotationSpeed), -clampSpeed, clampSpeed);
                    cmp.mVerticalRotationAcceleration = Mathf.Clamp(cmp.mVerticalRotationAcceleration * (1 - zoomAndRotationSpeed), -clampSpeed, clampSpeed);
                    cmp.mAlternateRotationAcceleration = Mathf.Clamp(cmp.mAlternateRotationAcceleration * (1 - zoomAndRotationSpeed), -clampSpeed, clampSpeed);
                }
                else {
                    cmp.mAcceleration.Value = Vector3.zero;
                    cmp.mRotationAcceleration = 0f;
                    cmp.mVerticalRotationAcceleration = 0f;
                    cmp.mAlternateRotationAcceleration = 0f;
                }

                cmp.mPreviousMouseX = Input.mousePosition.x;
                cmp.mPreviousMouseY = Input.mousePosition.y;
            }
            else {
                cmp.mCinematic.Value.fixedUpdate(timeStep);
            }

            cmp.mZoomAxis = 0f;


            return false;
        }

    }
}
