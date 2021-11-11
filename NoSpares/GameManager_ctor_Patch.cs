using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace NoSpares {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            ModuleTypeSolarPanel moduleTypeSolarPanel = (ModuleTypeSolarPanel)TypeList<ModuleType, ModuleTypeList>.find("ModuleTypeSolarPanel");
            if (moduleTypeSolarPanel != null) {
                Traverse.Create(moduleTypeSolarPanel).Field<float>("mCondicionDecayTime" ).Value= 0f;
            }
            ModuleTypeWindTurbine moduleTypeWindTurbine = (ModuleTypeWindTurbine)TypeList<ModuleType, ModuleTypeList>.find("ModuleTypeWindTurbine");
            if (moduleTypeWindTurbine != null) {
                Traverse.Create(moduleTypeWindTurbine).Field<float>("mCondicionDecayTime").Value = 0f;
            }
        }

    }

}
