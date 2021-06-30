using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Симуляция снов.
    /// </summary>
    public class DreamSimulation : MonoBehaviour
    {
        private const string themesPath = "Dream Themes/";

        private static Dream dream;

        private static DreamTheme[] themes;

        private void Awake()
        {
            themes = Resources.LoadAll<DreamTheme>(themesPath);
            dream = GetComponent<Dream>();

            StartNewDreamCycle();
        }

        public static void WakeUp()
        {
            dream.Stop();
            print("You woke up");
        }

        public static void StartNewDreamCycle()
        {
            dream.Stop();

            var theme = ChooseTheme();
            var rules = ChooseRules();

            dream.Play(theme, rules);
        }

        private static DreamTheme ChooseTheme()
        {
            return themes[Random.Range(0, themes.Length)];
        }

        private static DreamBehaviour ChooseRules()
        {
            return default;
        }
    }
}
