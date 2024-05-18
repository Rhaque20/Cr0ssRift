using System;
using System.Collections;
using UnityEngine;

public class StatusTracker : MonoBehaviour
{
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
        public int maxThresholds;
        public int currentIncreases;
    };
    protected StatusAilment[] _activeStatuses;
    [SerializeField]protected StatusBuildUp[] _statusBuildUps = new StatusBuildUp[Enum.GetNames(typeof(EnumLib.Status)).Length];

    protected Coroutine[] _statusTimers = new Coroutine[Enum.GetNames(typeof(EnumLib.Status)).Length];

    protected void Start()
    {
        _activeStatuses = new StatusAilment[Enum.GetNames(typeof(EnumLib.Status)).Length];
    }

    public int GetThreshold(int statusIndex)
    {
        return (_statusBuildUps[statusIndex].maxBuildUp + _statusBuildUps[statusIndex].thresholdIncrease
              * _statusBuildUps[statusIndex].currentIncreases);
    }

    protected IEnumerator StatusTimer(EnumLib.Status statusType)
    {
        if (statusType == EnumLib.Status.Burn)
        {
            float duration = 15;
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
            yield return new WaitForSeconds(15);
    }

    protected void ApplyBuildUp(EnumLib.Status statusType, int buildUpVal)
    {
        int statusIndex = (int) statusType;
        if (!_activeStatuses[statusIndex].triggered)
        {
            _activeStatuses[statusIndex].statusBuildUp += buildUpVal;

            if (_activeStatuses[statusIndex].statusBuildUp >= GetThreshold(statusIndex))
            {
                _activeStatuses[statusIndex].triggered = true;
            }
        }
    }
}