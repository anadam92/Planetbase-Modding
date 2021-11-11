using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planetbase;
using UnityEngine;

namespace StorageGuru {

    internal static class ContentManager {

        private static string STORAGE_ENABLE_ICON_PATH = "Mods\\Textures\\StorageEnable.png";
        private static string STORAGE_DISABLE_ICON_PATH = "Mods\\Textures\\StorageDisable.png";
        private static float greyScaleBrightness = 0.7058824f;

        public static Texture2D StorageEnableIcon { get; private set; }
        public static Texture2D StorageDisableIcon { get; private set; }

        internal static Dictionary<ResourceType, IconPair> LoadContent(ResourceType[] resourceTypes) {
            STORAGE_ENABLE_ICON_PATH = Path.Combine(Util.getFilesFolder(), STORAGE_ENABLE_ICON_PATH);
            STORAGE_DISABLE_ICON_PATH = Path.Combine(Util.getFilesFolder(), STORAGE_DISABLE_ICON_PATH);
            StorageEnableIcon = LoadTexture(STORAGE_ENABLE_ICON_PATH);
            StorageDisableIcon = LoadTexture(STORAGE_DISABLE_ICON_PATH);
            List<IconPair> icons = resourceTypes.Select((ResourceType x) => new IconPair(x.getIcon(), ApplyColorFix(x.getIcon()))).ToList();
            return resourceTypes.Select((ResourceType k, int i) => new {
                k = k,
                v = icons[i]
            }).ToDictionary(x => x.k, x => x.v);
        }

        private static Texture2D LoadTexture(string filepath) {
            if (File.Exists(filepath)) {
                byte[] data = File.ReadAllBytes(filepath);
                Texture2D texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(data);
                return Util.applyColor(texture2D);
            }
            return new Texture2D(1, 1);
        }

        public static Texture2D ApplyColorFix(Texture2D texture) {
            Color[] pixels = (from p in texture.GetPixels()
                              select new Color(greyScaleBrightness, greyScaleBrightness, greyScaleBrightness, p.a)).ToArray();
            texture.SetPixels(pixels);
            return Util.applyColor(texture);
        }

    }

}
