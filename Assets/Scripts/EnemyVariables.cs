using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyVariables: GlobalVariables
{
    void Start()
    {
        Debug.Log("Setting up ondeath function");
        _defenseCore = GetComponent<EnemyDefenseCore>();
        onDeath += LevelManager.instance.DecreaseTally;
    }
}