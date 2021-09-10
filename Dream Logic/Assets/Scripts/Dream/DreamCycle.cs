using System.Collections;
using UnityEngine;

namespace Game.Dream
{
    public class DreamCycle : MonoBehaviour
    {
        private float timeCounter;
        [SerializeField]
        private float maxCycleTime = 15f;

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

        private void OnEnable()
        {
            StartCoroutine(StartNewDreamCycle());
        }

        private void Update()
        {
            Simulate();
        }

        private void Simulate()
        {
            if (timeCounter >= maxCycleTime * difficulty.dreamDurationMultiplier)
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
            GameUI.FadeUI(DreamScore.scoreText, false);
            yield return new WaitForSeconds(5f);
            themeSwitcher.SetRandomTheme();
            modeSwitcher.SetRandomMode(themeSwitcher.currentTheme);
            ui.DisplayDescription(themeSwitcher.currentTheme.description, modeSwitcher.currentMode.description);
            GameUI.FadeUI(DreamScore.scoreText, true);
            difficulty.RaiseDifficulty();
        }
    }
}
