using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public static PlayerXP Instance { get; private set; }

    [SerializeField] private int baseXPToNext = 5;
    [SerializeField] private int xpStepPerLevel = 3;
    [SerializeField] private Transform pickupTarget;

    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; }
    public int XPToNext { get; private set; }

    public Transform PickupTarget
    {
        get
        {
            if (pickupTarget != null) return pickupTarget;
            return transform;
        }
    }

    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLeveledUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        XPToNext = ComputeXPToNext(Level);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        OnXPChanged?.Invoke(CurrentXP, XPToNext);
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        CurrentXP += amount;

        while (CurrentXP >= XPToNext)
        {
            CurrentXP -= XPToNext;
            Level++;
            XPToNext = ComputeXPToNext(Level);
            OnLeveledUp?.Invoke(Level);
        }

        OnXPChanged?.Invoke(CurrentXP, XPToNext);
    }

    private int ComputeXPToNext(int level)
    {
        return baseXPToNext + xpStepPerLevel * (level - 1);
    }
}
