using Planetbase;
using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using HarmonyLib;

namespace CameraOverhaul {

    public class CameraManagerCustom : Planetbase.CameraManager {

        public static new Planetbase.CameraManager getInstance() {
            if (mInstance == null) {
                mInstance = new CameraManagerCustom();
            }
            return mInstance;
        }

        #region "constants grabbed using reflection"
        private static object GameStateGame_Mode_PlacingModule = Traverse.Create<GameStateGame>().Type("Mode").Field("PlacingModule").GetValue();
        private static float TerrainGenerator_TotalSize = Traverse.Create<TerrainGenerator>().Field<float>("TotalSize").Value;
        #endregion

        #region "from original: float constants"
        private const float MinMovement = 0.01f;
        private const float TranslationStep = 80f;
        private const float ZoomStep = 60f;
        private const float RotationStep = 120f;
        private const float MinHeight = 12f;
        private const float MaxHeight = 30f;
        private const float HeightTolerance = 7.5f;
        private const float MinDisplacement = 0.001f;
        private const float VerticalAngle = 25f;
        private const float TitleClipDistance = 50000f;
        private const float GameClipDistance = 20000f;
        public new const float DefaultHeight = 21f;
        public new const float DefaultFov = 60f;
        public new const float DefaultNearClipPlane = 0.5f;
        #endregion

        #region "from CamerOverhaul"
        // These are not const so other mods can change them if they want
        public static float MIN_HEIGHT = 12f;
        public static float MAX_HEIGHT = 120f;
        private static float mVerticalRotationAcceleration = 0f;
        private static float mPreviousMouseY = 0f;
        private static float mAlternateRotationAcceleration = 0f;
        private static Plane mGroundPlane = new Plane(Vector3.up, new Vector3(TerrainGenerator_TotalSize, 0f, TerrainGenerator_TotalSize) * 0.5f);
        private static int mModulesize = 0;
        private static bool mIsPlacingModule = false;
        #endregion

        private static CameraManagerCustom mInstance = null;
        public static new readonly Quaternion StartupOrientation = Quaternion.Euler(25f, 180f, 0f);
        private static readonly SimpleTransform DefaultCameraTransform = new SimpleTransform(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));

        private Camera mMainCameraComponent;
        private GameObject mSkydomeCamera;
        private Cinematic mCinematic;
        private SimpleTransform mTargetTransform;
        private SimpleTransform mSourceTransform;
        private SimpleTransform mCurrentTransform = new SimpleTransform();
        private float mCameraTransition = 1f;
        private float mTransitionTime;
        private bool mLocked;
        private bool mZoomLocked;
        private float mCurrentHeight = 21f;
        private float mTargetHeight = 21f;
        private Vector3 mAcceleration = Vector3.zero;
        private float mRotationAcceleration;
        private float mPreviousMouseX;
        private bool mCapsLock;
        private float mZoomAxis;
        private GameObject mMainCamera;

        // properties

        public new GameObject getCamera() {
            return mMainCamera;
        }

        public new Cinematic getCinematic() {
            return mCinematic;
        }
        public new void setCinematic(Cinematic cimenatic) {
            mCinematic = cimenatic;
        }
        public new bool isCinematic() {
            return mCinematic != null;
        }

        public new Transform getTransform() {
            return mMainCamera.transform;
        }

        public new Vector3 getPosition() {
            return mMainCamera.transform.position;
        }

        public new Vector3 getDirection() {
            return mMainCamera.transform.forward;
        }

        public new void setAntiAliasingEnabled(bool enabled) {
            Antialiasing component = mMainCamera.GetComponent<Antialiasing>();
            if (component != null) {
                component.enabled = enabled;
            }
        }

        public new float getHeightRatio() {
            return Mathf.Clamp01((mMainCamera.transform.position.y - 12f) / 18f);
        }

        public new bool isTransitioning() {
            return mCameraTransition < 1f;
        }

        public new void setNearClipPlane(float nearClipPlane) {
            mMainCameraComponent.nearClipPlane = nearClipPlane;
        }

