using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// Класс для работы с UI-элементами в игре.
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        private static GameUI instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance == this)
                Destroy(gameObject);
        }

        public static Coroutine DisplayText(TMP_Text ui, string newText, float time)
        {
            return instance.StartCoroutine(DisplayText_Internal(ui, newText.ToCharArray(), time));
        }

        public static Coroutine DisplayText(TMP_Text ui, char[] charArray, float time)
        {
            return instance.StartCoroutine(DisplayText_Internal(ui, charArray, time));
        }

        private static IEnumerator DisplayText_Internal(TMP_Text ui, char[] charArray, float time)
        {
            ui.SetText(string.Empty);
            float counter = 0f;

            for (int i = 0; i < charArray.Length; i++)
            {
                while (counter < time)
                {
                    counter += Time.unscaledDeltaTime;
                    yield return null;
                }
                ui.SetText(charArray, 0, i + 1);
                counter = 0f;
            }
        }

        public static Coroutine FadeUIFromTo(Graphic ui, bool enable, float fromAlpha = 0f, float toAlpha = 1f, float time = .5f)
        {
            Color col = ui.color;
            col.a = fromAlpha;
            ui.color = col;
            return instance.StartCoroutine(FadeUI_Internal(ui, enable, fromAlpha, toAlpha, time));
        }

        public static Coroutine FadeUI(Graphic ui, bool enable, float alpha = 1f, float time = .5f)
        {
            return instance.StartCoroutine(FadeUI_Internal(ui, enable, ui.color.a, enable ? alpha : 0f, time));
        }

        public static void StopUICoroutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }

        private static IEnumerator FadeUI_Internal(Graphic ui, bool enable, float fromAlpha, float toAlpha, float time)
        {
            if (enable)
                ui.gameObject.SetActive(true);

            float counter = 0f;

            Color fromColor = ui.color;
            fromColor.a = fromAlpha;
            Color toColor = fromColor;
            toColor.a = toAlpha;

            while (counter <= time)
            {
                ui.color = Color.Lerp(fromColor, toColor, counter / time);
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!enable)
                ui.gameObject.SetActive(false);
        }
    }
}
