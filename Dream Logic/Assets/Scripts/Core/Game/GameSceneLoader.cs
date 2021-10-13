using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class GameSceneLoader : MonoBehaviour
    {
        public const int mainMenu = 0;
        public const int dreamScene = 1;

        private static GameSceneLoader instance;

        [SerializeField]
        private AssetReference[] _scenes;
        private static AssetReference[] scenes;

        private void Awake()
        {
            scenes = _scenes;
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
            instance.StartCoroutine(LoadScene_Internal(scenes[index]));
        }

        public static void LoadScene(AssetReference scene)
        {
            instance.StartCoroutine(LoadScene_Internal(scene));
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

        private static IEnumerator LoadScene_Internal(AssetReference scene)
        {
            GameLoadingScreen oldScreen = FindObjectOfType<GameLoadingScreen>(true);

            oldScreen.gameObject.SetActive(true);
            yield return GameUI.FadeUIFromTo(oldScreen.background, true, 0f, 1f);

            yield return Addressables.LoadSceneAsync(scene);

            GameLoadingScreen newScreen = FindObjectOfType<GameLoadingScreen>(true);

            newScreen.gameObject.SetActive(true);
            yield return GameUI.FadeUIFromTo(newScreen.background, false, 1f, 0f);
            newScreen.gameObject.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
