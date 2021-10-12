using Game.Dream;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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
        private LocalizedString[] firstGreetingMessages;

        [SerializeField]
        private LocalizedString finalGreetingMessage;

        private enum GreetingMode
        {
            Skip,
            ShowFinal,
            ShowAll
        }

        [SerializeField]
        private GreetingMode greetingMode;

        private void Start()
        {
            if (!seenGreet || greetingMode != GreetingMode.Skip)
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

            if (!PlayerPrefs.HasKey(launchedBefore) || greetingMode == GreetingMode.ShowAll)
                for (int i = 0; i < firstGreetingMessages.Length; i++)
                {
                    message.SetText(string.Empty);
                    yield return GameUI.FadeUI(message, true);
                    yield return GameUI.DisplayText(message, firstGreetingMessages[i].GetLocalizedString(), addLetterTime);
                    yield return new WaitForSeconds(showTime);
                    yield return GameUI.FadeUI(message, false);
                    yield return new WaitForSeconds(pauseTime);
                }

            message.SetText(string.Empty);
            yield return GameUI.FadeUI(message, true);
            yield return GameUI.DisplayText(message, finalGreetingMessage.GetLocalizedString(), addLetterTime);
            yield return new WaitForSeconds(showTime);

            GameUI.FadeUI(message, false, 0f, showTime * 2f);
            yield return GameUI.FadeUI(greetPanel, false, 0f, showTime * 2f);

            foreach (var button in buttons)
                button.enabled = true;
        }
    }
}