using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UpgradePool
{
    [SerializeField] private List<WeaponDataSO> availableWeapons = new List<WeaponDataSO>();
    private readonly List<UpgradeOption> workingCandidates = new List<UpgradeOption>();

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
        return chosen;
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
}
