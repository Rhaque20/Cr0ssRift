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

    protected Coroutine _regenerateArmor = null;

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

    public void Start()
    {
        _currentArmor = _maxArmor;
        _currentHP = _maxHP;
        _allVariables = GetComponent<GlobalVariables>();
    }


    protected virtual IEnumerator RegenerateArmor()
    {
        yield return new WaitForSeconds(5f);
        _currentArmor = _maxArmor;
        _allVariables.onArmorBreak?.Invoke(true);
        _regenerateArmor = null;
    }

    public void RecoverHealth(int healAmount)
    {
        _currentHP = Mathf.Clamp(_currentHP + healAmount,0,_maxHP);
    }

    public void RecoverArmor(int armorRecover)
    {
        _currentArmor = Mathf.Clamp(_currentArmor + armorRecover,0,_maxArmor);
    }

    public void DamageProcess(Skill skillReceived)
    {
        DealDamage(skillReceived.damage,skillReceived.attribute);
        DealArmorDamage(skillReceived.armorDamage,skillReceived.attribute);
        DealStatusDamage(skillReceived.statusDamage,(EnumLib.Status)skillReceived.attribute);
    }

    public virtual void DealStatusDamage(int statusDamage,EnumLib.Status status)
    {

    }

    public virtual void DealArmorDamage(int armorDamage,EnumLib.Element attribute)
    {
        if (_maxArmor != 0 && (ElementModifier(attribute) >= 2.0f || attribute == EnumLib.Element.Physical))
        {
            _currentArmor = Mathf.Clamp(_currentArmor - armorDamage, 0,_maxArmor);
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

        return modifier;
    }

    public virtual void Death()
    {
        
    }

    public virtual void DealDamage(int damage, EnumLib.Element attribute)
    {
        double damageCalc = damage * (_currentArmor > 0 ? 1 - (_defense * 0.04) : 1) * ElementModifier(attribute);

        int finalDamage = (int)Mathf.Round((float)damageCalc);

        _currentHP = Mathf.Clamp(_currentHP - finalDamage,0,_maxHP);

        _allVariables.onHealthUpdate?.Invoke(_currentHP/_maxHP);

        if (_currentHP == 0)
        {
            Debug.Log("Defeated "+this.name);
            Death();
        }

    }
}