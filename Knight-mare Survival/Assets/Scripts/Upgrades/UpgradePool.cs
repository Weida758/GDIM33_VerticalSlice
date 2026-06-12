using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UpgradePool
{
    [SerializeField] private List<WeaponDataSO> availableWeapons = new List<WeaponDataSO>();
    [SerializeField] private List<PlayerStatUpgradeDefinition> availableStatUpgrades = new List<PlayerStatUpgradeDefinition>();

    private readonly List<UpgradeOption> workingCandidates = new List<UpgradeOption>();
    private readonly List<PlayerStatUpgradeDefinition> workingStatCandidates = new List<PlayerStatUpgradeDefinition>();

    private static readonly PlayerStatUpgradeDefinition[] DefaultStatUpgrades =
    {
        new PlayerStatUpgradeDefinition(PlayerStatUpgradeType.MaxHealth, "Max Health", "+20 max health.", 20f),
        new PlayerStatUpgradeDefinition(PlayerStatUpgradeType.MoveSpeed, "Move Speed", "+0.4 movement speed.", 0.4f),
        new PlayerStatUpgradeDefinition(PlayerStatUpgradeType.InvincibilityDuration, "Recovery", "+0.1 seconds of damage invincibility.", 0.1f),
        new PlayerStatUpgradeDefinition(PlayerStatUpgradeType.GlobalDamage, "Damage Up", "+0.5 global weapon damage.", 0.5f)
    };

    public List<UpgradeOption> BuildOffers(WeaponManager weaponManager, int desiredCount)
    {
        workingCandidates.Clear();

        foreach (WeaponDataSO weaponData in availableWeapons)
        {
            if (weaponData == null) continue;

            bool alreadyOwned = weaponManager.Has(weaponData);

            if (!alreadyOwned)
            {
                if (weaponManager.HasFreeSlot)
                {
                    workingCandidates.Add(new NewWeaponOption(weaponData));
                }
                continue;
            }

            Weapon weapon = weaponManager.Get(weaponData);
            if (weapon != null && weapon.CanLevelUp)
            {
                workingCandidates.Add(new WeaponLevelUpOption(weaponData, weapon.Level));
            }
        }

        ShuffleCandidates();

        int finalCount = Mathf.Min(desiredCount, workingCandidates.Count);
        List<UpgradeOption> chosen = new List<UpgradeOption>(finalCount);
        for (int i = 0; i < finalCount; i++)
        {
            chosen.Add(workingCandidates[i]);
        }

        FillWithStatUpgrades(chosen, desiredCount);
        return chosen;
    }

    private void FillWithStatUpgrades(List<UpgradeOption> chosen, int desiredCount)
    {
        if (chosen.Count >= desiredCount) return;

        BuildStatCandidates();
        if (workingStatCandidates.Count == 0) return;

        ShuffleStatCandidates();

        int needed = desiredCount - chosen.Count;
        int statCount = Mathf.Min(needed, workingStatCandidates.Count);
        for (int i = 0; i < statCount; i++)
        {
            chosen.Add(new PlayerStatUpgradeOption(workingStatCandidates[i]));
        }
    }

    private void BuildStatCandidates()
    {
        workingStatCandidates.Clear();

        if (availableStatUpgrades != null)
        {
            for (int i = 0; i < availableStatUpgrades.Count; i++)
            {
                PlayerStatUpgradeDefinition upgrade = availableStatUpgrades[i];
                if (upgrade == null || !upgrade.enabled) continue;
                workingStatCandidates.Add(upgrade);
            }
        }

        if (workingStatCandidates.Count > 0)
        {
            EnsureDefaultStatUpgrade(PlayerStatUpgradeType.GlobalDamage);
            return;
        }

        for (int i = 0; i < DefaultStatUpgrades.Length; i++)
        {
            workingStatCandidates.Add(DefaultStatUpgrades[i]);
        }
    }

    private void EnsureDefaultStatUpgrade(PlayerStatUpgradeType type)
    {
        for (int i = 0; i < workingStatCandidates.Count; i++)
        {
            if (workingStatCandidates[i].type == type) return;
        }

        for (int i = 0; i < DefaultStatUpgrades.Length; i++)
        {
            if (DefaultStatUpgrades[i].type == type)
            {
                workingStatCandidates.Add(DefaultStatUpgrades[i]);
                return;
            }
        }
    }

    private void ShuffleCandidates()
    {
        for (int i = workingCandidates.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            UpgradeOption temp = workingCandidates[i];
            workingCandidates[i] = workingCandidates[swapIndex];
            workingCandidates[swapIndex] = temp;
        }
    }

    private void ShuffleStatCandidates()
    {
        for (int i = workingStatCandidates.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            PlayerStatUpgradeDefinition temp = workingStatCandidates[i];
            workingStatCandidates[i] = workingStatCandidates[swapIndex];
            workingStatCandidates[swapIndex] = temp;
        }
    }
}
