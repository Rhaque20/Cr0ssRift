using System;
using UnityEngine;

public class EnemyVariables: GlobalVariables
{
    public Action<bool> readyToAttack;
    void Start()
    {
        Debug.Log("Setting up ondeath function");
        _defenseCore = GetComponent<EnemyDefenseCore>();
        onDeath += LevelManager.instance.DecreaseTally;
    }
}