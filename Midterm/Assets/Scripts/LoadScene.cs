using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    private int sceneToLoad;
    AsyncOperation loadingOperation;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingBar;
    void Start()
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }
    void Update()
    {
        
    }
    public void sceneload()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingScreen.SetActive(true);
        while (!loadingOperation.isDone)
        {
            loadingBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f); 
            yield return null;
        }
    }
}
