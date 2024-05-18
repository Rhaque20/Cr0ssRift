using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    [SerializeField]private Image _healthBar,_spBar,_armorBar;
    [SerializeField]private TMP_Text _curHPText,_maxHPText;

    public void SetHealthBar(int _currentHP,int _maxHP)
    {
        _healthBar.fillAmount = (float)_currentHP/(float)_maxHP;
        
        _curHPText.SetText(_currentHP.ToString());
        _maxHPText.SetText(_maxHP.ToString());
    }

    public void SetSPBar(float ratio)
    {
        _spBar.fillAmount = ratio;
    }

    public void SetArmorBar(float ratio)
    {
        _armorBar.fillAmount = ratio;
    }
}