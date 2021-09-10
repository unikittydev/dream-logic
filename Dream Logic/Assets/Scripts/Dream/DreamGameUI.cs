using UnityEngine;
using UnityEngine.UI;

namespace Game.Dream
{
    public class DreamGameUI : MonoBehaviour
    {
        [SerializeField]
        private Image pausePanel;
        [SerializeField]
        private Image lostPanel;

        private Coroutine fadePause;

        public void Pause()
        {
            if (lostPanel.gameObject.activeSelf)
                return;
            Time.timeScale = 0f;

            if (fadePause != null)
                GameUI.StopUICoroutine(fadePause);
            fadePause = GameUI.FadeUI(pausePanel, true, alpha: .75f);
        }

        public void TogglePause()
        {
            if (lostPanel.gameObject.activeSelf)
                return;
            if (Time.timeScale > 0f)
                Pause();
            else
                Resume();
        }

        public void Resume()
        {
            if (lostPanel.gameObject.activeSelf)
                return;
            Time.timeScale = 1f;

            if (fadePause != null)
                GameUI.StopUICoroutine(fadePause);
            fadePause = GameUI.FadeUI(pausePanel, false);
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            GameUI.FadeUI(lostPanel, false);
        }

        public void Stop()
        {
            Time.timeScale = 0f;
            GameUI.FadeUI(lostPanel, true, alpha: .75f);
        }

        public void Exit()
        {
            GameSceneLoader.LoadScene(GameSceneLoader.mainMenu);
        }
    }
}
