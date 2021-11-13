using Planetbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace FreeFurnishing {
    
    [HarmonyPatch(typeof(Planetbase.Module), "canPlaceComponent", MethodType.Normal)]
    static class Module_canPlaceComponent_Patch {

        private static MethodInfo mi_clampComponentPosition = typeof(Planetbase.Module).GetMethod("clampComponentPosition");
        private static MethodInfo mi_intersectsAnyComponents = typeof(Planetbase.Module).GetMethod("intersectsAnyComponents");

        [HarmonyPrefix]
        static bool Prefix(ref bool __result , Planetbase.Module __instance, ConstructionComponent component) {
            if (Input.GetKeyUp(KeyCode.X)) {
                // rotate
                component.getTransform().Rotate(Vector3.up * 15f);
            }

            // step
            Vector3 fromCenter = component.getPosition() - __instance.getPosition();
            fromCenter.x = Mathf.Round(fromCenter.x * 2f) * 0.5f;
            fromCenter.z = Mathf.Round(fromCenter.z * 2f) * 0.5f;
            component.setPosition(__instance.getPosition() + fromCenter);

            mi_clampComponentPosition.Invoke(__instance , new object[]{component});
            __result = !(bool)mi_intersectsAnyComponents.Invoke(__instance, new object[] { component }) /*&& this.isValidLayoutPosition(component)*/;

            return false;
        }
        
    }

}
