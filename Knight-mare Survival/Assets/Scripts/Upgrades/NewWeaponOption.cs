using UnityEngine;

public class NewWeaponOption : UpgradeOption
{
    private readonly WeaponDataSO weaponData;

    public NewWeaponOption(WeaponDataSO weaponData)
    {
        this.weaponData = weaponData;
    }

    public override string Title
    {
        get { return $"{weaponData.weaponName} (NEW)"; }
    }

    public override string Description
    {
        get { return weaponData.description; }
    }

    public override Sprite Icon
    {
        get { return weaponData.icon; }
    }

    public override void Apply(WeaponManager weaponManager)
    {
        weaponManager.AddWeapon(weaponData);
    }
}
