using UnityEngine;

public class WeaponLevelUpOption : UpgradeOption
{
    private readonly WeaponDataSO weaponData;
    private readonly int currentLevel;

    public WeaponLevelUpOption(WeaponDataSO weaponData, int currentLevel)
    {
        this.weaponData = weaponData;
        this.currentLevel = currentLevel;
    }

    public override string Title
    {
        get
        {
            int nextLevel = currentLevel + 1;
            return $"{weaponData.weaponName} Lv {nextLevel}";
        }
    }

    public override string Description
    {
        get
        {
            int upgradeIndex = currentLevel;
            if (upgradeIndex < 0) return string.Empty;
            if (upgradeIndex >= weaponData.levels.Length) return string.Empty;

            return weaponData.levels[upgradeIndex].upgradeDescription;
        }
    }

    public override Sprite Icon
    {
        get { return weaponData.icon; }
    }

    public override void Apply(WeaponManager weaponManager)
    {
        weaponManager.LevelUpWeapon(weaponData);
    }
}