        public CameraManagerCustom() {
            mInstance = this;
            mMainCamera = createMainCamera();
            mSkydomeCamera = new GameObject();
            mSkydomeCamera.name = "Skydome Camera";
            Camera camera = mSkydomeCamera.AddComponent<Camera>();
            camera.nearClipPlane = 0.5f;
            camera.farClipPlane = 1000000f;
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.depth = -100f;
            camera.cullingMask = 262144;
            camera.renderingPath = RenderingPath.DeferredLighting;
            DefaultCameraTransform.apply(mMainCamera.transform);
            DefaultCameraTransform.apply(mSkydomeCamera.transform);
            initCamera();
        }

        private GameObject createMainCamera() {
            int qualityLevel = QualitySettings.GetQualityLevel();
            GameObject gameObject = ResourceList.getInstance().QualityCameras[qualityLevel];
            GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
            gameObject2.name = "Main Camera (" + gameObject.name.Replace("PrefabCamera", string.Empty) + ")";
            return gameObject2;
        }

        public new void onQualityChanged() {
            GameObject gameObject = createMainCamera();
            gameObject.transform.position = mMainCamera.transform.position;
            gameObject.transform.rotation = mMainCamera.transform.rotation;
            UnityEngine.Object.Destroy(mMainCamera);
            mMainCamera = gameObject;
            initCamera();
        }

        private void initCamera() {
            mMainCameraComponent = mMainCamera.GetComponent<Camera>();
            mMainCameraComponent.farClipPlane = 4000f;
            mMainCameraComponent.nearClipPlane = 0.5f;
            mMainCameraComponent.clearFlags = CameraClearFlags.Skybox;
            mMainCameraComponent.hdr = false;
            mMainCameraComponent.useOcclusionCulling = true;
            mMainCameraComponent.renderingPath = RenderingPath.DeferredLighting;
            float[] array = new float[32];
            float layerDistanceFactor = ExtraQualitySettings.getQualityLevelSettings().LayerDistanceFactor;
            array[10] = 150f * layerDistanceFactor;
            array[15] = 115f * layerDistanceFactor;
            array[13] = 200f * layerDistanceFactor;
            array[14] = 110f * layerDistanceFactor;
            array[17] = 250f * layerDistanceFactor;
            array[19] = 500f * layerDistanceFactor;
            array[12] = 210f * layerDistanceFactor;
            mMainCameraComponent.layerCullDistances = array;
            mMainCameraComponent.layerCullSpherical = true;
            mMainCameraComponent.cullingMask = -2359297;
            mMainCameraComponent.useOcclusionCulling = false;
            mMainCamera.AddComponent<AudioListener>();
        }

        public new void onGameStart() {
            mMainCameraComponent.farClipPlane = 20000f;
            mMainCameraComponent.clearFlags = CameraClearFlags.Depth;
            mMainCameraComponent.fieldOfView = 60f;
            mSkydomeCamera.SetActive(true);
        }

        public new void onNewGame() {
            mCurrentHeight = 21f;
            mTargetHeight = mCurrentHeight;
        }

        public new void onTitleScene() {
            mMainCameraComponent.clearFlags = CameraClearFlags.Color;
            mMainCameraComponent.farClipPlane = 50000f;
            setCameraTransform(DefaultCameraTransform);
            mSkydomeCamera.SetActive(false);
            mCinematic = null;
            mLocked = false;
            mZoomLocked = false;
        }

        public new void onLogoStart() {
            onTitleScene();
        }

