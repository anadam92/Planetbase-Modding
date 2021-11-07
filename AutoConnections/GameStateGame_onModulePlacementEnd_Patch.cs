using HarmonyLib;
using Planetbase;
using System.Collections.Generic;

namespace AutoConnections {

    [HarmonyPatch(typeof(GameStateGame), "onModulePlacementEnd", MethodType.Normal)]
    public abstract class GameStateGame_onModulePlacementEnd_Patch {

        private static Traverse<Module> t_mActiveModule = Traverse.Create<Module>().Field<Module>("mActiveModule");
        private static Traverse<List<Module>> t_mModules = Traverse.Create<Module>().Field<List<Module>>("mModules");
        private static FastInvokeHandler mi_Module_recycleLinkComponents = MethodInvoker.GetHandler(typeof(Module).GetMethod("recycleLinkComponents"));
        
        [HarmonyLib.HarmonyPrefix]
        public static void Prefix() {
            Module mActiveModule = t_mActiveModule.Value;
            List<Module> mModules = t_mModules.Value;

            // Add available connections
            List<Module> linkableModules = new List<Module>();
            for (int i = 0; i < mModules.Count; i++) {
                Module module = mModules[i];
                if (module != null && module != mActiveModule && Connection.canLink(mActiveModule, module, mActiveModule.getPosition(), module.getPosition())) {
                    linkableModules.Add(module);
                }
            }

            if (!mActiveModule.hasFlag(ModuleType.FlagDeadEnd) || linkableModules.Count == 1) {
                foreach (Module module in linkableModules) {
                    Connection connection = Connection.create(mActiveModule, module);
                    connection.onUserPlaced();
                    connection.setRenderTop(Traverse.Create<GameStateGame>().Field<bool>("mRenderTops").Value);
                    mi_Module_recycleLinkComponents.Invoke(mActiveModule, new object[] { });
                    mi_Module_recycleLinkComponents.Invoke(module, new object[] { });
                }
            }
        }

    }

}
