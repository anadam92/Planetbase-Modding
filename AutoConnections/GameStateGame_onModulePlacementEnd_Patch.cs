using HarmonyLib;
using Planetbase;
using System.Collections.Generic;

namespace AutoConnections {

    [HarmonyPatch(typeof(GameStateGame), "onModulePlacementEnd", MethodType.Normal)]
    public abstract class GameStateGame_onModulePlacementEnd_Patch {

        private static readonly Traverse<Module> t_mActiveModule;
        private static readonly Traverse<List<Module>> t_mModules;
        private static readonly FastInvokeHandler mi_Module_recycleLinkComponents;

        static GameStateGame_onModulePlacementEnd_Patch() {
            t_mActiveModule = Traverse.Create<Module>().Field<Module>("mActiveModule");
            t_mModules = Traverse.Create<Module>().Field<List<Module>>("mModules");
            mi_Module_recycleLinkComponents = MethodInvoker.GetHandler(AccessTools.Method(typeof(Module), "recycleLinkComponents"));
        }

        [HarmonyLib.HarmonyPrefix]
        public static void Prefix() {
            Module mActiveModule = t_mActiveModule.Value;
            List<Module> mModules = t_mModules.Value;
            if (mActiveModule == null || mModules == null) {
                return;
            }

            // Add available connections
            List<Module> linkableModules = new List<Module>();
            foreach (Module module in mModules) {
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
