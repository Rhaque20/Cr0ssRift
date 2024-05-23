using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBarriers : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]bool _triggerDirectionPos = true;
    [SerializeField]int index = 1;

    BoxCollider _collisionBox;
    bool _activated = false;
    void Start()
    {
        _collisionBox = GetComponent<BoxCollider>();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x &&  _triggerDirectionPos)
            {
                LevelManager.instance.SpawnWave(index);
                _collisionBox.isTrigger = false;
            }
        }
    }
}
