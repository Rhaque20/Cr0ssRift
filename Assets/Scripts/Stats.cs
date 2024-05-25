using System;
using System.Collections;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]protected int _maxHP = 100, _maxArmor = 50;

    [Range(0,20)]
    [SerializeField]protected int _defense = 0;
    [SerializeField]protected EnumLib.Element _attribute;

    protected GlobalVariables _allVariables;

    protected int _currentHP = 0, _currentArmor = 0;

    protected float _damageModifier = 1,_armorDamageModifier = 1;

    protected Coroutine _regenerateArmor = null;

    protected bool _isDead = false;

    public Action onDeath;

    public int currentHP
    {
        get { return _currentHP; }
    }

    public int maxHP
    {
        get { return _maxHP; }
    }

    public int currentArmor
    {
        get { return _currentArmor; }
    }
    
    public int maxArmor
    {
        get { return _maxArmor; }
    }

    public GlobalVariables allVariables
    {
        get { return _allVariables; }
    }

    public float damageModifier
    {
        get { return _damageModifier; }
    }

    public float armorDamageModifier
    {
        get { return _armorDamageModifier; }
    }

    public bool isDead
    {
        get { return _isDead; }
    }

    public void Start()
    {
        _currentArmor = _maxArmor;
        _currentHP = _maxHP;
        _allVariables = GetComponent<GlobalVariables>();
    }


    protected virtual IEnumerator RegenerateArmor()
    {
        yield return new WaitForSeconds(5f);
        FullRestoreArmor();
        _regenerateArmor = null;
        float armorRatio = (_maxArmor != 0 ? (float)_currentArmor/_maxArmor : 0);
        _allVariables.onArmorUpdate?.Invoke(armorRatio);
    }

    public virtual void FullRestoreArmor()
    {
        _currentArmor = _maxArmor;
        _allVariables.onArmorBreak?.Invoke(true);
    }

    public virtual void RecoverHealth(int healAmount)
    {
        if(_currentHP != 0)
        {
            _currentHP = Mathf.Clamp(_currentHP + healAmount,0,_maxHP);
            _allVariables.onHealthUpdate?.Invoke(_currentHP/_maxHP);
        }
    }

    public virtual void RecoverArmor(int armorRecover)
    {
        if (_currentArmor != 0)
        {
            _currentArmor = Mathf.Clamp(_currentArmor + armorRecover,0,_maxArmor);
            _allVariables.onArmorUpdate?.Invoke((float)_currentArmor/(float)_maxArmor);
        } 
    }

    public virtual void DamageProcess(Skill skillReceived, Stats _attackerStats)
    {
        DealDamage(skillReceived.damage,skillReceived.attribute, _attackerStats);

        if (!_isDead)
        {
            DealArmorDamage(skillReceived.armorDamage,skillReceived.attribute);
            DealStatusDamage(skillReceived.statusDamage,(EnumLib.Status)skillReceived.attribute);
        }
        
    }

    public virtual void DealStatusDamage(int statusDamage,EnumLib.Status status)
    {

    }

    public virtual void CounterArmorDamage(int armorDMG)
    {
        
        DealArmorDamage(armorDMG,EnumLib.Element.Physical);

        if(_currentArmor <= 0)
        {
            _allVariables.onCountered?.Invoke();
        }
    }

    public virtual void DealArmorDamage(int armorDamage,EnumLib.Element attribute)
    {
        if (_maxArmor != 0 && (ElementModifier(attribute) >= 2.0f || attribute == EnumLib.Element.Physical))
        {
            _currentArmor = Mathf.Clamp(_currentArmor - armorDamage, 0,_maxArmor);

            float armorRatio = (_maxArmor != 0 ? (float)_currentArmor/_maxArmor : 0);
            _allVariables.onArmorUpdate?.Invoke(armorRatio);

            if (_currentArmor <= 0 && _regenerateArmor == null)
            {
                _allVariables.onArmorBreak?.Invoke(false);
                _regenerateArmor = StartCoroutine(RegenerateArmor());
            }
        }

    }

    public double ElementModifier(EnumLib.Element attribute)
    {
        if (_attribute == EnumLib.Element.Physical)
        {
            return 1.0;
        }
        else if (_attribute == attribute)
        {
            return 0.5;
        }

        float modifier = 1;
        
        switch(attribute)
        {
            case EnumLib.Element.Fire:
            if (_attribute == EnumLib.Element.Ice)
                modifier = 2;
            break;
            case EnumLib.Element.Ice:
            if(_attribute == EnumLib.Element.Fire)
                modifier = 2;
            break;
        }

        if (modifier >= 2)
        {
            Debug.Log("EFFECTIVE!");
        }

        return modifier;
    }

    // Later on remove instance of global and replace with enemy
    public virtual void Death()
    {
        onDeath?.Invoke();

        LevelManager.instance.DecreaseTally();

        Delegate[] delegateArray = onDeath.GetInvocationList();
        Debug.Log("List of delegates is length "+delegateArray.Length);
        foreach (Delegate d in delegateArray)
            onDeath -= (Action)d;
        
        gameObject.SetActive(false);
    }

    public virtual void DealDamage(int damage, EnumLib.Element attribute, Stats _attackerStats)
    {
        double damageCalc = damage * (_currentArmor > 0 ? 1 - (_defense * 0.04) : 1) * ElementModifier(attribute);

        int finalDamage = (int)Mathf.Round((float)damageCalc);

        _currentHP = Mathf.Clamp(_currentHP - finalDamage,0,_maxHP);

        _allVariables.onHealthUpdate?.Invoke((float)_currentHP/_maxHP);

        if (_currentHP == 0)
        {
            _isDead = true;
            Debug.Log("Defeated "+this.name);
            Death();
        }

    }
}