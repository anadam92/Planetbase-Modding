using Planetbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;


namespace MoreSpeed {

    public static class Main {

        public static bool active;
        private static Harmony harmony;

        /// <summary>
        /// Send a response to the mod manager about the launch status, success or not.
        /// </summary>
        /// <param name="modEntry"></param>
        /// <returns></returns>
        public static bool Load(UnityModManager.ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            harmony = new Harmony(modEntry.Info.Id);
            return true; // If false the mod will show an error.
        }

        static bool doPatching(bool patch = true) {
            bool result = true;
//            try {
                if (patch) {
                    harmony.PatchAll(Assembly.GetExecutingAssembly());
                }
                else {
                    harmony.UnpatchAll();
                }
//            }
//            catch (Exception) {
//                result = false;
//            }
            return result;
        }

        /// <summary>
        /// Called when the mod is turned to on/off.
        /// With this function you control an operation of the mod and inform users whether it is enabled or not.
        /// </summary>
        /// <param name="modEntry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value /* active or inactive */) {
            bool result = true;
            if (active != value) {
                result = doPatching(value);
                if (result) {
                    active = value;
                }
            }
            return result; // If true, the mod will switch the state. If not, the state will not change.
        }

    }
}
