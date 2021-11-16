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

    public class MoreSpeed {

        private static float[] TimeScales_beforePatching;

        public static void doPatching() {
            Traverse<float[]>  t_TimeManager_TimeScales = Traverse.Create(Singleton<TimeManager>.getInstance()).Field<float[]>("TimeScales");
            TimeScales_beforePatching = t_TimeManager_TimeScales.Value;
            HashSet<float> set = new HashSet<float>(TimeScales_beforePatching);
            set.Add(6f);
            set.Add(8f);
            set.Add(10f);
            t_TimeManager_TimeScales.Value = set.OrderBy((o) => o).ToArray();
        }

        public static void doUnPatching() {
            Singleton<TimeManager>.getInstance().setNormalSpeed();
            Traverse.Create(Singleton<TimeManager>.getInstance()).Field<float[]>("TimeScales").Value = TimeScales_beforePatching;
            TimeScales_beforePatching = null;
        }

    }

}
