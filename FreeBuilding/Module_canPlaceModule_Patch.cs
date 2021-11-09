using HarmonyLib;
using Planetbase;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace FreeBuilding {

    [HarmonyPatch(typeof(Planetbase.Module), "canPlaceModule", MethodType.Normal)]
    static class Character_isCloseCameraAvailable_Patch {

        private static FieldInfo fi_Vector3_y = typeof(Vector3).GetField("y");

        /// <summary>
        /// {position.y = floorHeight;} and {if (construction is Connection) return false;}
        /// </summary>
        private static IEnumerable<CodeInstruction[]> sequences = new CodeInstruction[][]{
            new CodeInstruction[]{
                new CodeInstruction(OpCodes.Ldarga_S, (byte)1),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Stfld, fi_Vector3_y)
            },
            new CodeInstruction[]{
                new CodeInstruction(OpCodes.Ldloc_S, (byte)16),
                new CodeInstruction(OpCodes.Isinst, typeof(Planetbase.Connection)),
                new CodeInstruction(OpCodes.Brfalse),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ret)
            },
        };

        /// <summary>
        /// a CodeInstruction sequence from which to remove till before {return true;}
        /// </summary>
        private static IEnumerable<CodeInstruction> sequenceLast = new CodeInstruction[]{
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Ldarg_3),
            new CodeInstruction(OpCodes.Ldc_R4, 0.5f),
            new CodeInstruction(OpCodes.Mul),
            new CodeInstruction(OpCodes.Ldc_R4, 2f),
            new CodeInstruction(OpCodes.Add),
            new CodeInstruction(OpCodes.Ldc_I4, 1024),
        };

        /// <summary>
        /// change min allowed distance from 9f or 10f to 3f
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler1(this IEnumerable<CodeInstruction> instructions) {
            foreach (CodeInstruction instruction in instructions) {
                if (instruction.opcode.Equals(OpCodes.Ldc_R4)) {
                    if (Equals(instruction.operand, 9f) || Equals(instruction.operand, 10f)) {
                        instruction.operand = 3f;
                    }
                }
                yield return instruction;
            }

        }

        /// <summary>
        /// remove {position.y = floorHeight;} and {if (construction is Connection) return false;}
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler2(this IEnumerable<CodeInstruction> instructions) {
            Queue<CodeInstruction> queue = new Queue<CodeInstruction>();
            IEnumerator<CodeInstruction> enOuter = instructions.GetEnumerator();
            IEnumerator<IEnumerable<CodeInstruction>> en_sequences = sequences.Select(cis => cis.AsEnumerable()).GetEnumerator();
            IEnumerator<CodeInstruction> enInner;

            enInner = en_sequences.MoveNext() ? en_sequences.Current.GetEnumerator() : null;

            while (enOuter.MoveNext()) {
                bool nowMatch = false;
                if (enInner != null && enInner.MoveNext() && (nowMatch = enInner.Current.Equals(enOuter.Current))) {
                    queue.Enqueue(enOuter.Current);
                    while (enInner.MoveNext() && enOuter.MoveNext() && (nowMatch = enInner.Current.Equals(enOuter.Current))) {
                        queue.Enqueue(enOuter.Current);
                    }
                    if (nowMatch) {
                        queue.Clear();
                        enInner = en_sequences.MoveNext() ? en_sequences.Current.GetEnumerator() : null;
                    }
                    else {
                        enInner.Reset();
                        while (queue.Count > 0) {
                            yield return queue.Dequeue();
                        }
                        yield return enOuter.Current;
                    }
                }
                else {
                    if (enInner != null) enInner.Reset();
                    yield return enOuter.Current;
                }
            }
        }

        /// <summary>
        /// remove last 2 if blocks
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler3(this IEnumerable<CodeInstruction> instructions) {
            Queue<CodeInstruction> queue = new Queue<CodeInstruction>();
            IEnumerator<CodeInstruction> enOuter = instructions.GetEnumerator();
            IEnumerator<CodeInstruction> enInner = sequenceLast.GetEnumerator();
            bool finished = false;
            while (!finished && enOuter.MoveNext()) {
                bool nowMatch = false;
                if (enInner.MoveNext() && (nowMatch = enInner.Current.Equals(enOuter.Current))) {
                    queue.Enqueue(enOuter.Current);
                    while (enInner.MoveNext() && enOuter.MoveNext() && (nowMatch = enInner.Current.Equals(enOuter.Current))) {
                        queue.Enqueue(enOuter.Current);
                    }
                    if (nowMatch) {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                        yield return new CodeInstruction(OpCodes.Ret);
                        finished = true;
                    }
                    else {
                        enInner.Reset();
                        while (queue.Count > 0) {
                            yield return queue.Dequeue();
                        }
                        yield return enOuter.Current;
                    }
                }
                else {
                    enInner.Reset();
                    yield return enOuter.Current;
                }
            }
        }

    }
}
