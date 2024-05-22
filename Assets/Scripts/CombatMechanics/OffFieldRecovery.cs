

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OffFieldRecovery : MonoBehaviour, IOnDeath, ISwitchCharacter
{
    [SerializeField]private PlayerStats _owner;
    private bool _isOnField = false, _canRecover = true;

    [SerializeField]private bool _isPenalized = false;

    protected Coroutine _regenerateArmor = null;

    [SerializeField] private float _healDelay = 1f, _penaltyDuration = 5;

    [SerializeField] private float _spDrainOnField = 2.5f,_spRegenOnField = 2f, _spRegenOffField = 5f;

    private float _interval = 0;

    Coroutine _penaltyTimer = null;

    public void SetOwner(PlayerStats owner)
    {
        _owner = owner;
        _owner.allVariables.onArmorBreak += TriggerRegenerateArmor;
        _owner.allVariables.onDeath += OnDeath;
    }

    public void SetFieldStatus(bool value)
    {
        _isOnField = value;
    }

    public void TriggerRegenerateArmor(bool value)
    {
        if(_regenerateArmor == null)
        {
            _regenerateArmor = StartCoroutine(RegenerateArmor());
        }

    }

    private IEnumerator RegenerateArmor()
    {
        float recoverTime = 5f;

        while (recoverTime > 0f)
        {
            if(_isOnField)
            {
                recoverTime -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                recoverTime -= Time.deltaTime * 2;
                yield return new WaitForSeconds(Time.deltaTime * 2);
            }
        }
        _owner.FullRestoreArmor();

        if (_isOnField)
            PlayerUIManager.instance.SetArmorBar((float)_owner.currentArmor/(float)_owner.maxArmor);
        
        _regenerateArmor = null;
    }

    private IEnumerator PenaltyTimer()
    {
        float penaltyTime = _penaltyDuration;

        while(penaltyTime > 0f)
        {
            if (_isOnField)
            {
                PlayerUIManager.instance.SetSPDrainBar(1 - (penaltyTime/_penaltyDuration));
            }
            else
            {
                PlayerUIManager.instance.SetSPDrainBar(1 - (penaltyTime/_penaltyDuration),transform.GetSiblingIndex());
            }

            penaltyTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _owner.playerVariables.onForcedUnSummon?.Invoke();

        if (_isOnField)
            PlayerUIManager.instance.SetSPDrainBar(0);
        else
            PlayerUIManager.instance.SetSPDrainBar(0,transform.GetSiblingIndex());

        _owner.RecoverSP(100);
        
        _penaltyTimer = null;
        
    }

    public void SetPenalize()
    {
        _isPenalized = !_isPenalized;

        if(_isPenalized)
        {
            Debug.Log(this.name+" got hit with the penalty");
            if(_penaltyTimer != null)
                StopCoroutine(_penaltyTimer);
            
            _penaltyTimer = StartCoroutine(PenaltyTimer());
        }
    }

    public void Update()
    {
        if(_canRecover && !_isPenalized)
        {
            if (!_isOnField)
            {
                _interval += Time.deltaTime;

                if (_interval >= _healDelay)
                {
                    Debug.Log("Recovering");
                    _owner.RecoverHealth(5);
                    _owner.RecoverArmor(5);
                    _interval = 0;
                }

                _owner.RecoverSP(_spRegenOffField * Time.deltaTime);
                
            }
            else
            {
                if(_owner.hasFamiliarSummoned)
                    _owner.RecoverSP(_spDrainOnField * Time.deltaTime * -1);
                else
                    _owner.RecoverSP(_spRegenOnField * Time.deltaTime);
            }
                
        }
        
    }

    public void OnDeath()
    {
        _canRecover = false;
    }

    public void SwitchOut()
    {
        _isOnField = false;
        _owner.RecoverHealth(0);
        _owner.RecoverArmor(0);
        _owner.RecoverSP(0);
    }

    public void SwitchIn()
    {
        _isOnField = true;
        _interval = 0;
    }
}