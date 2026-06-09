using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _loadLevelButton;
    [SerializeField] private TMP_InputField _levelInput;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Image _incorrectLevelIndicator;

    private int _selectedLevel;

    private void OnEnable()
    {
        _loadLevelButton.onClick.AddListener(OnLoadLevelClick);
        _quitButton.onClick.AddListener(OnQuitClick);
        _levelInput.onValueChanged.AddListener(OnLevelSelected);
    }

    private void OnDisable()
    {
        _loadLevelButton.onClick.RemoveListener(OnLoadLevelClick);
        _quitButton.onClick.RemoveListener(OnQuitClick);
        _levelInput.onValueChanged.RemoveListener(OnLevelSelected);
    }

    private void Start()
    {
        _incorrectLevelIndicator.enabled = false;
    }

    private void OnLoadLevelClick()
    {
        if (_selectedLevel > 0) // проверка, что уровень корректный
        {
            LoadLevel(_selectedLevel);
        }
        else
        {
            _incorrectLevelIndicator.enabled = true;
            Debug.LogError($"There is no level: {_selectedLevel}");
        }
    }

    private void OnQuitClick()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
        return;
        #endif
        Application.Quit();
    }

    private void OnLevelSelected(string value)
    {
        if (int.TryParse(value, out int parsedLevel))
        {
            _selectedLevel = parsedLevel;
            
            if (_selectedLevel > 0) // проверка, что уровень корректный
            {
                _incorrectLevelIndicator.enabled = false;
            }
        }
        else
        {
            _selectedLevel = -1;
        }
    }

    private void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}