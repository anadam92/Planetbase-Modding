using HarmonyLib;
using Planetbase;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace CameraOverhaul {
    [HarmonyPatch(typeof(CameraManager), "getInstance", MethodType.Normal)]
    static class CameraManager_getInstance_Patch {

        private static ConstructorInfo ci_CameraManagerCustom_ctor = typeof(CameraManagerCustom).GetConstructor(new Type[] { });

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(this IEnumerable<CodeInstruction> instructions) {
            foreach (CodeInstruction instruction in instructions) {
                if (instruction.opcode.Equals(OpCodes.Newobj)) {
                    instruction.operand = ci_CameraManagerCustom_ctor;
                }
                yield return instruction;
            }
        }

    }
}
