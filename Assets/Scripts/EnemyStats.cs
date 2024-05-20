using System;
using System.Collections;
using UnityEngine;

public class EnemyStats : Stats
{
    public override void Death()
    {
        onDeath?.Invoke();

        Delegate[] delegateArray = onDeath.GetInvocationList();
        Debug.Log("List of delegates is length "+delegateArray.Length);
        foreach (Delegate d in delegateArray)
            onDeath -= (Action)d;
        
        gameObject.SetActive(false);
    }
}