using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private const int _maxLevel = 1;
    private static LevelManager _instance;
    private Coroutine _currentLoadingCoroutine;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool IsLevelCorrect(int level)
    {
        return level is > 0 and <= _maxLevel;
    }

    public static void StopLoading()
    {
        if (_instance != null && _instance._currentLoadingCoroutine != null)
        {
            _instance.StopCoroutine(_instance._currentLoadingCoroutine);
            _instance._currentLoadingCoroutine = null;
            Debug.Log("Loading stopped");
        }
    }

    public static IEnumerator LoadByCoroutine(int level)
    {
        Debug.Log($"Start loading level [{level}]");
        
        // Эмулируем загрузку с задержкой
        float elapsedTime = 0f;
        float loadDelay = 3f;
        
        while (elapsedTime < loadDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Debug.Log($"Level [{level}] Loaded");
        
        if (_instance != null)
        {
            _instance._currentLoadingCoroutine = null;
        }
    }
}