using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace EternalBot {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            Traverse.Create(TypeList<ModuleType, ModuleTypeList>.find<ModuleTypeMine>()).Field<int>("mFlags").Value |= 32768;
        }

    }

}
