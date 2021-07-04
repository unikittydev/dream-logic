using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Dream
{
    /// <summary>
    /// Симуляция снов.
    /// </summary>
    public class DreamSimulation : MonoBehaviour
    {
        public static UnityEvent<PlayerController, ControllerColliderHit> onPlayerHit { get; } = new UnityEvent<PlayerController, ControllerColliderHit>();

        public static PlayerController player { get; set; }

        private static Transform _environment;
        public static Transform environment => _environment;

        private static float timeCounter;
        private static float maxTime = 15f;

        public static float score { get; set; }

        private static DreamThemeSwitcher themeSwitcher;
        private static DreamModeSwitcher modeSwitcher;
        private static DreamDifficulty _difficulty;
        public static DreamDifficulty difficulty => _difficulty;

        private static FloorSpawner _floorSpawner;
        public static FloorSpawner floorSpawner => _floorSpawner;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            _environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;

            themeSwitcher = GetComponent<DreamThemeSwitcher>();
            modeSwitcher = GetComponent<DreamModeSwitcher>();
            _difficulty = GetComponent<DreamDifficulty>();

            _floorSpawner = FindObjectOfType<FloorSpawner>();

            timeCounter = maxTime;
        }

        private void Update()
        {
            if (timeCounter >= maxTime * difficulty.dreamDurationMultiplier)
            {
                StartCoroutine(StartNewDreamCycle());
                timeCounter = 0f;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        public static void WakeUp()
        {
            print($"You woke up with score [{(int)score}]");
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            timeCounter = -float.NegativeInfinity;
        }

        public IEnumerator StartNewDreamCycle()
        {
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            yield return new WaitForSeconds(5f);
            themeSwitcher.SetRandomTheme();
            modeSwitcher.SetAllowedMode(themeSwitcher.currentTheme);
            difficulty.RaiseDifficulty();
        }
    }
}
