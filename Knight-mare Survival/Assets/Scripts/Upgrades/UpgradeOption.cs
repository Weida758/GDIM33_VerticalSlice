using UnityEngine;

public abstract class UpgradeOption
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract Sprite Icon { get; }

    public abstract void Apply(WeaponManager weaponManager);
}
