using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private int maxWeaponSlots = 6;
    [SerializeField] private List<WeaponDataSO> startingWeapons = new();

    private readonly Dictionary<WeaponDataSO, Weapon> equipped = new();

    public event Action<Weapon> OnWeaponAdded;
    public event Action<Weapon> OnWeaponLeveledUp;

    public int SlotsUsed
    {
        get { return equipped.Count; }
    }

    public int SlotsMax
    {
        get { return maxWeaponSlots; }
    }

    public bool HasFreeSlot
    {
        get { return SlotsUsed < SlotsMax; }
    }

    public IEnumerable<Weapon> Equipped
    {
        get { return equipped.Values; }
    }

    private void Start()
    {
        foreach (WeaponDataSO data in startingWeapons)
        {
            if (data != null)
            {
                AddWeapon(data);
            }
        }
    }

    public bool Has(WeaponDataSO data)
    {
        if (data == null) return false;
        return equipped.ContainsKey(data);
    }

    public Weapon Get(WeaponDataSO data)
    {
        Weapon weapon;
        equipped.TryGetValue(data, out weapon);
        return weapon;
    }

    public bool CanAdd(WeaponDataSO data)
    {
        if (data == null) return false;
        if (Has(data)) return false;
        return HasFreeSlot;
    }

    public bool CanLevelUp(WeaponDataSO data)
    {
        if (!Has(data)) return false;
        return equipped[data].CanLevelUp;
    }

    public bool AddWeapon(WeaponDataSO data)
    {
        if (!CanAdd(data)) return false;
        if (data.weaponPrefab == null)
        {
            Debug.LogError($"WeaponDataSO '{data.weaponName}' has no weaponPrefab assigned.");
            return false;
        }

        GameObject weaponObject = Instantiate(data.weaponPrefab, transform, false);
        weaponObject.transform.localPosition = Vector3.zero;

        Weapon weapon = weaponObject.GetComponent<Weapon>();
        if (weapon == null)
        {
            Debug.LogError($"weaponPrefab for '{data.weaponName}' has no Weapon component.");
            Destroy(weaponObject);
            return false;
        }

        weapon.Initialize(data);
        equipped[data] = weapon;
        OnWeaponAdded?.Invoke(weapon);
        return true;
    }

    public bool LevelUpWeapon(WeaponDataSO data)
    {
        Weapon weapon;
        if (!equipped.TryGetValue(data, out weapon)) return false;
        if (!weapon.LevelUp()) return false;
        OnWeaponLeveledUp?.Invoke(weapon);
        return true;
    }
}
