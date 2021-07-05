using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dream
{
    /// <summary>
    /// Пользовательский интерфейс сцены снов.
    /// </summary>
    public class DreamUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _score;
        public TMP_Text score => _score;

        [SerializeField]
        private TMP_Text themeDesc;
        [SerializeField]
        private TMP_Text modeDesc;

        [SerializeField]
        private Image pausePanel;
        [SerializeField]
        private Image lostPanel;

        [SerializeField]
        private float idleTime;
        [SerializeField]
        private float addLetterTime;

        private Coroutine themeCoroutine;
        private Coroutine modeCoroutine;

        private void Update()
        {
            score.SetText(((int)DreamSimulation.score).ToString());

            if (Input.GetKeyDown(KeyCode.Escape) && !lostPanel.gameObject.activeSelf)
            {
                if (Time.timeScale < 1f)
                    Resume();
                else
                    Pause();
            }
        }

        public void Pause()
        {
            StartCoroutine(FadeUI(pausePanel, true, alpha: .75f));
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            StartCoroutine(FadeUI(pausePanel, false, alpha: .75f));
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            StartCoroutine(FadeUI(lostPanel, false, alpha: .75f));
        }

        public void Stop()
        {
            StartCoroutine(FadeUI(lostPanel, true, alpha: .75f));
            Time.timeScale = 0f;
        }

        public void Exit()
        {
            MenuRoom.WakeUp();
        }

        public void DisplayDescription(string theme, string mode)
        {
            if (themeCoroutine != null)
                StopCoroutine(themeCoroutine);
            if (modeCoroutine != null)
                StopCoroutine(modeCoroutine);
            themeCoroutine = StartCoroutine(DisplayText(theme, themeDesc));
            modeCoroutine = StartCoroutine(DisplayText(mode, modeDesc));
        }

        private IEnumerator DisplayText(string text, TMP_Text ui)
        {
            ui.SetText(string.Empty);
            StartCoroutine(FadeUI(ui, true));

            float counter = 0f;

            for (int i = 0; i < text.Length; i++)
            {
                while (counter < addLetterTime)
                {
                    counter += Time.deltaTime;
                    yield return null;
                }
                ui.SetText(string.Concat(ui.text, text[i]));
                counter = 0f;
            }
            yield return new WaitForSeconds(idleTime);
            StartCoroutine(FadeUI(ui, false));
        }

        public static IEnumerator FadeUI(Behaviour ui, bool enable, float alpha = 1f, float time = .5f)
        {
            Func<Color> getColor = () => ui is TMP_Text ? (ui as TMP_Text).color : (ui as Image).color;
            Action<Color> setColor = (color) =>
            {
                if (ui is TMP_Text)
                    (ui as TMP_Text).color = color;
                else
                    (ui as Image).color = color;
            };

            if (enable)
                ui.gameObject.SetActive(true);

            float startAlpha = getColor().a;
            float endAlpha = enable ? alpha : 0f;

            float counter = 0f;

            while (counter <= time)
            {
                Color col = getColor();
                col.a = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(counter / time));
                setColor(col);
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!enable)
                ui.gameObject.SetActive(false);
        }
    }
}

