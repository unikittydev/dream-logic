using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// Скрипт, отвечающий за смену сцен.
    /// </summary>
    public class MenuRoom : MonoBehaviour
    {
        private const int mainSceneIndex = 0;
        private const int playSceneIndex = 1;

        private static MenuRoom instance;

        [SerializeField]
        private float minWaitTime;

        private void Awake()
        {
            Time.timeScale = 1f;
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else if (instance != this)
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        public static void StartGame()
        {
            instance.StartCoroutine(instance.StartGameCoroutine(playSceneIndex, instance.minWaitTime));
        }

        public static void WakeUp()
        {
            instance.StartCoroutine(instance.StartGameCoroutine(mainSceneIndex, 0f));
        }

        private IEnumerator StartGameCoroutine(int sceneIndex, float waitTime)
        {
            var listener = Camera.main.GetComponent<AudioListener>();
            FindObjectOfType<EventSystem>().gameObject.SetActive(false);
            var buttons = FindObjectsOfType<MenuRoomButton>();
            foreach (var button in buttons)
                button.buttonEnabled = false;

            yield return new WaitForSeconds(waitTime);

            var sceneLoader = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

            while (!sceneLoader.isDone)
            {
                yield return null;
            }
            listener.enabled = false;

            var sceneUnloader = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!sceneUnloader.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        }
    }
}
