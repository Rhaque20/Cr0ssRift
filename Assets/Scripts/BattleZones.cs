using System.Collections.Generic;
using UnityEngine;
public class BattleZones : MonoBehaviour
{
    [SerializeField]List<BoxCollider> _arenaZones = new List<BoxCollider>();

    public void LockDown()
    {
        foreach(BoxCollider b in _arenaZones)
        {
            b.isTrigger = false;
        }
    }

    public void Release()
    {
        foreach(BoxCollider b in _arenaZones)
        {
            b.isTrigger = true;
        }
    }
}