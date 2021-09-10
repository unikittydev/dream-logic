using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Пользовательский интерфейс сцены снов.
    /// </summary>
    public class DreamDescriptionUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text themeDesc;
        [SerializeField]
        private TMP_Text modeDesc;

        [SerializeField]
        private float idleTime;
        [SerializeField]
        private float addLetterTime;

        private Coroutine themeCoroutine;
        private Coroutine modeCoroutine;
        
        public void DisplayDescription(string theme, string mode)
        {
            if (themeCoroutine != null)
                StopCoroutine(themeCoroutine);
            if (modeCoroutine != null)
                StopCoroutine(modeCoroutine);
            themeCoroutine = StartCoroutine(DisplayDescription_Internal(themeDesc, theme));
            modeCoroutine = StartCoroutine(DisplayDescription_Internal(modeDesc, mode));
        }

        private IEnumerator DisplayDescription_Internal(TMP_Text ui, string text)
        {
            ui.SetText(string.Empty);
            GameUI.FadeUI(ui, true);
            yield return GameUI.DisplayText(ui, text, addLetterTime);
            yield return new WaitForSeconds(idleTime);
            GameUI.FadeUI(ui, false);
        }
    }
}

