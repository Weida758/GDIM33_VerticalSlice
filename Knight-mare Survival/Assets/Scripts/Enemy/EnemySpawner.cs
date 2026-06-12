using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnOption
    {
        public GameObject prefab;
        public float unlockTime;
        public float maxWeight = 1f;
    }

    public Transform player;
    public float waveInterval = 6f;
    public float minWaveInterval = 6f;
    public float waveIntervalDecreasePerMinute = 0.2f;
    public int enemiesPerWave = 5;
    public int enemiesPerWaveIncrease = 1;
    public float enemyIncreaseInterval = 25f;
    public int maxEnemiesPerWave = 60;
    public float spawnBuffer = 1.5f;
    public int maxActiveEnemies = 300;

    public GameObject earlyEnemyPrefab;
    public GameObject lateEnemyPrefab;
    public float lateEnemyUnlockTime = 120f;
    public GameObject strongEnemyPrefab;
    public float strongEnemyUnlockTime = 40f;
    public GameObject bossEnemyPrefab;
    public float bossFirstSpawnTime = 60f;
    public float bossSpawnInterval = 60f;
    public float bossSpawnExtraDistance = 2f;
    public EnemySpawnOption[] extraEnemyOptions;
    public float enemyWeightRampDuration = 45f;
    public float lateEnemyMaxWeight = 1.2f;
    public float strongEnemyMaxWeight = 1.2f;
    public float healthScalePerMinute = 0.1f;
    public float damageScalePerMinute = 0.05f;
    public float speedScalePerMinute = 0.02f;
    public float bossHealthMultiplier = 8f;
    public float bossDamageMultiplier = 1.25f;
    public float bossSpeedMultiplier = 0.75f;

    private float timer;
    private float elapsed;
    private float bossTimer;
    private Camera cam;
    private float cachedSpawnDistance;
    private float cachedAspect;
    private float cachedOrthoSize;

    void Start()
    {
        cam = Camera.main;
        timer = waveInterval;
        bossTimer = bossFirstSpawnTime;
        RecalculateSpawnDistance();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        elapsed += dt;
        timer -= dt;
        bossTimer -= dt;

        if (cam.orthographicSize != cachedOrthoSize || cam.aspect != cachedAspect)
            RecalculateSpawnDistance();

        if (timer <= 0f)
        {
            SpawnWave();
            timer = GetCurrentWaveInterval();
        }

        if (bossEnemyPrefab != null && bossTimer <= 0f)
        {
            SpawnBoss();
            bossTimer = bossSpawnInterval;
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
        int currentEnemiesPerWave = GetCurrentEnemiesPerWave();
        int toSpawn = Mathf.Min(currentEnemiesPerWave, maxActiveEnemies - EnemyTracker.ActiveCount);
        if (toSpawn <= 0) return;

        Vector2 playerPos = player.position;
        bool lateUnlocked = lateEnemyPrefab != null && elapsed >= lateEnemyUnlockTime;

        for (int i = 0; i < toSpawn; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 pos = new Vector2(
                playerPos.x + Mathf.Cos(angle) * cachedSpawnDistance,
                playerPos.y + Mathf.Sin(angle) * cachedSpawnDistance
            );

            GameObject prefab = PickEnemyPrefab(lateUnlocked);
            if (prefab == null) continue;

            GameObject enemy = EnemyPool.Instance.Get(prefab, pos);
            ApplyDifficulty(enemy, false);
        }
    }

    void SpawnBoss()
    {
        if (EnemyTracker.ActiveCount >= maxActiveEnemies) return;

        Vector2 playerPos = player.position;
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float spawnDistance = cachedSpawnDistance + bossSpawnExtraDistance;
        Vector2 pos = new Vector2(
            playerPos.x + Mathf.Cos(angle) * spawnDistance,
            playerPos.y + Mathf.Sin(angle) * spawnDistance
        );

        GameObject boss = EnemyPool.Instance.Get(bossEnemyPrefab, pos);
        ApplyDifficulty(boss, true);
    }

    GameObject PickEnemyPrefab(bool lateUnlocked)
    {
        float earlyWeight = earlyEnemyPrefab != null ? 1f : 0f;
        float lateWeight = lateUnlocked && lateEnemyPrefab != null
            ? GetRampedWeight(lateEnemyUnlockTime, lateEnemyMaxWeight)
            : 0f;
        float strongWeight = strongEnemyPrefab != null && elapsed >= strongEnemyUnlockTime
            ? GetRampedWeight(strongEnemyUnlockTime, strongEnemyMaxWeight)
            : 0f;
        float extraWeight = GetExtraEnemyTotalWeight();
        float totalWeight = earlyWeight + lateWeight + strongWeight + extraWeight;

        if (totalWeight <= 0f) return earlyEnemyPrefab;

        float roll = Random.Range(0f, totalWeight);

        if (roll < earlyWeight) return earlyEnemyPrefab;
        roll -= earlyWeight;

        if (roll < lateWeight) return lateEnemyPrefab;
        roll -= lateWeight;

        if (roll < strongWeight) return strongEnemyPrefab;
        roll -= strongWeight;

        GameObject extraPrefab = PickExtraEnemyPrefab(roll);
        return extraPrefab != null ? extraPrefab : earlyEnemyPrefab;
    }

    float GetExtraEnemyTotalWeight()
    {
        if (extraEnemyOptions == null) return 0f;

        float total = 0f;
        for (int i = 0; i < extraEnemyOptions.Length; i++)
        {
            EnemySpawnOption option = extraEnemyOptions[i];
            if (option == null || option.prefab == null || elapsed < option.unlockTime) continue;
            total += GetRampedWeight(option.unlockTime, option.maxWeight);
        }

        return total;
    }

    GameObject PickExtraEnemyPrefab(float roll)
    {
        if (extraEnemyOptions == null) return null;

        for (int i = 0; i < extraEnemyOptions.Length; i++)
        {
            EnemySpawnOption option = extraEnemyOptions[i];
            if (option == null || option.prefab == null || elapsed < option.unlockTime) continue;

            float weight = GetRampedWeight(option.unlockTime, option.maxWeight);
            if (roll < weight) return option.prefab;
            roll -= weight;
        }

        return null;
    }

    float GetRampedWeight(float unlockTime, float maxWeight)
    {
        if (enemyWeightRampDuration <= 0f) return maxWeight;

        float ramp = Mathf.Clamp01((elapsed - unlockTime) / enemyWeightRampDuration);
        return maxWeight * ramp;
    }

    void ApplyDifficulty(GameObject enemy, bool isBoss)
    {
        if (enemy == null) return;

        float minutes = elapsed / 60f;
        float healthMultiplier = 1f + minutes * healthScalePerMinute;
        float damageMultiplier = 1f + minutes * damageScalePerMinute;
        float speedMultiplier = 1f + minutes * speedScalePerMinute;

        if (isBoss)
        {
            healthMultiplier *= bossHealthMultiplier;
            damageMultiplier *= bossDamageMultiplier;
            speedMultiplier *= bossSpeedMultiplier;
        }

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null) health.ApplyHealthMultiplier(healthMultiplier);

        EnemyDamageOnContact contactDamage = enemy.GetComponent<EnemyDamageOnContact>();
        if (contactDamage != null) contactDamage.ApplyDamageMultiplier(damageMultiplier);

        EnemyChase chase = enemy.GetComponent<EnemyChase>();
        if (chase != null) chase.ApplySpeedMultiplier(speedMultiplier);

        EnemyProjectileShooter shooter = enemy.GetComponent<EnemyProjectileShooter>();
        if (shooter != null) shooter.ApplyDamageMultiplier(damageMultiplier);
    }

    int GetCurrentEnemiesPerWave()
    {
        if (enemyIncreaseInterval <= 0f || enemiesPerWaveIncrease <= 0)
            return enemiesPerWave;

        int increases = Mathf.FloorToInt(elapsed / enemyIncreaseInterval);
        int scaledWaveSize = enemiesPerWave + increases * enemiesPerWaveIncrease;
        return Mathf.Min(scaledWaveSize, maxEnemiesPerWave);
    }

    float GetCurrentWaveInterval()
    {
        float minutes = elapsed / 60f;
        return Mathf.Max(minWaveInterval, waveInterval - minutes * waveIntervalDecreasePerMinute);
    }
}
