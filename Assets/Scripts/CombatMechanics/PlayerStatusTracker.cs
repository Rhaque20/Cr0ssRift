using System;
using System.Collections;
using UnityEngine;

public class PlayerStatusTracker : StatusTracker
{
    protected override IEnumerator StatusTimer(EnumLib.Status statusType)
    {
        float maxDuration = 15,duration = maxDuration;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            PlayerUIManager.instance.StatusDisplayTick(statusType,duration/maxDuration);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _statusTimers[(int)statusType] = null;
        
        ClearStatus(statusType);
    }

    public override void ApplyBuildUp(EnumLib.Status statusType, int buildUpVal)
    {
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
                
            }
        }

        _activeStatuses[statusIndex] = status;
    }
}