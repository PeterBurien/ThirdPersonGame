using UnityEngine;
using TMPro;
using System.Collections;

public class CoinCounterUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _coinText;
  
    [Header("Animation Settings")]
    [SerializeField] private float _popScaleMultiplier = 1.2f;
    [SerializeField] private float _popDuration = 0.2f;
    
    private CoinCollector _coinCollector;
    private Vector3 _originalIconScale;
    private int _lastCoinCount = 0;
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _coinCollector = player.GetComponent<CoinCollector>();
        }
        
        // Подписываемся на событие сбора всех монет
        EventBus.OnAllCoinsCollected += OnAllCoinsCollected;
        
        // Обновляем текст сразу
        UpdateCoinText();
    }
    
    private void OnDestroy()
    {
        EventBus.OnAllCoinsCollected -= OnAllCoinsCollected;
    }
    
    private void Update()
    {
        if (_coinCollector != null && _lastCoinCount != _coinCollector.CoinsCollected)
        {
            _lastCoinCount = _coinCollector.CoinsCollected;
          }
    }
 
    
    private IEnumerator PopAnimation(Transform target)
    {
        float elapsedTime = 0;
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * _popScaleMultiplier;
        
        // Увеличиваем
        while (elapsedTime < _popDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (_popDuration / 2);
            target.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        elapsedTime = 0;
        
        // Возвращаем обратно
        while (elapsedTime < _popDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (_popDuration / 2);
            target.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        target.localScale = originalScale;
    }
    
    private void UpdateCoinText()
    {
        if (_coinText != null && _coinCollector != null)
        {
            // ТОЛЬКО количество собранных монет, без общего количества
            _coinText.text = _coinCollector.CoinsCollected.ToString();
        }
    }
    
    private void OnAllCoinsCollected()
    {
        if (_coinText != null)
        {
            StartCoroutine(AllCoinsAnimation());
        }
    }
    
    private IEnumerator AllCoinsAnimation()
    {
        // Анимация для текста при сборе всех монет
        Color originalColor = _coinText.color;
        float elapsedTime = 0;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float pingPong = Mathf.PingPong(elapsedTime * 2, 0.5f);
            _coinText.color = Color.Lerp(originalColor, Color.yellow, pingPong);
            yield return null;
        }
        
        _coinText.color = originalColor;
    }
}