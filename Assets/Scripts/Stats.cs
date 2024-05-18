using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]protected int _maxHP = 100, _maxArmor = 50;

    [Range(0,20)]
    [SerializeField]protected int _defense = 0;
    [SerializeField]protected EnumLib.Element _attribute;

    protected int _currentHP = 0, _currentArmor = 0;

    protected void Start()
    {
        _currentArmor = _maxArmor;
        _currentHP = _maxHP;
    }

    public void DealArmorDamage(int armorDamage)
    {
        _currentArmor = Mathf.Clamp(_currentArmor - armorDamage, 0,_maxArmor);
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

    public void DealDamage(int damage, EnumLib.Element attribute)
    {
        double damageCalc = damage * (_currentArmor > 0 ? 1 - (_defense * 0.04) : 1) * ElementModifier(attribute);

        int finalDamage = (int)Mathf.Round((float)damageCalc);

        _currentHP = Mathf.Clamp(_currentHP - finalDamage,0,_maxHP);

        if (_currentHP == 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Defeated "+this.name);
        }

    }
}