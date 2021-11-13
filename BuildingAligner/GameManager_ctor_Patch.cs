using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace BuildingAligner {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        private static Type type_DebugRenderer = Assembly.GetAssembly(typeof(GameManager)).GetType("Planetbase.DebugRenderer");
        private static Traverse t_DebugRenderer = Traverse.Create(type_DebugRenderer);

        [HarmonyPostfix]
        public static void Postfix() {
            MethodInvoker.GetHandler(AccessTools.DeclaredMethod(type_DebugRenderer, "addGroup")).Invoke(null, new object[] { "Connections" });
        }

    }

}
