using System.Collections.Generic;
using UnityEngine;
public class BattleZones : MonoBehaviour
{
    [SerializeField]bool _isFinalZone = false, _isChallengeZone = false;
    [SerializeField]List<BoxCollider> _arenaZones = new List<BoxCollider>();

    public bool isFinalZone
    {
        get { return _isFinalZone;}
    }

    public bool isChallengeZone
    {
        get { return _isChallengeZone;}
    }

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