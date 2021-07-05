using Game.Dream;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// UI главного меню.
    /// </summary>
    public class MenuRoomUI : MonoBehaviour
    {
        private const string launchedBefore = "LAUNCHED_BEFORE";

        private static bool seenGreet;

        [SerializeField]
        private TMP_Text message;
        [SerializeField]
        private Image greetPanel;
        [SerializeField]
        private TMP_Text _navigationText;
        public TMP_Text navigationText => _navigationText;

        [SerializeField]
        private float showTime;
        [SerializeField]
        private float pauseTime;
        [SerializeField]
        private float _addLetterTime;
        public float addLetterTime => _addLetterTime;

        [SerializeField]
        private string[] firstGreetingMessages;

        [SerializeField]
        private string finalGreetingMessage;

        private MenuRoomButton[] buttons;

        private void Awake()
        {
            if (!seenGreet)
            {
                buttons = FindObjectsOfType<MenuRoomButton>();

                foreach (var button in buttons)
                    button.buttonEnabled = false;
                greetPanel.gameObject.SetActive(true);
                StartCoroutine(GreetingMessage());
            }
            seenGreet = true;
            PlayerPrefs.SetInt(launchedBefore, 1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ShowNavigationText(string hintText)
        {
            navigationText.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DreamUI.DisplayText(navigationText, hintText, addLetterTime));
        }

        private IEnumerator GreetingMessage()
        {
            if (!PlayerPrefs.HasKey(launchedBefore))
                for (int i = 0; i < firstGreetingMessages.Length; i++)
                {
                    message.SetText(string.Empty);
                    yield return StartCoroutine(DreamUI.FadeUI(message, true));
                    yield return StartCoroutine(DreamUI.DisplayText(message, firstGreetingMessages[i], addLetterTime));
                    yield return new WaitForSeconds(showTime);
                    yield return StartCoroutine(DreamUI.FadeUI(message, false));
                    yield return new WaitForSeconds(pauseTime);
                }

            message.SetText(string.Empty);
            yield return StartCoroutine(DreamUI.FadeUI(message, true));
            yield return StartCoroutine(DreamUI.DisplayText(message, finalGreetingMessage, addLetterTime));
            yield return new WaitForSeconds(showTime);

            StartCoroutine(DreamUI.FadeUI(message, false, 0f, showTime * 2f));
            yield return StartCoroutine(DreamUI.FadeUI(greetPanel, false, 0f, showTime * 2f));

            foreach (var button in buttons)
                button.buttonEnabled = true;
        }
    }
}
