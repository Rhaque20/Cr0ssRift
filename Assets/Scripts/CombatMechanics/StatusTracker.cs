using System;
using System.Collections;
using UnityEngine;

public class StatusTracker : MonoBehaviour
{
    [Serializable]
    public struct StatusAilment
    {
        public EnumLib.Status statusType;
        public int statusBuildUp;
        public float duration;
        public bool triggered;
    };
    
    [Serializable]
    public struct StatusBuildUp
    {
        public EnumLib.Status statusType;
        public int maxBuildUp;
        public int thresholdIncrease;
        public int maxIncreases;
        public int currentIncreases;
    };
    [SerializeField]protected StatusAilment[] _activeStatuses;
    [SerializeField]protected StatusBuildUp[] _statusBuildUps = new StatusBuildUp[Enum.GetNames(typeof(EnumLib.Status)).Length];

    protected Coroutine[] _statusTimers = new Coroutine[Enum.GetNames(typeof(EnumLib.Status)).Length];

    protected void Start()
    {
        _activeStatuses = new StatusAilment[Enum.GetNames(typeof(EnumLib.Status)).Length];

        for(int i = 0 ; i < _statusBuildUps.Length; i++)
        {
            if (_statusBuildUps[i].maxBuildUp == 0)
                _statusBuildUps[i].maxBuildUp = 100;
        }
    }

    protected int GetThreshold(int statusIndex)
    {
        return (_statusBuildUps[statusIndex].maxBuildUp + _statusBuildUps[statusIndex].thresholdIncrease
              * _statusBuildUps[statusIndex].currentIncreases);
    }

    protected virtual IEnumerator StatusTimer(EnumLib.Status statusType)
    {
        float duration = 15;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _statusTimers[(int)statusType] = null;
        
        ClearStatus(statusType);
    }

    protected void ClearStatus(EnumLib.Status statusType)
    {
        int statusIndex = (int)statusType;
        StatusAilment status = _activeStatuses[statusIndex];
        status.duration = 0;
        status.triggered = false;
        status.statusBuildUp = 0;

        StatusBuildUp currentBuildUp = _statusBuildUps[statusIndex];

        currentBuildUp.currentIncreases = Mathf.Clamp(currentBuildUp.currentIncreases + 1,0,currentBuildUp.maxIncreases);

        _activeStatuses[statusIndex] = status;
        _statusBuildUps[statusIndex] = currentBuildUp;

    }

    public virtual void ApplyBuildUp(EnumLib.Status statusType, int buildUpVal)
    {
        int statusIndex = (int)statusType;
        StatusAilment status = _activeStatuses[statusIndex];

        if (!status.triggered)
        {
            status.statusBuildUp += buildUpVal;

            if (status.statusBuildUp >= GetThreshold(statusIndex))
            {
                status.triggered = true;
                _statusTimers[statusIndex] = StartCoroutine(StatusTimer(statusType));
            }
        }

        _activeStatuses[statusIndex] = status;
    }
}