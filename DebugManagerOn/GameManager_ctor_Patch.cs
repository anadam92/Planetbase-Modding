using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace DebugManagerOn {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            GameObject go = new GameObject();
            go.AddComponent<DebugManagerOn_MonoBehaviour>();
            GameObject.DontDestroyOnLoad(go);
        }

    }

}
