using HarmonyLib;
using Planetbase;

namespace FreeBuilding {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            Traverse.Create(TypeList<ModuleType, ModuleTypeList>.find<ModuleTypeMine>()).Field<int>("mFlags").Value |= ModuleType.FlagAutoRotate;
        }

    }

}
