using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace CameraOverhaul {

    internal class CameraManagerProxy {

        private static CameraManagerProxy _currentInstance;

        internal static CameraManagerProxy get(CameraManager cameraManager) {
            if ((CameraManagerProxy._currentInstance == null) || !object.ReferenceEquals(CameraManagerProxy._currentInstance.cameraManager, cameraManager)) {
                _currentInstance = new CameraManagerProxy(cameraManager);
            }
            return _currentInstance;
        }

        private CameraManager cameraManager;
        private Traverse t_cameraManager;

        public bool mIsPlacingModule = false;
        public int mModulesize = 0;
        public float mPreviousMouseX = 0f;
        public float mPreviousMouseY = 0f;
        public float mVerticalRotationAcceleration = 0f;
        public float mAlternateRotationAcceleration = 0f;
        public float mRotationAcceleration = 0f;
        public float mZoomAxis;

        public Traverse<SimpleTransform> mTargetTransform;
        public Traverse<SimpleTransform> mSourceTransform;
        public Traverse<SimpleTransform> mCurrentTransform;

        public Traverse<GameObject> mMainCamera;
        public Traverse<GameObject> mSkydomeCamera;
        public Traverse<Cinematic> mCinematic;
        public Traverse<Vector3> mAcceleration;
        public Traverse<float> mTargetHeight;
        public Traverse<float> mCurrentHeight;
        public Traverse<bool> mLocked;
        public Traverse<float> mCameraTransition;
        public Traverse<float> mTransitionTime;

        public FastInvokeHandler placeOnFloor;
        public FastInvokeHandler updateCinematic;


        private CameraManagerProxy(CameraManager cameraManager) {
            this.cameraManager = cameraManager;
            this.t_cameraManager = Traverse.Create(cameraManager);

            this.mMainCamera = t_cameraManager.Field<GameObject>("mMainCamera");
            this.mSkydomeCamera = t_cameraManager.Field<GameObject>("mSkydomeCamera");

            this.mCinematic = t_cameraManager.Field<Cinematic>("mCinematic");
            this.mAcceleration = t_cameraManager.Field<Vector3>("mAcceleration");
            this.mAcceleration.Value = Vector3.zero;

            this.mTargetTransform = t_cameraManager.Field<SimpleTransform>("mTargetTransform");
            this.mSourceTransform = t_cameraManager.Field<SimpleTransform>("mSourceTransform");
            this.mCurrentTransform = t_cameraManager.Field<SimpleTransform>("mCurrentTransform");
            this.mCurrentTransform.Value = new SimpleTransform();

            this.mTargetHeight = t_cameraManager.Field<float>("mTargetHeight");
            this.mCurrentHeight = t_cameraManager.Field<float>("mCurrentHeight");
            this.mTargetHeight.Value = 21f;
            this.mCurrentHeight.Value = 21f;
            this.mLocked = t_cameraManager.Field<bool>("mLocked");
            this.mCameraTransition = t_cameraManager.Field<float>("mCameraTransition");
            this.mCameraTransition.Value = 1f;
            this.mTransitionTime = t_cameraManager.Field<float>("mTransitionTime");


            this.placeOnFloor = MethodInvoker.GetHandler(AccessTools.Method(typeof(CameraManager), "placeOnFloor"));
            this.updateCinematic = MethodInvoker.GetHandler(AccessTools.Method(typeof(CameraManager), "updateCinematic"));

        }

    }

}
