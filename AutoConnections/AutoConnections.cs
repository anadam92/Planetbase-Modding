using Planetbase;
using Redirection;
using System.Collections.Generic;
using UnityEngine;

namespace AutoConnections
{
    public class AutoConnections : IMod
    {
        public void Init()
        {
            Redirector.PerformRedirections();
            Debug.Log("[MOD] AutoConnections activated");
        }

        public void Update()
        {

        }
    }

    public abstract class CustomGameStateGame : GameStateGame
    {
        private CustomGameStateGame(string s, int i) : base(s, i) { }

        [RedirectFrom(typeof(GameStateGame))]
        public new void placeModule()
        {
            if (this.mActiveModule != null)
            {
                if (this.mActiveModule.isValidPosition())
                {
                    this.mActiveModule.setPositionY(TerrainGenerator.getInstance().getFloorHeight());
                    this.mActiveModule.onUserPlaced();
                    this.mActiveModule.playSound(SoundList.getInstance().ConstructionPlace);
                    this.mCost = null;

                    // Add available connections
                    List<Module> linkableModules = new List<Module>();
                    for (int i = 0; i < Module.mModules.Count; i++)
                    {
                        Module module = Module.mModules[i];
                        if (module != null && module != this.mActiveModule && Connection.canLink(mActiveModule, module, mActiveModule.getPosition(), module.getPosition()))
                        {
                            linkableModules.Add(module);
                        }

                        // if we already have 2 possible links, we already we know we can't connect so stop now
                        if (linkableModules.Count == 2)
                            break;
                    }

                    if (linkableModules.Count == 1)
                    {
                        Module.linkModules(mActiveModule, linkableModules[0], mRenderTops);
                    }

                    this.onModulePlacementEnd();
                }
                else
                {
                    AudioPlayer.getInstance().play(SoundList.getInstance().ButtonClickWrong, null);
                    this.mActiveModule.destroy();
                    this.onModulePlacementEnd();
                    this.addToast(StringList.get("hint_placement_invalid"), 3f);
                }
            }
        }
    }
}
