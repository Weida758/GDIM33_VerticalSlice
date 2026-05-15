using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button selectButton;

    private UpgradeOption boundOption;
    private Action<UpgradeOption> onSelected;

    private void Awake()
    {
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(HandleClick);
        }
    }

    public void Bind(UpgradeOption option, Action<UpgradeOption> onSelected)
    {
        this.boundOption = option;
        this.onSelected = onSelected;

        if (iconImage != null)
        {
            iconImage.sprite = option.Icon;
            iconImage.enabled = (option.Icon != null);
        }

        if (titleText != null)
        {
            titleText.text = option.Title;
        }

        if (descriptionText != null)
        {
            descriptionText.text = option.Description;
        }
    }

    private void HandleClick()
    {
        if (boundOption == null) return;
        if (onSelected == null) return;

        onSelected(boundOption);
    }
}
