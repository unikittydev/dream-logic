using Game.Dream;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MenuGreeting : MonoBehaviour
    {
        private const string launchedBefore = "LAUNCHED_BEFORE";

        private static bool seenGreet;

        [SerializeField]
        private Image greetPanel;
        [SerializeField]
        private TMP_Text message;

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

        [SerializeField]
        private bool skipGreeting;

        private void Start()
        {
            seenGreet = seenGreet || skipGreeting;
            if (!seenGreet)
            {
                greetPanel.gameObject.SetActive(true);
                StartCoroutine(GreetingMessage());
            }
            seenGreet = true;
            PlayerPrefs.SetInt(launchedBefore, 1);
        }

        private IEnumerator GreetingMessage()
        {
            MenuRoomButton[] buttons = FindObjectsOfType<MenuRoomButton>();

            foreach (var button in buttons)
                button.enabled = false;

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
                button.enabled = true;
        }
    }
}
