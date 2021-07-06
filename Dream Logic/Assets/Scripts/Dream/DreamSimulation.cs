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
        private static Vector3 defaultPlayerPosition;

        private static Transform _environment;
        public static Transform environment => _environment;

        private static float timeCounter;
        private static float maxTime = 15f;

        public static float score { get; set; }

        private static DreamThemeSwitcher themeSwitcher;
        private static DreamModeSwitcher modeSwitcher;
        private static DreamDifficulty _difficulty;
        public static DreamDifficulty difficulty => _difficulty;
        private static DreamUI _ui;
        public static DreamUI ui => _ui;

        private static FloorSpawner _floorSpawner;
        public static FloorSpawner floorSpawner => _floorSpawner;

        [SerializeField]
        private float _voidHeight;
        private static float voidHeight;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            defaultPlayerPosition = player.tr.position;
            _environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;

            themeSwitcher = GetComponent<DreamThemeSwitcher>();
            modeSwitcher = GetComponent<DreamModeSwitcher>();
            _difficulty = GetComponent<DreamDifficulty>();
            _ui = GetComponent<DreamUI>();

            _floorSpawner = FindObjectOfType<FloorSpawner>();

            timeCounter = maxTime * difficulty.dreamDurationMultiplier - Time.deltaTime;
            voidHeight = _voidHeight;

            score = 0f;
        }

        private void Update()
        {
            Simulate();
            CheckPlayerPosition();
        }

        private void Simulate()
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

        private void CheckPlayerPosition()
        {
            if (player.tr.position.y < voidHeight)
            {
                player.Move(defaultPlayerPosition);
            }
        }

        public static void WakeUp()
        {
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            timeCounter = -float.NegativeInfinity;
            ui.Stop();
        }

        public static void Restart()
        {
            ui.Restart();
            timeCounter = maxTime * difficulty.dreamDurationMultiplier;
            player.Move(defaultPlayerPosition);
            score = 0f;
            difficulty.ResetDifficulty();
        }

        public IEnumerator StartNewDreamCycle()
        {
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            StartCoroutine(DreamUI.FadeUI(ui.score, false));
            yield return new WaitForSeconds(5f);
            themeSwitcher.SetRandomTheme();
            modeSwitcher.SetAllowedMode(themeSwitcher.currentTheme);
            ui.DisplayDescription(themeSwitcher.currentTheme.description, modeSwitcher.GetCurrentModeDescription());
            StartCoroutine(DreamUI.FadeUI(ui.score, true));
            difficulty.RaiseDifficulty();
        }
    }
}
