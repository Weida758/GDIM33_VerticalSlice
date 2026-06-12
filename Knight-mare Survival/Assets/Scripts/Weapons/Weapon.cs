using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponDataSO Data { get; private set; }
    public int Level { get; private set; } = 1;

    public event Action<Weapon> OnLevelChanged;

    private WeaponManager weaponManager;

    protected WeaponLevel Stats
    {
        get { return Data.levels[Level - 1]; }
    }

    protected float Damage
    {
        get
        {
            float bonus = weaponManager != null ? weaponManager.GlobalDamageBonus : 0f;
            return Mathf.Max(0f, Stats.damage + bonus);
        }
    }

    public bool CanLevelUp
    {
        get
        {
            if (Data == null) return false;
            return Level < Data.MaxLevel;
        }
    }

    public void Initialize(WeaponDataSO data)
    {
        Data = data;
        Level = 1;
        weaponManager = GetComponentInParent<WeaponManager>();
        OnInitialized();
        OnLevelChanged?.Invoke(this);
    }

    public bool LevelUp()
    {
        if (!CanLevelUp) return false;
        Level++;
        OnUpgraded();
        OnLevelChanged?.Invoke(this);
        return true;
    }

    protected virtual void OnInitialized() { }
    protected virtual void OnUpgraded() { }
}
