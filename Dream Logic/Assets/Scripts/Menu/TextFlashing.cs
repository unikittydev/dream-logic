using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFlashing : MonoBehaviour
    {
        private TMP_Text text;

        [SerializeField]
        private LocalizedString zzzText;

        [SerializeField]
        private float displayTime;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        public Coroutine DisplayText()
        {
            return GameUI.DisplayText(text, zzzText.GetLocalizedString(), displayTime);
        }

        public void Clear()
        {
            text.SetText(string.Empty);
        }
    }
}
