using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private CanvasGroup _menu;

    private void Start()
    {
        Debug.Log(Application.isPlaying);
        
        if (_menu != null)
        {
            _menu.alpha = 0;
            _menu.blocksRaycasts = false;

            Debug.Log("Level initialized");
            
            _menu.alpha = 1;
            _menu.blocksRaycasts = true;
        }
    }
}