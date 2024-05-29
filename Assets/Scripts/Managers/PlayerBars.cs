using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    const int FROZEN = 0, PARALYZE = 1, DEAD = 2, DEFAULT = 3;
    [SerializeField]private Image _healthBar,_spBar,_armorBar, _healthDrainBar, _spDrainBar;
    [SerializeField]private TMP_Text _curHPText,_maxHPText;

    [SerializeField]private bool _isMiniBar = false;
    [SerializeField]private Image _coolDownDisplay;
    [SerializeField]private Color[] _statusColors = new Color[3];


    public void SetHealthBar(int _currentHP,int _maxHP)
    {
        float ratio = (float)_currentHP/_maxHP;
        
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

    public void SetCoolDownColor(EnumLib.Status status)
    {
        if(status == EnumLib.Status.Dead)
        {
            _coolDownDisplay.color = _statusColors[DEAD];
        }
        else if (status == EnumLib.Status.Frozen)
        {
            _coolDownDisplay.color = _statusColors[FROZEN];
        }
        else
        {
            _coolDownDisplay.color = _statusColors[PARALYZE];
        }
            
    }

    public void SetCoolDownColorDefault()
    {
        _coolDownDisplay.color = _statusColors[DEFAULT];
    }
}