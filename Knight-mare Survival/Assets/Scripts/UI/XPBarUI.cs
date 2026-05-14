using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;

    private PlayerXP source;

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
    }

    private void TryBind()
    {
        if (source != null) return;

        source = PlayerXP.Instance;
        if (source == null) return;

        source.OnXPChanged += HandleXPChanged;
        source.OnLeveledUp += HandleLeveledUp;

        HandleXPChanged(source.CurrentXP, source.XPToNext);
        HandleLeveledUp(source.Level);
    }

    private void Unbind()
    {
        if (source == null) return;

        source.OnXPChanged -= HandleXPChanged;
        source.OnLeveledUp -= HandleLeveledUp;
        source = null;
    }

    private void HandleXPChanged(int currentXP, int xpToNext)
    {
        if (xpSlider == null) return;

        xpSlider.minValue = 0f;
        xpSlider.maxValue = xpToNext;
        xpSlider.value = currentXP;
    }

    private void HandleLeveledUp(int newLevel)
    {
        if (levelText == null) return;

        levelText.text = $"Lv {newLevel}";
    }
}
