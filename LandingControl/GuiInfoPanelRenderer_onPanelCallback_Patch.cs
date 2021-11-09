using HarmonyLib;
using Planetbase;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace LandingControl {

    [HarmonyPatch(typeof(GuiInfoPanelRenderer), "onPanelCallback", MethodType.Normal)]
    static class Character_isCloseCameraAvailable_Patch {

        private static ConstructorInfo ci_GuiLandingPermissionsCustom_ctor = typeof(GuiLandingPermissionsCustom).GetConstructor(new Type[] { });

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(this IEnumerable<CodeInstruction> instructions) {
            foreach (CodeInstruction instruction in instructions) {
                if (instruction.opcode.Equals(OpCodes.Newobj)) {
                    instruction.operand = ci_GuiLandingPermissionsCustom_ctor;
                }
                yield return instruction;
            }
        }

    }
}
