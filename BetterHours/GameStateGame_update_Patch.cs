using Planetbase;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace BetterHours {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            StatsCollector instance = Singleton<StatsCollector>.getInstance();
            EnvironmentManager instance2 = Singleton<EnvironmentManager>.getInstance();
            if (instance != null && instance2 != null) {
                double dayHours = BetterHours.getDayHours();
                Traverse.Create(instance).Field<float>("mRefreshPeriod").Value = (float)(((double)instance2.getDayTime() + (double)instance2.getNightTime()) / (dayHours / 6.0));
            }
        }

    }

}
