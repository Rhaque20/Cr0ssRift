using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    [SerializeField] protected CombatCore _combatCore;
    [SerializeField] protected Forces _forces;
    public void HitScan()
    {
        _combatCore.HitScan();
    }

    public void Recover()
    {
        _combatCore.Recover();
    }

    public void Charge(float power)
    {
        _forces.Charge(power);
    }
}