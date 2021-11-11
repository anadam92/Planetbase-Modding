using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Planetbase;
using UnityEngine;

namespace FastAirlock {

    [HarmonyPatch(typeof(InteractionAirlock), "update", MethodType.Normal)]
    class InteractionAirlock_update_Patch {

        private static object InteractionAirlock_Stage_Wait = Traverse.Create<InteractionAirlock>().Type("Stage").Field("Wait").GetValue();
        private static object InteractionAirlock_Stage_Exit = Traverse.Create<InteractionAirlock>().Type("Stage").Field("Exit").GetValue();
        private static object InteractionAirlock_Stage_GoEntry = Traverse.Create<InteractionAirlock>().Type("Stage").Field("GoEntry").GetValue();

        [HarmonyPostfix]
        public static void Postfix(InteractionAirlock __instance, ref bool __result, float timeStep) {
            Traverse t___instance = Traverse.Create(__instance);
            Selectable __instance_mSelectable = t___instance.Field<Selectable>("mSelectable").Value;
            Traverse t_mStage = t___instance.Field("mStage");
            Traverse<float> t_mStageProgress = t___instance.Field<float>("mStageProgress");
            Traverse<Vector3> t___instance_mTarget = t___instance.Field<Vector3>("mTarget");


            float num = timeStep * FastAirlockPatch.speedmult;
            Construction construction = __instance_mSelectable as Construction;
            if (construction != null && !construction.isPowered() && t_mStage.GetValue() == InteractionAirlock_Stage_Wait) {
                __result = true;
                return;
            }
            if (__instance_mSelectable.getFirstInteraction() == __instance) {
                t_mStageProgress.Value += num;
                if (t_mStageProgress.Value > 1f || t_mStage.GetValue() == InteractionAirlock_Stage_Wait) {
                    t___instance.Method("onStageDone").GetValue();
                    t_mStageProgress.Value = 0f;
                    if (t_mStage.GetValue() == InteractionAirlock_Stage_Exit) {
                        __result = true;
                        return;
                    }
                }
            }
            else {
                t_mStage.SetValue(InteractionAirlock_Stage_Wait);
                t___instance_mTarget.Value = t___instance.Method("getQueuePosition").GetValue<Vector3>(new object[] { __instance_mSelectable.getInteractionIndex(__instance) });
            }

            Character __instance_mCharacter = t___instance.Field<Character>("mCharacter").Value;
            Traverse<CharacterAnimationType> t___instance_mAnimationType = t___instance.Field<CharacterAnimationType>("mAnimationType");


            Vector3 direction = t___instance_mTarget.Value - __instance_mCharacter.getPosition();
            float magnitude = direction.magnitude;
            float num2 = Mathf.Min(4f * num, magnitude);
            if (magnitude > 0.25f) {
                Vector3 target = ((t_mStage.GetValue() != InteractionAirlock_Stage_Wait || !(magnitude < 1f)) ? direction.flatDirection() : (__instance_mSelectable.getPosition() - __instance_mCharacter.getPosition()).flatDirection());
                Vector3 direction2 = __instance_mCharacter.getDirection();
                __instance_mCharacter.setPosition(__instance_mCharacter.getPosition() + direction.normalized * num2);
                __instance_mCharacter.setDirection(Vector3.RotateTowards(direction2, target, (float)Math.PI * 2f * num, 0.1f));
                if (t___instance_mAnimationType.Value != CharacterAnimationType.Walk) {
                    t___instance_mAnimationType.Value = CharacterAnimationType.Walk;
                    __instance_mCharacter.playWalkAnimation();
                }
            }
            else {
                if (t_mStage.GetValue() == InteractionAirlock_Stage_GoEntry) {
                    t_mStageProgress.Value = 1f;
                }
                if (t___instance_mAnimationType.Value != 0) {
                    t___instance_mAnimationType.Value = CharacterAnimationType.Idle;
                    __instance_mCharacter.playIdleAnimation();
                }
            }
            __result = false;
            return;
        }

    }

}
