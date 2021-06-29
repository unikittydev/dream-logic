using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Симуляция снов.
    /// </summary>
    public class DreamSimulation : MonoBehaviour
    {
        private static Dream currentDream;

        public static void WakeUp()
        {
            EndDream();
        }

        public static void StartNewDreamCycle()
        {
            EndDream();
            ChooseTheme();
            ApplyTheme();
            ChooseRule();
            ApplyRule();
            StartDream();
        }

        private static void ChooseTheme()
        {

        }

        private static void ApplyTheme()
        {

        }

        private static void ChooseRule()
        {

        }

        private static void ApplyRule()
        {

        }

        private static void StartDream()
        {

        }

        private static void EndDream()
        {

        }
    }
}
