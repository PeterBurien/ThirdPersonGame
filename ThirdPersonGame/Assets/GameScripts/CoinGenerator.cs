using System.Collections;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private BoxCollider _spawnArea;
    [SerializeField] private GameObject _coinPrefab;

    private void OnEnable()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        Debug.Log("Start spawn");

        for (int i = 0; i < 10; i++)
        {
            var bounds = _spawnArea.bounds;
            var randomPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );

            Instantiate(_coinPrefab, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.05f); // 50ms delay
        }

        Debug.Log("All coins were spawned");
    }
}