using System;
using System.Collections;
using UnityEngine;

public class StatusTracker : MonoBehaviour
{
    protected const int WEAKEN = 0, BURN = 1, FROZEN = 2, PARALYZE = 3;
    protected float _maxDuration = 15;
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

    [SerializeField]protected Sprite[] _statusSprites = new Sprite[3];
    protected SpriteRenderer _statusVisual;

    protected Coroutine[] _statusTimers = new Coroutine[Enum.GetNames(typeof(EnumLib.Status)).Length];

    protected GlobalVariables _globalVariables;

    protected virtual void Start()
    {
        _activeStatuses = new StatusAilment[Enum.GetNames(typeof(EnumLib.Status)).Length];

        for(int i = 0 ; i < _statusBuildUps.Length; i++)
        {
            if (_statusBuildUps[i].maxBuildUp == 0)
                _statusBuildUps[i].maxBuildUp = 100;
        }
    }

    public bool HasStatus(EnumLib.Status status)
    {
        return _statusTimers[(int)status] != null;
    }

    public bool HasBuildUp(EnumLib.Status status)
    {
        return _activeStatuses[(int)status].statusBuildUp > 0;
    }

    public virtual void SetUpTracker(GlobalVariables global, SpriteRenderer statusSprite)
    {
        _globalVariables = global;
        _statusVisual = statusSprite;
    }

    protected int GetThreshold(int statusIndex)
    {
        return (_statusBuildUps[statusIndex].maxBuildUp + _statusBuildUps[statusIndex].thresholdIncrease
              * _statusBuildUps[statusIndex].currentIncreases);
    }

    protected virtual IEnumerator StatusTimer(EnumLib.Status statusType)
    {
        float duration = _maxDuration;
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