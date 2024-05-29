using UnityEngine;
public class LunmoCore : PlayerCore
{
    [SerializeField]Sprite[] _chargeLevel = new Sprite[6];
    [SerializeField]Sprite[] _crackLevel = new Sprite[6];
    [SerializeField]Sprite[] _slashLevel = new Sprite[6];
    float _chargePower = 0;

    
    protected override void Start()
    {
        base.Start();
        _chargedAttackAction = _playerControls.Combat.ChargeAttack;
    }

    private void ResetCharge()
    {
        _chargePower = 0;
    }

    public virtual void ChargeAttack()
    {
        
    }



    void Update()
    {
        if (_chargedAttackAction.IsPressed())
        {
            ChargeAttack();
        }
    }
}