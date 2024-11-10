using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;        // Prefab của zombie
    public Transform portal;               // Vị trí portal để spawn zombie
    public int baseZombiesPerWave = 5;     // Số lượng zombie ban đầu mỗi đợt
    public float timeBetweenWaves = 10f;   // Thời gian giữa các đợt
    public float spawnInterval = 1f;       // Thời gian giữa các zombie trong cùng một đợt

    public int currentWave = 0;
    private bool isSpawning = false;

    public void Init()
    {
        StartCoroutine(SpawnWaves());
    }

    public IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
            Debug.Log("Starting wave " + currentWave);

            // Tăng số lượng zombie theo wave (có thể thay đổi công thức tùy theo nhu cầu)
            int zombiesInCurrentWave = baseZombiesPerWave + (currentWave * 2);  // Tăng số zombie mỗi wave

            // Gọi hàm spawn zombie theo từng đợt
            yield return StartCoroutine(SpawnZombiesInWave(zombiesInCurrentWave));
        }
    }

    private IEnumerator SpawnZombiesInWave(int zombiesToSpawn)
    {
        isSpawning = true;

        // Giới hạn tối đa số lượng zombie trong một wave (có thể tùy chỉnh)
        int maxZombiesPerWave = 50;
        zombiesToSpawn = Mathf.Min(zombiesToSpawn, maxZombiesPerWave);

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            Instantiate(zombiePrefab, portal.position, portal.rotation);
        }
        else
        {
            Debug.LogWarning("Zombie Prefab is missing or has been destroyed.");
        }
    }
}
