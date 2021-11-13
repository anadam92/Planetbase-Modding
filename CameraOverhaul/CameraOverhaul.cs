using Planetbase;
using HarmonyLib;
using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace CameraOverhaul {

    public class CameraOverhaul {

        public static object GameStateGame_Mode_PlacingModule;
        public static float TerrainGenerator_TotalSize;
        public static Plane mGroundPlane;

        public static float MIN_HEIGHT = 12f;
        public static float MAX_HEIGHT = 120f;

        static CameraOverhaul() {
            GameStateGame_Mode_PlacingModule = Traverse.Create<GameStateGame>().Type("Mode").Field("PlacingModule").GetValue();
            TerrainGenerator_TotalSize = Traverse.Create<TerrainGenerator>().Field<float>("TotalSize").Value;
            mGroundPlane = new Plane(Vector3.up, new Vector3(TerrainGenerator_TotalSize, 0f, TerrainGenerator_TotalSize) * 0.5f);
        }

    }

}
