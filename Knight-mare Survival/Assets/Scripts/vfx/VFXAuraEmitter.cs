using UnityEngine;

public class VFXAuraEmitter : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform followTarget;
    public Vector2 offset;
    public float interval = 1f;
    public bool parentToTarget = true;

    [Header("Weapon Sync")]
    [SerializeField] private Weapon sourceWeapon;
    [SerializeField] private bool syncIntervalToWeaponCooldown = true;
    [SerializeField] private bool scaleWithWeaponArea = true;
    [SerializeField] private float weaponAreaAtVfxScaleOne = 1f;
    [SerializeField] private float visualScaleMultiplier = 1f;

    private float timer;

    private void Start()
    {
        if (followTarget == null) followTarget = transform;

        if (sourceWeapon == null)
        {
            sourceWeapon = GetComponent<Weapon>();
            if (sourceWeapon == null) sourceWeapon = GetComponentInParent<Weapon>();
        }

        RefreshIntervalFromWeapon();
    }

    private void Update()
    {
        RefreshIntervalFromWeapon();

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = Mathf.Max(0.01f, interval);
        }
    }

    private void Spawn()
    {
        if (vfxPrefab == null || VFXPool.Instance == null) return;

        Vector2 pos = (Vector2)followTarget.position + offset;
        Transform parent = parentToTarget ? followTarget : null;

        GameObject vfx = VFXPool.Instance.Get(vfxPrefab, pos, Quaternion.identity, parent);
        vfx.transform.localScale = vfxPrefab.transform.localScale * GetAreaScale();
    }

    private void RefreshIntervalFromWeapon()
    {
        if (!syncIntervalToWeaponCooldown) return;

        if (TryGetWeaponStats(out WeaponLevel stats))
            interval = Mathf.Max(0.01f, stats.cooldown);
    }

    private float GetAreaScale()
    {
        if (!scaleWithWeaponArea) return visualScaleMultiplier;
        if (!TryGetWeaponStats(out WeaponLevel stats)) return visualScaleMultiplier;

        float baseArea = Mathf.Max(0.0001f, weaponAreaAtVfxScaleOne);
        return Mathf.Max(0.01f, stats.area / baseArea) * visualScaleMultiplier;
    }

    private bool TryGetWeaponStats(out WeaponLevel stats)
    {
        stats = default;

        if (sourceWeapon == null) return false;
        if (sourceWeapon.Data == null || sourceWeapon.Data.levels == null) return false;

        int index = sourceWeapon.Level - 1;
        if (index < 0 || index >= sourceWeapon.Data.levels.Length) return false;

        stats = sourceWeapon.Data.levels[index];
        return true;
    }
}