using UnityEngine;
using System.Collections;

public class PlayerStats : Stats,ISwitchCharacter
{
    // Once you add the party manager, set the statustracker through party manager
    [SerializeField]protected PlayerStatusTracker _playerStatusTracker;
    [SerializeField]protected EnumLib.Element _familiarElement;
    [SerializeField]protected float _currentSP = 100;

    [SerializeField]protected PlayerVariables _playerVariables;

    [SerializeField]protected bool _hasFamiliarSummoned = false;

    protected bool _mercyFramesActive = false;

    protected Coroutine _mercyTimer = null;


    public float currentSP
    {
        get { return _currentSP; }
    }

    public bool hasFamiliarSummoned
    {
        get{ return _hasFamiliarSummoned;}
    }

    public EnumLib.Element familiarElement
    {
        get { return _familiarElement;}
    }

    public PlayerVariables playerVariables
    {
        get { return _playerVariables; }
    }


    public override void DamageProcess(Skill skillReceived, Stats _attackerStats)
    {
        if(!_mercyFramesActive)
            base.DamageProcess(skillReceived, _attackerStats);
    }


    public override void DealArmorDamage(int armorDamage,EnumLib.Element attribute)
    {
        if (_maxArmor != 0 && (ElementModifier(attribute) >= 2.0f || attribute == EnumLib.Element.Physical))
        {
            _currentArmor = Mathf.Clamp(_currentArmor - armorDamage, 0,_maxArmor);
            if (_currentArmor <= 0 && _regenerateArmor == null)
            {
                _allVariables.onArmorBreak?.Invoke(false);
            }
        }

        PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);

    }

    void Start()
    {
        base.Start();

        _playerVariables = _allVariables as PlayerVariables;
    }

    // public override void FullRestoreArmor()
    // {
    //     _currentArmor = _maxArmor;
    //     _allVariables.onArmorBreak?.Invoke(true);
        
    //     PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);
    // }

    public void SetElement(bool _isSummoned)
    {
        _attribute = (_isSummoned ? EnumLib.Element.Physical : _familiarElement);
        _hasFamiliarSummoned = _isSummoned;
    }

    public override void RecoverHealth(int healAmount)
    {
        base.RecoverHealth(healAmount);
        if(!_isDead)
        {
            if(gameObject.activeSelf)
                PlayerUIManager.instance.SetHealthBar(_currentHP,_maxHP);
            else
                PlayerUIManager.instance.SetHealthBar(_currentHP,_maxHP,transform.GetSiblingIndex());
            
        }
    }

    private IEnumerator MercyTimer()
    {
        _mercyFramesActive = true;

        if (_currentArmor == 0)
        {
            Physics.IgnoreLayerCollision(6,7,true);
            yield return new WaitForSeconds(2f + _playerVariables.playerStaggerSystem.staggerDuration);
        }
        else
            yield return new WaitForSeconds(_playerVariables.playerStaggerSystem.staggerDuration * 0.5f);
        
        Physics.IgnoreLayerCollision(6,7,false);
        _mercyFramesActive = false;
        _mercyTimer = null;
    }

    public void RecoverSP(float spAmount)
    {

        _currentSP = Mathf.Clamp(_currentSP + spAmount,0,100);

        if (_currentSP <= 0f && _hasFamiliarSummoned)
        {
            _playerVariables.onSummonFamiliar?.Invoke(false);
            _playerVariables.onForcedUnSummon?.Invoke();
        }

        if(!_isDead)
        {
            if (gameObject.activeSelf)
                PlayerUIManager.instance.SetSPBar(_currentSP/100);
            else
                PlayerUIManager.instance.SetSPBar(_currentSP/100,transform.GetSiblingIndex());
        }
    }

    public override void RecoverArmor(int armorRecover)
    {
        base.RecoverArmor(armorRecover);
        if(_currentHP != 0)
        {
            if(gameObject.activeSelf)
                PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);
            else
                PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor,transform.GetSiblingIndex());
        } 
    }

    public override void DealStatusDamage(int statusDamage,EnumLib.Status status)
    {
        if (_playerStatusTracker != null)
        {
            _playerStatusTracker.ApplyBuildUp(status,statusDamage);
        }
    }

    public override void DealDamage(int damage, EnumLib.Element attribute, Stats _attackerStats)
    {
        double damageCalc = damage * (_currentArmor > 0 ? 1 - (_defense * 0.04) : 1) * ElementModifier(attribute);

        damageCalc *= _attackerStats.damageModifier;
        int finalDamage = (int)Mathf.Round((float)damageCalc);

        _currentHP = Mathf.Clamp(_currentHP - finalDamage,0,_maxHP);

        PlayerUIManager.instance.SetHealthBar(_currentHP,_maxHP);

        if (_currentHP == 0)
        {
            _isDead = true;
            Debug.Log("Defeated "+this.name);
            _playerVariables.onDeath?.Invoke();
        }
        else
        {
            if (_mercyTimer == null)
                _mercyTimer = StartCoroutine(MercyTimer());
        }

    }

    public void SwitchOut()
    {
        if (_mercyTimer != null)
        {
            StopCoroutine(_mercyTimer);
            _mercyFramesActive = false;
        }
    }

    public void SwitchIn()
    {
        
    }
}