        public new void update(float timeStep) {
            if (mZoomAxis == 0f) {
                mZoomAxis = Input.GetAxis("Zoom");
            }

            GameState gameState = GameManager.getInstance().getGameState();
            if (gameState != null && !gameState.isCameraFixed()) {
                if (mCinematic == null) {
                    GameStateGame gameStateGame = gameState as GameStateGame;

                    if (gameStateGame != null && Traverse.Create(gameStateGame).Field("mMode").GetValue() == GameStateGame_Mode_PlacingModule && mIsPlacingModule) {
                        Traverse.Create(gameStateGame).Field("mCurrentModuleSize").SetValue(mModulesize);
                    }

                    Transform transform = mMainCamera.transform;

                    float xAxis = mAcceleration.x;
                    float yAxis = mAcceleration.y;
                    float zAxis = mAcceleration.z;
                    float absXAxis = Mathf.Abs(xAxis);
                    float absYAxis = Mathf.Abs(yAxis);
                    float absZAxis = Mathf.Abs(zAxis);

                    if (!mLocked) {
                        // if zooming
                        if (absYAxis > 0.01f) {
                            float speed = Mathf.Clamp(60f * timeStep, 0.01f, 100f);
                            float newHeight = Mathf.Clamp(mCurrentHeight + yAxis * speed, MIN_HEIGHT, MAX_HEIGHT);

                            if (transform.eulerAngles.x < 86f) {
                                zAxis += (mCurrentHeight - newHeight) / speed;
                                absZAxis = Mathf.Abs(zAxis);
                            }

                            mCurrentHeight = newHeight;
                            mTargetHeight = mCurrentHeight;
                        }

                        // Move forwards
                        if (absZAxis > 0.001f) {
                            transform.position += new Vector3(transform.forward.x, 0f, transform.forward.z).normalized * zAxis * timeStep * 80f;
                        }

                        // Move sideways
                        if (absXAxis > 0.001f) {
                            transform.position += new Vector3(transform.right.x, 0f, transform.right.z).normalized * xAxis * timeStep * 80f;
                        }

                        // rotate around cam
                        Vector3 eulerAngles = transform.eulerAngles;
                        if (Mathf.Abs(mRotationAcceleration) > 0.01f) {
                            eulerAngles.y += mRotationAcceleration * timeStep * 120f;
                        }

                        if (Mathf.Abs(mVerticalRotationAcceleration) > 0.01f) {
                            eulerAngles.x = Mathf.Clamp(eulerAngles.x - mVerticalRotationAcceleration * timeStep * 120f, 20f, 87f);
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

                            transform.position += movement;
                        }
                    }

                    // rotate around world
                    if (Mathf.Abs(mAlternateRotationAcceleration) > 0.01f) {
                        Ray ray = new Ray(transform.position, transform.forward);
                        float dist;
                        if (mGroundPlane.Raycast(ray, out dist)) {
                            transform.RotateAround(transform.position + transform.forward * dist, Vector3.up, mAlternateRotationAcceleration * timeStep * 120f);
                        }
                    }

                    // if we moved, set the correct height
                    if (!mLocked && (absZAxis > 0.001f || absXAxis > 0.001f || absYAxis > 0.01f)) {
                        placeOnFloor(mCurrentHeight);
                    }

                    // Calc map center and distance
                    Vector3 mapCenter = new Vector3(TerrainGenerator_TotalSize, 0f, TerrainGenerator_TotalSize) * 0.5f;
                    Vector3 mapCenterToCam = transform.position - mapCenter;
                    float distToMapCenter = mapCenterToCam.magnitude;

                    // limit cam to 375 units from center
                    if (distToMapCenter > 375f) {
                        mMainCamera.transform.position = mapCenter + mapCenterToCam.normalized * 375f;
                    }
                }
                else {
                    updateCinematic(timeStep);
                }
            }

            // interpolate the position when the game moves the camera to a specific location (e.g when editing a building)
            if (mCameraTransition < 1f) {
                if (mTransitionTime == 0f) {
                    mCameraTransition = 1f;
                }
                else {
                    float num4 = timeStep / mTransitionTime;
                    mCameraTransition += num4;
                }
                mCurrentTransform.interpolate(mSourceTransform, mTargetTransform, mCameraTransition);
                mCurrentTransform.apply(mMainCamera.transform);
            }

            mSkydomeCamera.transform.rotation = mMainCamera.transform.rotation;
        }

