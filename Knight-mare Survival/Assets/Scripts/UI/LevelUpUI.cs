using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private UpgradeCardUI[] cards;
    [SerializeField] private UpgradePool pool;
    [SerializeField] private int offerCount = 3;

    private PlayerXP source;
    private readonly Queue<int> pendingLevelUps = new Queue<int>();
    private bool isPanelOpen;

    private void OnEnable()
    {
        TryBind();
    }

    private void Start()
    {
        TryBind();
    }

    private void OnDisable()
    {
        Unbind();
        
        if (isPanelOpen)
        {
            Time.timeScale = 1f;
            isPanelOpen = false;
        }
    }

    private void TryBind()
    {
        if (source != null) return;

        source = PlayerXP.Instance;
        if (source == null) return;

        source.OnLeveledUp += HandleLeveledUp;
    }

    private void Unbind()
    {
        if (source == null) return;

        source.OnLeveledUp -= HandleLeveledUp;
        source = null;
    }

    private void HandleLeveledUp(int newLevel)
    {
        pendingLevelUps.Enqueue(newLevel);

        if (!isPanelOpen)
        {
            ShowNextOffer();
        }
    }

    private void ShowNextOffer()
    {
        while (pendingLevelUps.Count > 0)
        {
            pendingLevelUps.Dequeue();
            List<UpgradeOption> offers = pool.BuildOffers(weaponManager, offerCount);

            if (offers.Count > 0)
            {
                OpenPanel(offers);
                return;
            }
        }

        ClosePanel();
    }

    private void OpenPanel(List<UpgradeOption> offers)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }

        isPanelOpen = true;
        Time.timeScale = 0f;

        for (int i = 0; i < cards.Length; i++)
        {
            UpgradeCardUI card = cards[i];
            if (card == null) continue;

            if (i < offers.Count)
            {
                card.gameObject.SetActive(true);
                card.Bind(offers[i], HandleOptionSelected);
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }
    }

    private void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

        isPanelOpen = false;
        Time.timeScale = 1f;
    }

    private void HandleOptionSelected(UpgradeOption option)
    {
        option.Apply(weaponManager);
        ShowNextOffer();
    }
}
