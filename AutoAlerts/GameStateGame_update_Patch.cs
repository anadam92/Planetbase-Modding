using Planetbase;
using System.Collections.Generic;
using HarmonyLib;

namespace AutoAlerts {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        /// <summary>
        /// mod specific: true if the planetbase alert has been auto-activated (activated by this mod), false if activated manually by the player
        /// </summary>
        private static bool m_autoActivated;

        /// <summary>
        /// mod specific: the type of the alert
        /// </summary>
        private static AlertState m_activatedState;

        [HarmonyPostfix]
        public static void Postfix() {
            if (ConstructionComponent.findOperational(TypeList<ComponentType, ComponentTypeList>.find<SecurityConsole>()) == null)
                return;

            AlertState state = SecurityManager.getInstance().getAlertState();

            // if the state has been changed manually, don't do anything else. Will be activated again if the player sets NoAlert
            if (state != m_activatedState) {
                m_activatedState = AlertState.NoAlert;
                m_autoActivated = false;
                return;
            }

            List<Character> intruders = Character.getSpecializationCharacters(SpecializationList.IntruderInstance);
            if (intruders != null) {
                foreach (Character intruder in intruders) {
                    if (intruder.hasStatusFlag(Character.StatusFlagDetected)) {
                        // check number of guards vs intruders - want to keep on yellow while ratio guards/intruders is high enough
                        float numIntruders = intruders.Count;
                        float numGuards = Character.getCountOfSpecialization(TypeList<Specialization, SpecializationList>.find<Guard>());

                        float ratio = numGuards / numIntruders;
                        AlertState newState = ratio < 0.75f ? AlertState.RedAlert : AlertState.YellowAlert;

                        if (newState != m_activatedState) {
                            SecurityManager.getInstance().setAlertState(newState);
                            m_activatedState = newState;
                            m_autoActivated = true;
                        }

                        return;
                    }
                }
            }

            if (DisasterManager.getInstance().anyInProgress()) {
                if (state != AlertState.YellowAlert) {
                    SecurityManager.getInstance().setAlertState(AlertState.YellowAlert);
                    m_activatedState = AlertState.YellowAlert;
                    m_autoActivated = true;
                }

                return;
            }

            if (m_autoActivated) {
                // Only disable alert if it's the same one we set
                if (state == m_activatedState)
                    SecurityManager.getInstance().setAlertState(AlertState.NoAlert);

                m_activatedState = AlertState.NoAlert;
                m_autoActivated = false;
            }

        }
    }
}
