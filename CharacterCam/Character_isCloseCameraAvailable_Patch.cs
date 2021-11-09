using HarmonyLib;
using Planetbase;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace CharacterCam {

    [HarmonyPatch(typeof(Character), "isCloseCameraAvailable", MethodType.Normal)]
    static class Character_isCloseCameraAvailable_Patch {

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(this IEnumerable<CodeInstruction> instructions) {
            return new CodeInstruction[]{
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Ret)
            };
        }

    }
}
