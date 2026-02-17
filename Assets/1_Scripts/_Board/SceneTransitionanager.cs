using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionanager : MonoBehaviour
{
    public static Action OnSceneReady;
    public static Action OnSceneReadyFirstTime;

    public static SceneTransitionanager instance;

    public CanvasGroup fade;
    private bool _IsTransitioning;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // FuncToCall

    public void LoadSingle(string pSceneName)
    {
        StartCoroutine(DoTransition(pSceneName, LoadSceneMode.Single));
    }

    public void LoadAdditive(string pSceneName)
    {
        StartCoroutine(DoTransition(pSceneName, LoadSceneMode.Additive));
    }

    public void UnloadAdditive(string pSceneName)
    {
        StartCoroutine(UnloadRoutine(pSceneName));
    }



    // transitions
    IEnumerator DoTransition(string pSceneName, LoadSceneMode pMode)
    {
        if (_IsTransitioning) yield break;
        _IsTransitioning = true;

        // fade out current scene
        yield return Fade(1f, 0.5f);

        // Load scene 
        AsyncOperation op = SceneManager.LoadSceneAsync(pSceneName, pMode);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        yield return new WaitForSeconds(0.3f);

        op.allowSceneActivation = true;
        yield return null;

        // persistnt ou pas
        Scene loadedScene = SceneManager.GetSceneByName(pSceneName);
        if (pMode == LoadSceneMode.Additive)
            SceneManager.SetActiveScene(loadedScene);

        // Intro
        yield return Fade(0f, 0.5f);

        OnSceneReady?.Invoke();

        _IsTransitioning = false;
    }

    IEnumerator UnloadRoutine(string pSceneName)
    {
        if (_IsTransitioning) yield break;
        _IsTransitioning = true;

        yield return Fade(1f, 0.5f);

        AsyncOperation op = SceneManager.UnloadSceneAsync(pSceneName);
        while (!op.isDone)
            yield return null;

        // remettre la scène board active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BoardGame"));

        yield return Fade(0f, 0.5f);

        OnSceneReady?.Invoke();

        _IsTransitioning = false;
    }

    private IEnumerator Fade(float pTarget, float pTime)
    {
        float lElapsTime = 0;

        while (lElapsTime < pTime)
        {
            lElapsTime += Time.deltaTime;

            // to add fade

            yield return null;
        }
    }

    public void SwitchOverlay(string toUnload, string toLoad)
    {
        StartCoroutine(SwitchRoutine(toUnload, toLoad));
    }

    IEnumerator SwitchRoutine(string unloadScene, string loadScene)
    {
        if (_IsTransitioning) yield break;
        _IsTransitioning = true;

        yield return SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

        if (SceneManager.GetSceneByName(unloadScene).isLoaded)
            yield return SceneManager.UnloadSceneAsync(unloadScene);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadScene));

        OnSceneReadyFirstTime?.Invoke();
        _IsTransitioning = false;
    }
}