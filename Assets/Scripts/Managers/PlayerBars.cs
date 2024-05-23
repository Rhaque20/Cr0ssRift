using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    [SerializeField]private Image _healthBar,_spBar,_armorBar, _healthDrainBar, _spDrainBar;
    [SerializeField]private TMP_Text _curHPText,_maxHPText;

    [SerializeField]private bool _isMiniBar = false;
    [SerializeField]private Image _coolDownDisplay;

    public void SetHealthBar(int _currentHP,int _maxHP)
    {
        float ratio = (float)_currentHP/(float)_maxHP;
        
        if(!_isMiniBar)
        {
            _curHPText.SetText(_currentHP.ToString());
            _maxHPText.SetText(_maxHP.ToString());
        }
        else
        {
            ratio *= 0.5f;
        }

        _healthBar.fillAmount = ratio;
        
    }

    public void SetSPBar(float ratio)
    {
        if (_isMiniBar)
            ratio *= 0.5f;
        _spBar.fillAmount = ratio;
    }

    public void SetSPDrainBar(float ratio)
    {
        if(_isMiniBar)
            ratio *= 0.5f;
        
        _spDrainBar.fillAmount = ratio;
    }

    public void SetArmorBar(float ratio)
    {
        _armorBar.fillAmount = ratio;
    }

    public void SetCoolDown(float ratio)
    {
        _coolDownDisplay.fillAmount = ratio;
    }
}