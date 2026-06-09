using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _coinCounterText;
    [SerializeField] private GameObject _collectionEffectPrefab;
    
    
    [Header("Settings")]
    [SerializeField] private bool _showDebugLogs = true;
    
    private int _coinsCollected = 0;
    private int _totalCoinsInLevel = 0;
    private AudioSource _audioSource;
    
    public int CoinsCollected => _coinsCollected;
    public int TotalCoinsInLevel => _totalCoinsInLevel;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        UpdateCoinUI();
        CountTotalCoins();
    }
    
    private void CountTotalCoins()
    {
        Coin[] allCoins = FindObjectsByType<Coin>(FindObjectsSortMode.None);
        _totalCoinsInLevel = allCoins.Length;
        
        if (_showDebugLogs)
            Debug.Log($"Total coins in level: {_totalCoinsInLevel}");
    }
    
    public void CollectCoin(Coin coin)
    {
        _coinsCollected++;
        
        if (_showDebugLogs)
            Debug.Log($"Coin collected! Total: {_coinsCollected}/{_totalCoinsInLevel}");
        
        
        // Создаём визуальный эффект
        if (_collectionEffectPrefab != null)
        {
            GameObject effect = Instantiate(_collectionEffectPrefab, coin.transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        
        // Обновляем UI
        UpdateCoinUI();
        
        // Уничтожаем монетку
        coin.Collect();
        
        // Проверяем, собраны ли все монеты
        if (_coinsCollected >= _totalCoinsInLevel && _totalCoinsInLevel > 0)
        {
            OnAllCoinsCollected();
        }
    }
    
    private void UpdateCoinUI()
    {
        if (_coinCounterText != null)
        {
            _coinCounterText.text = _coinsCollected.ToString();
        }
    }
    
    private void OnAllCoinsCollected()
    {
        if (_showDebugLogs)
            Debug.Log("All coins collected! Great job!");
        
        // Здесь можно добавить логику при сборе всех монет
        // Например: открыть дверь, показать сообщение, загрузить следующий уровень и т.д.
        
        // Отправляем событие, которое могут использовать другие скрипты
        EventBus.TriggerOnAllCoinsCollected();
    }
    
    public void ResetCounter()
    {
        _coinsCollected = 0;
        CountTotalCoins();
        UpdateCoinUI();
        
        if (_showDebugLogs)
            Debug.Log("Coin counter reset");
    }
}