using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : Stats
{
    [SerializeField] private Image _healthBar, _armorBar;

    void Start()
    {
        base.Start();
        _allVariables.onHealthUpdate += ChangeHealthRatio;
        _allVariables.onArmorUpdate += ChangeArmorRatio;
    }

    void ChangeHealthRatio(float ratio)
    {
        _healthBar.fillAmount = ratio;
    }

    void ChangeArmorRatio(float ratio)
    {
        _armorBar.fillAmount = ratio;
    }
}