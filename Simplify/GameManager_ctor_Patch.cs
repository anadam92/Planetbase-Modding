using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using System.Xml;

namespace Simplifiy {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            ApplyPatches.PatchSpecialisations();
            ApplyPatches.PatchComponentTypesStart();
            ApplyPatches.PatchModuleTypes();
        }

    }

}
