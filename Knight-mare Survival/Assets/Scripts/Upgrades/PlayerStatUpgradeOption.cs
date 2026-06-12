using UnityEngine;

public enum PlayerStatUpgradeType
{
    MaxHealth,
    MoveSpeed,
    InvincibilityDuration,
    GlobalDamage
}

[System.Serializable]
public class PlayerStatUpgradeDefinition
{
    public PlayerStatUpgradeType type = PlayerStatUpgradeType.MaxHealth;
    public string title = "Max Health";
    [TextArea] public string description = "+20 max health.";
    public Sprite icon;
    public float amount = 20f;
    public bool enabled = true;

    public PlayerStatUpgradeDefinition() { }

    public PlayerStatUpgradeDefinition(PlayerStatUpgradeType type, string title, string description, float amount)
    {
        this.type = type;
        this.title = title;
        this.description = description;
        this.amount = amount;
        enabled = true;
    }
}

public class PlayerStatUpgradeOption : UpgradeOption
{
    private readonly PlayerStatUpgradeDefinition definition;

    public PlayerStatUpgradeOption(PlayerStatUpgradeDefinition definition)
    {
        this.definition = definition;
    }

    public override string Title
    {
        get { return definition.title; }
    }

    public override string Description
    {
        get { return definition.description; }
    }

    public override Sprite Icon
    {
        get { return definition.icon; }
    }

    public override void Apply(WeaponManager weaponManager)
    {
        switch (definition.type)
        {
            case PlayerStatUpgradeType.MaxHealth:
                ApplyMaxHealth(weaponManager);
                break;
            case PlayerStatUpgradeType.MoveSpeed:
                ApplyMoveSpeed(weaponManager);
                break;
            case PlayerStatUpgradeType.InvincibilityDuration:
                ApplyInvincibilityDuration(weaponManager);
                break;
            case PlayerStatUpgradeType.GlobalDamage:
                ApplyGlobalDamage(weaponManager);
                break;
        }
    }

    private void ApplyMaxHealth(WeaponManager weaponManager)
    {
        PlayerHealth health = ResolvePlayerComponent<PlayerHealth>(weaponManager);
        if (health == null)
        {
            Debug.LogWarning("Could not apply max health upgrade because PlayerHealth was not found.");
            return;
        }

        health.AddMaxHealth(Mathf.RoundToInt(definition.amount), true);
    }

    private void ApplyMoveSpeed(WeaponManager weaponManager)
    {
        Player player = ResolvePlayerComponent<Player>(weaponManager);
        if (player == null)
        {
            Debug.LogWarning("Could not apply move speed upgrade because Player was not found.");
            return;
        }

        player.AddMoveSpeed(definition.amount);
    }

    private void ApplyInvincibilityDuration(WeaponManager weaponManager)
    {
        PlayerHealth health = ResolvePlayerComponent<PlayerHealth>(weaponManager);
        if (health == null)
        {
            Debug.LogWarning("Could not apply invincibility upgrade because PlayerHealth was not found.");
            return;
        }

        health.AddInvincibilityDuration(definition.amount);
    }

    private void ApplyGlobalDamage(WeaponManager weaponManager)
    {
        if (weaponManager == null)
        {
            weaponManager = ResolvePlayerComponent<WeaponManager>(null);
        }

        if (weaponManager == null)
        {
            Debug.LogWarning("Could not apply global damage upgrade because WeaponManager was not found.");
            return;
        }

        weaponManager.AddGlobalDamageBonus(definition.amount);
    }

    private static T ResolvePlayerComponent<T>(WeaponManager weaponManager) where T : Component
    {
        if (weaponManager != null)
        {
            T component = weaponManager.GetComponent<T>();
            if (component != null) return component;

            component = weaponManager.GetComponentInParent<T>();
            if (component != null) return component;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return null;

        return playerObject.GetComponent<T>();
    }
}
