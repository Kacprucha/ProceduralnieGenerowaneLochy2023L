using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject LoadingCanvas;

    [SerializeField]
    public Slider LoadingBar;

    public void LoadScene (int sceneId)
    {
        StartCoroutine (LoadSceneAsync (sceneId));
    }

    IEnumerator LoadSceneAsync (int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync (sceneId);

        LoadingCanvas.SetActive (true);

        while (!operation.isDone)
        {
            float progressValue = operation.progress * 100;
            LoadingBar.value = progressValue;

            yield return null;
        }
    }
}
