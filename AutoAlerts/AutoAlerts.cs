using Planetbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityModManagerNet;


namespace AutoAlerts
{

    public class AutoAlerts : Payload.IMod
    {

        public static bool enabled;
        private static Harmony harmony;

        private bool m_autoActivated;
        private AlertState m_activatedState;


        // Send a response to the mod manager about the launch status, success or not.
        public static bool Load(UnityModManager.ModEntry modEntry) {
            // modEntry.Info - Contains all fields from the 'Info.json' file.
            // modEntry.Path - The path to the mod folder e.g. '\Steam\steamapps\common\YourGame\Mods\TestMod\'.
            // modEntry.Active - Active or inactive.
            // modEntry.Logger - Writes logs to the 'Log.txt' file.
            // modEntry.OnToggle - The presence of this function will let the mod manager know that the mod can be safely disabled during the game.
            modEntry.OnToggle = OnToggle;
            // modEntry.OnGUI - Called to draw UI.
            // modEntry.OnSaveGUI - Called while saving.
            // modEntry.OnUpdate - Called by MonoBehaviour.Update.
            // modEntry.OnLateUpdate - Called by MonoBehaviour.LateUpdate.
            // modEntry.OnFixedUpdate - Called by MonoBehaviour.FixedUpdate.

            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            enabled = true;

            return true; // If false the mod will show an error.
        }

        // Called when the mod is turned to on/off.
        // With this function you control an operation of the mod and inform users whether it is enabled or not.
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value /* active or inactive */) {
            if (enabled != value) {
                enabled = value;
                if (enabled) {
                    harmony.PatchAll(Assembly.GetExecutingAssembly());
                }
                else {
                    harmony.UnpatchAll();
                }
            }
            return true; // If true, the mod will switch the state. If not, the state will not change.
        }



        public void Init()
        {
            m_activatedState = AlertState.NoAlert;
            m_autoActivated = false;

            Debug.Log("[MOD] AutoAlerts activated");
        }

        public void Update()
        {
            if (ConstructionComponent.findOperational(TypeList<ComponentType, ComponentTypeList>.find<SecurityConsole>()) == null)
                return;

            AlertState state = SecurityManager.getInstance().getAlertState();

            // if the state has been changed manually, don't do anything else. Will be activated again if the player sets NoAlert
            if (state != m_activatedState)
            {
                m_activatedState = AlertState.NoAlert;
                m_autoActivated = false;
                return;
            }

            List<Character> intruders = Character.getSpecializationCharacters(SpecializationList.IntruderInstance);
            if (intruders != null)
            {
                foreach (Character intruder in intruders)
                {
                    if (intruder.hasStatusFlag(Character.StatusFlagDetected))
                    {
                        // check number of guards vs intruders - want to keep on yellow while ratio guards/intruders is high enough
                        float numIntruders = intruders.Count;
                        float numGuards = Character.getCountOfSpecialization(TypeList<Specialization, SpecializationList>.find<Guard>());

                        float ratio = numGuards / numIntruders;
                        AlertState newState = ratio < 0.75f ? AlertState.RedAlert : AlertState.YellowAlert;

                        if (newState != m_activatedState)
                        {
                            SecurityManager.getInstance().setAlertState(newState);
                            m_activatedState = newState;
                            m_autoActivated = true;
                        }

                        return;
                    }
                }
            }

            if (DisasterManager.getInstance().anyInProgress())
            {
                if (state != AlertState.YellowAlert)
                {
                    SecurityManager.getInstance().setAlertState(AlertState.YellowAlert);
                    m_activatedState = AlertState.YellowAlert;
                    m_autoActivated = true;
                }

                return;
            }

            if (m_autoActivated)
            {
                // Only disable alert if it's the same one we set
                if (state == m_activatedState)
                    SecurityManager.getInstance().setAlertState(AlertState.NoAlert);

                m_activatedState = AlertState.NoAlert;
                m_autoActivated = false;
            }
        }
    }
}
