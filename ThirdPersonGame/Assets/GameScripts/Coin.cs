using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private float _rotateSpeed = 18f;
    [SerializeField] private float _collectAnimationDuration = 0.3f;
    [SerializeField] private int _coinValue = 1;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem _collectParticles;
    [SerializeField] private float _floatAmplitude = 0.2f;
    [SerializeField] private float _floatSpeed = 2f;
    
    private bool _isCollected = false;
    private Vector3 _startPosition;
    private CoinCollector _collector;
    
    public int CoinValue => _coinValue;
    public bool IsCollected => _isCollected;
    
    private void Start()
    {
        _startPosition = transform.position;
        StartCoroutine(DoRotate());
        StartCoroutine(DoFloatAnimation());
        
        // Находим коллектор на игроке
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _collector = player.GetComponent<CoinCollector>();
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag and CoinCollector component");
        }
    }
    
    private IEnumerator DoRotate()
    {
        while (true)
        {
            transform.rotation *= Quaternion.Euler(0, _rotateSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }
    
    private IEnumerator DoFloatAnimation()
    {
        float time = 0;
        while (!_isCollected)
        {
            time += Time.deltaTime * _floatSpeed;
            float offsetY = Mathf.Sin(time) * _floatAmplitude;
            transform.position = _startPosition + new Vector3(0, offsetY, 0);
            yield return null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            if (_collector == null)
            {
                _collector = other.GetComponent<CoinCollector>();
            }
            
            if (_collector != null)
            {
                _collector.CollectCoin(this);
            }
            else
            {
                // Если нет коллектора, просто собираем монетку без счёта
                Collect();
            }
        }
    }
    
    public void Collect()
    {
        if (_isCollected) return;
        
        _isCollected = true;
        
        // Воспроизводим партиклы
        if (_collectParticles != null)
        {
            ParticleSystem particles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }
        
        // Анимация сбора (уменьшение и исчезновение)
        StartCoroutine(CollectAnimation());
    }
    
    private IEnumerator CollectAnimation()
    {
        float elapsedTime = 0;
        Vector3 originalScale = transform.localScale;
        
        while (elapsedTime < _collectAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _collectAnimationDuration;
            
            // Уменьшаем размер
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}