        public new void fixedUpdate(float timeStep, int frameIndex) {
            if (this.mCinematic == null) {
                float lateralMoveSpeed = timeStep * 6f;
                float zoomAndRotationSpeed = timeStep * 10f;

                GameState gameState = GameManager.getInstance().getGameState();

                // this only happens when placing a module and only if current height < 21
                if (mTargetHeight != mCurrentHeight) {
                    mCurrentHeight += Mathf.Sign(mTargetHeight - mCurrentHeight) * timeStep * 30f;
                    if (Mathf.Abs(mCurrentHeight - mTargetHeight) < 0.5f) {
                        mCurrentHeight = mTargetHeight;
                    }
                }

                if (gameState != null && !gameState.isCameraFixed() && !TimeManager.getInstance().isPaused()) {
                    KeyBindingManager keyBindingManager = KeyBindingManager.getInstance();
                    GameStateGame gameStateGame = gameState as GameStateGame;
                    Traverse t_gameStateGame = Traverse.Create(gameStateGame);
                    if (gameStateGame != null && t_gameStateGame.Field("mMode").GetValue() == GameStateGame_Mode_PlacingModule) {

                        Traverse<int> t_mCurrentModuleSize = t_gameStateGame.Field<int>("mCurrentModuleSize");

                        if (!mIsPlacingModule) {
                            mIsPlacingModule = true;
                            mModulesize = t_mCurrentModuleSize.Value;
                        }

                        // we're zooming
                        if (Mathf.Abs(mZoomAxis) > 0.001f || Mathf.Abs(keyBindingManager.getCompositeAxis(ActionType.CameraZoomOut, ActionType.CameraZoomIn)) > 0.001f) {
                            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                                ModuleType mPlacedModuleType = t_gameStateGame.Field<ModuleType>("mPlacedModuleType").Value;
                                if (mZoomAxis <= -0.1f || keyBindingManager.getBinding(ActionType.CameraZoomOut).justUp()) {
                                    if (mModulesize > mPlacedModuleType.getMinSize()) {
                                        mModulesize--;
                                    }
                                }
                                else if ((mZoomAxis >= 0.1f || keyBindingManager.getBinding(ActionType.CameraZoomIn).justUp()) && mModulesize < mPlacedModuleType.getMaxSize()) {
                                    mModulesize++;
                                }
                            }
                        }

                        t_mCurrentModuleSize.Value = mModulesize;
                    }
                    else {
                        mIsPlacingModule = false;
                    }

                    mAcceleration.x += keyBindingManager.getCompositeAxis(ActionType.CameraMoveLeft, ActionType.CameraMoveRight) * lateralMoveSpeed;
                    mAcceleration.z += keyBindingManager.getCompositeAxis(ActionType.CameraMoveBack, ActionType.CameraMoveForward) * lateralMoveSpeed;

                    if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) {
                        mAcceleration.y -= mZoomAxis * zoomAndRotationSpeed;
                        mAcceleration.y -= keyBindingManager.getCompositeAxis(ActionType.CameraZoomOut, ActionType.CameraZoomIn) * zoomAndRotationSpeed;
                    }

                    mAlternateRotationAcceleration -= keyBindingManager.getCompositeAxis(ActionType.CameraRotateLeft, ActionType.CameraRotateRight) * zoomAndRotationSpeed;

                    // Rotate with middle mouse button
                    if (Input.GetMouseButton(2)) {
                        float mouseDeltaX = Input.mousePosition.x - mPreviousMouseX;
                        if (Mathf.Abs(mouseDeltaX) > Mathf.Epsilon)
                            mRotationAcceleration += zoomAndRotationSpeed * mouseDeltaX * 0.1f;

                        float mouseDeltaY = Input.mousePosition.y - mPreviousMouseY;
                        if (Mathf.Abs(mouseDeltaY) > Mathf.Epsilon)
                            mVerticalRotationAcceleration += zoomAndRotationSpeed * mouseDeltaY * 0.1f;
                    }

                    // Move with mouse on screen borders
                    if (!Application.isEditor) {
                        float screenBorder = Screen.height * 0.01f;
                        if (Input.mousePosition.x < screenBorder) {
                            mAcceleration.x = mAcceleration.x - lateralMoveSpeed;
                        }
                        else if (Input.mousePosition.x > Screen.width - screenBorder) {
                            mAcceleration.x = mAcceleration.x + lateralMoveSpeed;
                        }
                        if (Input.mousePosition.y < screenBorder) {
                            mAcceleration.z = mAcceleration.z - lateralMoveSpeed;
                        }
                        else if (Input.mousePosition.y > Screen.height - screenBorder) {
                            mAcceleration.z = mAcceleration.z + lateralMoveSpeed;
                        }
                    }

                    float clampSpeed = !Input.GetKey(KeyCode.LeftShift) ? 1f : 0.25f;
                    mAcceleration.x = Mathf.Clamp(mAcceleration.x - mAcceleration.x * lateralMoveSpeed, -clampSpeed, clampSpeed);
                    mAcceleration.z = Mathf.Clamp(mAcceleration.z - mAcceleration.z * lateralMoveSpeed, -clampSpeed, clampSpeed);
                    mAcceleration.y = Mathf.Clamp(mAcceleration.y - mAcceleration.y * zoomAndRotationSpeed, -clampSpeed, clampSpeed);
                    mRotationAcceleration = Mathf.Clamp(mRotationAcceleration - mRotationAcceleration * zoomAndRotationSpeed, -clampSpeed, clampSpeed);
                    mVerticalRotationAcceleration = Mathf.Clamp(mVerticalRotationAcceleration - mVerticalRotationAcceleration * zoomAndRotationSpeed, -clampSpeed, clampSpeed);
                    mAlternateRotationAcceleration = Mathf.Clamp(mAlternateRotationAcceleration - mAlternateRotationAcceleration * zoomAndRotationSpeed, -clampSpeed, clampSpeed);
                }
                else {
                    mAcceleration = Vector3.zero;
                    mRotationAcceleration = 0f;
                    mVerticalRotationAcceleration = 0f;
                    mAlternateRotationAcceleration = 0f;
                }

