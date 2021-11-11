using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace MoreSpeed {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            Traverse<float[]> t_TimeScales = Traverse.Create(Singleton<TimeManager>.getInstance()).Field<float[]>("TimeScales");
            List<float> list = t_TimeScales.Value.ToList<float>();
            list.Add(6f);
            list.Add(8f);
            list.Add(10f);
            t_TimeScales.Value = list.ToArray();
        }

    }

}
