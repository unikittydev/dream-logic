using System.Collections;
using UnityEngine;

namespace Game.Dream
{
    public class DreamCycle : MonoBehaviour
    {
        private float timeCounter;
        [SerializeField]
        private float maxCycleTime = 15f;

        public float totalTime => maxCycleTime * difficulty.dreamDurationMultiplier;

        private DreamThemeSwitcher themeSwitcher;
        private DreamModeSwitcher modeSwitcher;

        private DreamDifficulty difficulty;

        private DreamDescriptionUI ui;

        private void Awake()
        {
            themeSwitcher = GetComponent<DreamThemeSwitcher>();
            modeSwitcher = GetComponent<DreamModeSwitcher>();

            difficulty = GetComponent<DreamDifficulty>();

            ui = GetComponent<DreamDescriptionUI>();
        }

        private void Start()
        {
            StartCoroutine(StartNewDreamCycle());
        }

        private void OnEnable()
        {
            timeCounter = maxCycleTime;
        }

        private void Update()
        {
            Simulate();
        }

        private void Simulate()
        {
            if (timeCounter >= totalTime)
            {
                StartCoroutine(StartNewDreamCycle());
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        private IEnumerator StartNewDreamCycle()
        {
            timeCounter = -5f;
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            var newTheme = StartCoroutine(themeSwitcher.LoadRandomTheme());
            // Загрузить новую тему
            // Загрузить новый режим
            GameUI.FadeUI(DreamScore.scoreText, false);
            yield return new WaitForSeconds(5f);
            yield return newTheme;
            themeSwitcher.SwitchTheme();
            // Применить эту тему
            // Применить этот режим
            modeSwitcher.SetRandomMode(themeSwitcher.currentTheme);
            ui.DisplayDescription(themeSwitcher.currentTheme.description, modeSwitcher.currentMode.description);
            GameUI.FadeUI(DreamScore.scoreText, true);
            difficulty.RaiseDifficulty();
        }
    }
}
