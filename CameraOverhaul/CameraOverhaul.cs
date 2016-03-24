using Planetbase;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CameraOverhaul
{
    public class CameraOverhaul : IMod
    {
        public void Init()
        {
            Redirector.PerformRedirections();
            Debug.Log("[MOD] CameraOverhaul activated");
        }

        public void Update()
        {
            
        }
    }

    public abstract class CustomCameraManager : CameraManager
    {
        [RedirectFrom(typeof(CameraManager))]
        public new void update(float timeStep)
        {
            if (this.mZoomAxis == 0f)
            {
                this.mZoomAxis = Input.GetAxis("Zoom");
            }
            GameState gameState = GameManager.getInstance().getGameState();
            if (gameState != null && !gameState.isCameraFixed() && !this.mLocked)
            {
                if (this.mCinematic == null)
                {
                    float x = this.mAcceleration.x;
                    float num = this.mAcceleration.z;
                    if (Mathf.Abs(this.mAcceleration.y) > 0.01f)
                    {
                        float num2 = Mathf.Clamp(60f * timeStep, 0.01f, 100f);
                        float num3 = Mathf.Clamp(this.mCurrentHeight + this.mAcceleration.y * num2, 12f, 100f);
                        num += (this.mCurrentHeight - num3) / num2;
                        this.mCurrentHeight = num3;
                        this.mTargetHeight = this.mCurrentHeight;
                    }
                    Transform transform = this.mMainCamera.transform;
                    if (Mathf.Abs(num) > 0.01f && Mathf.Abs(num) > 0.001f)
                    {
                        transform.position += new Vector3(transform.forward.x, 0f, transform.forward.z) * num * timeStep * 80f;
                    }
                    if (Mathf.Abs(x) > 0.01f && Mathf.Abs(x) > 0.001f)
                    {
                        transform.position += new Vector3(transform.right.x, 0f, transform.right.z) * x * timeStep * 80f;
                    }
                    if (Mathf.Abs(this.mRotationAcceleration) > 0.01f)
                    {
                        transform.RotateAround(this.mMainCamera.transform.position, new Vector3(0f, 1f, 0f), this.mRotationAcceleration * timeStep * 120f);
                    }
                    Vector3 vector = new Vector3(2000f, 0f, 2000f) * 0.5f;
                    Vector3 vector2 = this.mMainCamera.transform.position - vector;
                    float magnitude = vector2.magnitude;
                    if (magnitude > 375f)
                    {
                        this.mMainCamera.transform.position = vector + vector2.normalized * 750f * 0.5f;
                    }
                    if (Mathf.Abs(num) > 0.001f || Mathf.Abs(x) > 0.001f || this.mZoomLocked)
                    {
                        this.placeOnFloor(this.mCurrentHeight);
                        Vector3 eulerAngles = this.mMainCamera.transform.rotation.eulerAngles;
                        eulerAngles.x = 25f;
                        this.mMainCamera.transform.rotation = Quaternion.Euler(eulerAngles);
                    }
                }
                else
                {
                    this.updateCinematic(timeStep);
                }
            }
            if (this.mCameraTransition < 1f)
            {
                if (this.mTransitionTime == 0f)
                {
                    this.mCameraTransition = 1f;
                }
                else
                {
                    float num4 = timeStep / this.mTransitionTime;
                    this.mCameraTransition += num4;
                }
                this.mCurrentTransform.interpolate(this.mSourceTransform, this.mTargetTransform, this.mCameraTransition);
                this.mCurrentTransform.apply(this.mMainCamera.transform);
            }
            this.mSkydomeCamera.transform.rotation = this.mMainCamera.transform.rotation;
        }
    }
}
