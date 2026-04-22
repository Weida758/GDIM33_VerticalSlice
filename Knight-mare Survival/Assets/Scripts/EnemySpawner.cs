using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public float waveInterval = 15f;
    public int enemiesPerWave = 8;
    public float spawnBuffer = 1.5f;
    public int maxActiveEnemies = 300;

    public GameObject earlyEnemyPrefab;
    public GameObject lateEnemyPrefab;
    public float lateEnemyUnlockTime = 120f;

    private float timer;
    private float elapsed;
    private Camera cam;
    private float cachedSpawnDistance;
    private float cachedAspect;
    private float cachedOrthoSize;

    void Start()
    {
        cam = Camera.main;
        timer = waveInterval;
        RecalculateSpawnDistance();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        elapsed += dt;
        timer -= dt;

        if (cam.orthographicSize != cachedOrthoSize || cam.aspect != cachedAspect)
            RecalculateSpawnDistance();

        if (timer <= 0f)
        {
            SpawnWave();
            timer = waveInterval;
        }
    }

    void RecalculateSpawnDistance()
    {
        cachedOrthoSize = cam.orthographicSize;
        cachedAspect = cam.aspect;
        float camWidth = cachedOrthoSize * cachedAspect;
        cachedSpawnDistance = Mathf.Sqrt(camWidth * camWidth + cachedOrthoSize * cachedOrthoSize) + spawnBuffer;
    }

    void SpawnWave()
    {
        int active = transform.parent != null ? 0 : EnemyTracker.ActiveCount;
        int toSpawn = Mathf.Min(enemiesPerWave, maxActiveEnemies - EnemyTracker.ActiveCount);
        if (toSpawn <= 0) return;

        Vector2 playerPos = player.position;
        bool lateUnlocked = lateEnemyPrefab != null && elapsed >= lateEnemyUnlockTime;

        for (int i = 0; i < toSpawn; i++)
        {
            float angle = Random.Range(0f, 6.2831853f);
            Vector2 pos = new Vector2(
                playerPos.x + Mathf.Cos(angle) * cachedSpawnDistance,
                playerPos.y + Mathf.Sin(angle) * cachedSpawnDistance
            );

            GameObject prefab = (lateUnlocked && Random.value < 0.5f) ? lateEnemyPrefab : earlyEnemyPrefab;
            EnemyPool.Instance.Get(prefab, pos);
        }
    }
}