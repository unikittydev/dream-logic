using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        StartCoroutine(LoadScene(1));
    }

    private IEnumerator LoadScene(int index)
    {
        int closeIndex = SceneManager.GetActiveScene().buildIndex;

        var a = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        while (!a.isDone)
            yield return null;

        SceneManager.UnloadSceneAsync(closeIndex);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(index));
    }
}
