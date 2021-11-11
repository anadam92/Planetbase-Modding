using Planetbase;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace NoIntruders {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            List<Character> specializationCharacters = Character.getSpecializationCharacters(SpecializationList.IntruderInstance);
            if (specializationCharacters != null && specializationCharacters.Count > 0) {
                foreach (Character item in specializationCharacters) {
                    if (item != null && !item.isDead()) {
                        item.setArmed(false);
                        Traverse.Create(item).Method("setDead").GetValue(new object[] { });
                    }
                }
            }
        }

    }
}
