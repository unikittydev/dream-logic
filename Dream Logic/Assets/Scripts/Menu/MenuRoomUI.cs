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
        [SerializeField]
        private TMP_Text message;
        [SerializeField]
        private Image greetPanel;

        [SerializeField]
        private float showTime;
        [SerializeField]
        private float pauseTime;
        [SerializeField]
        private float addLetterTime;

        [SerializeField]
        private string[] firstGreetingMessages;

        [SerializeField]
        private string finalGreetingMessage;

        private void Awake()
        {
            greetPanel.gameObject.SetActive(true);
            StartCoroutine(GreetingMessage());
        }

        private IEnumerator GreetingMessage()
        {
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
            StartCoroutine(DreamUI.FadeUI(greetPanel, false, 0f, showTime * 2f));
        }
    }
}
