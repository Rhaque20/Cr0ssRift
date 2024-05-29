using System;
using System.Collections;
using UnityEngine;

public class PlayerStatusTracker : StatusTracker
{
    PlayerStats _playerStats;

    public override void SetUpTracker(GlobalVariables globalVariables, SpriteRenderer statusSprite)
    {
        _globalVariables = globalVariables;
        
        if (globalVariables is PlayerVariables)
        {
            _playerStats = (globalVariables as PlayerVariables).playerStats;
        }

        _statusVisual = statusSprite;
    }

    public void CheckActiveCCStatus()
    {
        if(_statusTimers[FROZEN] != null || _statusTimers[PARALYZE] != null)
        {
            _globalVariables.onImmobilized?.Invoke(false);
        }
        else
        {
            _globalVariables.onImmobilized?.Invoke(true);
        }
    }
    
    protected override IEnumerator StatusTimer(EnumLib.Status statusType)
    {
        float maxDuration = _maxDuration,duration = maxDuration;
        float timePassed = 0, timeReduction = 0;

        StatusAilment newStatusInfo = _activeStatuses[(int)statusType];
        newStatusInfo.duration = duration;

        _activeStatuses[(int)statusType] = newStatusInfo;
        
        if (statusType == EnumLib.Status.Frozen || statusType == EnumLib.Status.Paralyze)
        {
            _globalVariables.onImmobilized?.Invoke(false);
        }

        while (duration > 0)
        {
            if (_playerStats.gameObject.activeSelf)
            {
                timeReduction = Time.deltaTime;
            }
            else
            {
                timeReduction = 1.5f * Time.deltaTime;
            }

            duration -= timeReduction;
            timePassed += timeReduction;

            newStatusInfo = _activeStatuses[(int)statusType];
            newStatusInfo.duration = duration;
            
            _activeStatuses[(int)statusType] = newStatusInfo;

            if (_playerStats.gameObject.activeSelf)
                PlayerUIManager.instance.StatusDisplayTick(statusType,duration/maxDuration);
            else
                PlayerUIManager.instance.StatusDisplayTick(statusType,duration/maxDuration,transform.GetSiblingIndex());

            if(timePassed > 1)
            {
                timePassed = 0;
                if(statusType == EnumLib.Status.Burn || statusType == EnumLib.Status.Corrosion)
                {
                    _playerStats.DealDamageByStatus(10,statusType);
                }
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (statusType == EnumLib.Status.Frozen || statusType == EnumLib.Status.Paralyze)
        {
            _globalVariables.onImmobilized?.Invoke(true);
        }

        _statusTimers[(int)statusType] = null;
        
        ClearStatus(statusType);
        _statusVisual.sprite = null;
    }

    public override void ApplyBuildUp(EnumLib.Status statusType, int buildUpVal)
    {
        if (buildUpVal == 0)
            return;
        
        int statusIndex = (int)statusType;
        StatusAilment status = _activeStatuses[statusIndex];

        if (!status.triggered)
        {
            status.statusBuildUp += buildUpVal;
            Debug.Log("Apply status to"+this.name+" with build up of "+status.statusBuildUp);
            PlayerUIManager.instance.StatusBuildUpDisplay(statusType,(float)status.statusBuildUp/(float)GetThreshold(statusIndex));

            if (status.statusBuildUp >= GetThreshold(statusIndex))
            {
                status.triggered = true;
                PlayerUIManager.instance.StatusDisplayTick(statusType,1f);
                _statusTimers[statusIndex] = StartCoroutine(StatusTimer(statusType));
                _statusVisual.sprite = _statusSprites[statusIndex-1];
                
            }
        }

        _activeStatuses[statusIndex] = status;
    }
}