                mPreviousMouseX = Input.mousePosition.x;
                mPreviousMouseY = Input.mousePosition.y;
            }
            else {
                mCinematic.fixedUpdate(timeStep);
            }

            mZoomAxis = 0f;
        }

        public new void updateCinematic(float timeStep) {
            if (mCinematic != null) {
                mCinematic.update(timeStep);
            }
        }

        public new void lookAtColonyShip(Ship colonyShip, float time) {
            Vector3 position = colonyShip.getPosition();
            Quaternion orientation = Quaternion.Euler(25f, 180f + PlanetManager.getCurrentPlanet().getColonyShipRotation(), 0f);
            position += colonyShip.getDirection() * 45f;
            position = calculateFloorPosition(position, 21f);
            SimpleTransform targetTransform = new SimpleTransform(position, orientation);
            transition(targetTransform, time);
        }

        public new void placeOnFloor(float height) {
            mMainCamera.transform.position = calculateFloorPosition(getPosition(), height);
        }

        public new Vector3 calculateFloorPosition(Vector3 position, float height) {
            Vector3 terrainPosition;
            if (PhysicsUtil.findFloor(position, out terrainPosition)) {
                terrainPosition.y = Mathf.Max(terrainPosition.y - 7.5f, 0f);
                return terrainPosition + Vector3.up * height;
            }
            return position;
        }

        public new Vector3 calculateCameraPosition(Vector3 position, float height) {
            Vector3 terrainPosition;
            if (PhysicsUtil.findFloor(position, out terrainPosition)) {
                return terrainPosition + new Vector3(0f, height, 0f);
            }
            Debug.LogWarning("Could not find camera floor!!");
            return position;
        }

        public new void scrollToPosition(Vector3 targetPosition) {
            if (!mLocked) {
                Vector3 position = mMainCamera.transform.position;
                Vector3 forward = mMainCamera.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                position.x = targetPosition.x;
                position.z = targetPosition.z;
                position -= forward * 30f;
                position = calculateCameraPosition(position, mCurrentHeight);
                SimpleTransform targetTransform = new SimpleTransform(position, mMainCamera.transform.rotation);
                transition(targetTransform, 1f);
            }
        }

        public new void focusOnPosition(Vector3 position, float distance) {
            SimpleTransform simpleTransform = new SimpleTransform(mMainCamera.transform);
            simpleTransform.focusOn(position, distance);
            transition(simpleTransform, 1f);
            mLocked = true;
        }

        public new void unfocus() {
            mLocked = false;
            transition(mSourceTransform, 1f);
        }

        public new void lockZoom() {
            mAcceleration.y = 0f;
            if (mCurrentHeight < 21f) {
                mTargetHeight = 21f;
            }
            mZoomLocked = true;
        }

        public new void unlockZoom() {
            mZoomLocked = false;
        }

        public new void setCameraTransform(SimpleTransform targetTransform) {
            mTransitionTime = 0f;
            mCameraTransition = 1f;
            targetTransform.apply(mMainCamera.transform);
        }

        public new void transition(SimpleTransform targetTransform, float time) {
            mCameraTransition = 0f;
            mTransitionTime = time;
            mSourceTransform = new SimpleTransform(mMainCamera.transform);
            mTargetTransform = targetTransform;
        }

        public new Vector3 worldToScreenPoint(Vector3 point) {
            return mMainCameraComponent.WorldToScreenPoint(point);
        }

        public new void resetSettings() {
            mMainCameraComponent.nearClipPlane = 0.5f;
            mMainCameraComponent.fieldOfView = 60f;
        }

        public new void renderCubemap() {
            int num = 1024;
            Cubemap cubemap = new Cubemap(num, TextureFormat.RGB24, false);
            Texture2D texture2D = new Texture2D(num * 6, num, TextureFormat.RGB24, false);
            Color[] array = new Color[num * 6 * num];
            GameObject gameObject = new GameObject("CubemapCamera");
            Camera camera = gameObject.AddComponent<Camera>();
            gameObject.transform.position = mMainCamera.transform.position;
            gameObject.transform.rotation = Quaternion.identity;
            camera.cullingMask = -1;
            camera.farClipPlane = 2000000f;
            gameObject.GetComponent<Camera>().RenderToCubemap(cubemap);
            for (int i = 0; i < 6; i++) {
                Color[] pixels = cubemap.GetPixels((CubemapFace)i);
                for (int j = 0; j < num; j++) {
                    Array.Copy(pixels, num * (num - j - 1), array, num * i + num * 6 * j, num);
                }
            }
            texture2D.SetPixels(array);
            Directory.CreateDirectory(Util.getFilesFolder());
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(Util.getFilesFolder() + "/cubemap" + DateTime.Now.ToString("_h_mm_ss_ff") + ".png", bytes);
            UnityEngine.Object.Destroy(gameObject);
        }

        public new bool isSoundInRange(SoundDefinition definition, Vector3 soundPosition) {
            float num = definition.getMaxDistance() + 10f;
            return (getPosition() - soundPosition).sqrMagnitude < num * num;
        }

        public new void serialize(XmlNode rootNode, string name) {
            System.Xml.XmlNode parent = Serialization.createNode(rootNode, name);
            Serialization.serializeFloat(parent, "height", mCurrentHeight);
            Serialization.serializeVector3(parent, "position", mMainCamera.transform.position);
            Serialization.serializeQuaternion(parent, "orientation", mMainCamera.transform.localRotation);
        }

        public new void deserialize(XmlNode node) {
            mMainCamera.transform.position = Serialization.deserializeVector3(node["position"]);
            Vector3 eulerAngles = StartupOrientation.eulerAngles;
            Vector3 eulerAngles2 = Serialization.deserializeQuaternion(node["orientation"]).eulerAngles;
            eulerAngles.y = eulerAngles2.y;
            mMainCamera.transform.localRotation = Quaternion.Euler(eulerAngles);
            mCurrentHeight = Serialization.deserializeFloat(node["height"]);
            mTargetHeight = mCurrentHeight;
        }

    }

}
