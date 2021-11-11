using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace BetterHours {

    public class BetterHours {

        public static double getDayHours() {
            double result = 24.0;
            PlanetManager instance = Singleton<PlanetManager>.getInstance();
            Traverse t_instance_mCurrentPlanet = Traverse.Create(instance).Field("mCurrentPlanet");
            PlanetDefinition instance_mCurrentPlanet_mDefinition = t_instance_mCurrentPlanet.Field<PlanetDefinition>("mDefinition").Value;
            if (instance != null && t_instance_mCurrentPlanet.GetValue<Planet>() != null && instance_mCurrentPlanet_mDefinition != null) {
                result = instance_mCurrentPlanet_mDefinition.DayHours + instance_mCurrentPlanet_mDefinition.NightHours;
            }
            return result;
        }

    }

}
