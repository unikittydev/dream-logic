using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (instance == this)
            Destroy(gameObject);
        else
            instance = this;

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
        yield return new WaitForSeconds(waitTime);

        var sceneLoader = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!sceneLoader.isDone)
        {
            yield return null;
        }
        Camera.main.GetComponent<AudioListener>().enabled = false;

        var sceneUnloader = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!sceneUnloader.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }
}
