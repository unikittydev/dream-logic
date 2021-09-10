using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneLoader : MonoBehaviour
    {
        public const int mainMenu = 0;
        public const int dreamScene = 1;

        private static GameSceneLoader instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static void LoadScene(int index)
        {
            instance.StartCoroutine(LoadScene_Internal(index));
        }

        public static void Quit()
        {
            Application.Quit();
        }

        private static IEnumerator LoadScene_Internal(int sceneIndex)
        {
            GameLoadingScreen oldScreen = FindObjectOfType<GameLoadingScreen>(true);

            oldScreen.gameObject.SetActive(true);
            yield return GameUI.FadeUIFromTo(oldScreen.background, true, 0f, 1f);

            yield return SceneManager.LoadSceneAsync(sceneIndex);

            GameLoadingScreen newScreen = FindObjectOfType<GameLoadingScreen>(true);

            newScreen.gameObject.SetActive(true);
            yield return GameUI.FadeUIFromTo(newScreen.background, false, 1f, 0f);
            newScreen.gameObject.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
