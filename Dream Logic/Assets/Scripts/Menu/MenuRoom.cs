using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuRoom : MonoBehaviour
{
    private const int playSceneIndex = 1;

    private static MenuRoom instance;

    [SerializeField]
    private float minWaitTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public static void StartGame()
    {
        instance.StartCoroutine(instance.StartGameCoroutine());
    }

    public static void ShortWakeUp()
    {

    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(minWaitTime);

        var sceneLoader = SceneManager.LoadSceneAsync(playSceneIndex, LoadSceneMode.Additive);

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(playSceneIndex));
    }
}
