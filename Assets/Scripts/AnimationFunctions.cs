using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    [SerializeField] protected CombatCore _combatCore;
    public void HitScan()
    {
        _combatCore.HitScan();
    }

    public void Recover()
    {
        _combatCore.Recover();
    }
}