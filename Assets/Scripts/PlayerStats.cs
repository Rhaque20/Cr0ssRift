using UnityEngine;

public class PlayerStats : Stats
{

    public override void DealArmorDamage(int armorDamage,EnumLib.Element attribute)
    {
        base.DealArmorDamage(armorDamage,attribute);

        PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);

    }
    public override void DealDamage(int damage, EnumLib.Element attribute)
    {
        double damageCalc = damage * (_currentArmor > 0 ? 1 - (_defense * 0.04) : 1) * ElementModifier(attribute);

        int finalDamage = (int)Mathf.Round((float)damageCalc);

        _currentHP = Mathf.Clamp(_currentHP - finalDamage,0,_maxHP);

        PlayerUIManager.instance.SetHealthBar(_currentHP,_maxHP);

        if (_currentHP == 0)
        {
            //gameObject.SetActive(false);
            Debug.Log("Defeated "+this.name);
        }

    }
}