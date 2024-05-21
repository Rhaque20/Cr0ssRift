

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OffFieldRecovery : MonoBehaviour, IOnDeath, ISwitchCharacter
{
    [SerializeField]private PlayerStats _owner;
    private bool _isOnField = false, _canRecover = true;

    protected Coroutine _regenerateArmor = null;

    [SerializeField] private float _healDelay = 1f;

    private float _interval = 0;

    public void SetOwner(PlayerStats owner)
    {
        _owner = owner;
        _owner.allVariables.onArmorBreak += TriggerRegenerateArmor;
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

    public void Update()
    {
        if (_owner != null && !_isOnField && _canRecover)
        {
            _interval += Time.deltaTime;

            if (_interval >= _healDelay)
            {
                Debug.Log("Recovering");
                _owner.RecoverHealth(5);
                _owner.RecoverArmor(5);
                _interval = 0;
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
    }

    public void SwitchIn()
    {
        _isOnField = true;
        _interval = 0;
    }
}