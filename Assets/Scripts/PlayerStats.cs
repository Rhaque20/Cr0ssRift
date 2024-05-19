using UnityEngine;
using System.Collections;

public class PlayerStats : Stats
{
    // Once you add the party manager, set the statustracker through party manager
    [SerializeField]protected PlayerStatusTracker _playerStatusTracker;

    protected override IEnumerator RegenerateArmor()
    {
        yield return new WaitForSeconds(5f);
        _currentArmor = _maxArmor;
        _allVariables.onArmorBreak?.Invoke(true);
        _regenerateArmor = null;
        PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);
    }
    public override void DealArmorDamage(int armorDamage,EnumLib.Element attribute)
    {
        base.DealArmorDamage(armorDamage,attribute);

        PlayerUIManager.instance.SetArmorBar((float)_currentArmor/(float)_maxArmor);

    }

    public override void DealStatusDamage(EnumLib.Status status,int statusDamage)
    {
        if (_playerStatusTracker != null)
        {
            _playerStatusTracker.ApplyBuildUp(status,statusDamage);
        }
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