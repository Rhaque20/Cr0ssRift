using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IvaraCore : PlayerCore
{
    [SerializeField]private float _boltPower = 0;
    [SerializeField]private float _maxHeldTime = 0.5f;
    [SerializeField]private float _chargeRate = 0.5f;
    [SerializeField]private float _projectileForce = 200f;

    [SerializeField]private float _damping = 20;

    [SerializeField]private LayerMask _mousePointMask;

    [SerializeField]Transform _aimTelegraph;

    

    [SerializeField]GameObject _bullet;
    Vector3 _firingDirection = Vector3.zero;

    Camera _activeCamera;

    protected override void Start()
    {
        base.Start();

        _chargedAttackAction = _playerControls.Combat.ChargeAttack;
        _normalAttackAction = _playerControls.Combat.NormalAttack;

        _activeCamera = Camera.main;

        if (_aimTelegraph != null)
        {
            _aimTelegraph.gameObject.SetActive(false);
        }

        if(_chargeBar)
        {
            _isReadyIcon = _chargeBar.transform.GetChild(0).GetComponent<Image>();
        }

        Debug.Log("Calling ivara start");

        ProjectileManager.instance.AddProjectile(_bullet,2);
    }

    public void SlowdownWhileCharging(InputAction.CallbackContext callback)
    {
        if(_boltPower < 1.0f)
            _playerStats.speedModifier = 0.5f;
        else
            _playerStats.speedModifier = 1.0f;
    }

    public void RevertSpeed(InputAction.CallbackContext callback)
    {
        _playerStats.speedModifier = 1.0f;
    }

    public override void Attack()
    {
        if(_boltPower >= 1.0f)
        {
            Debug.Log("Releasing a bolt");
            _boltPower = 0;
            _isReadyIcon.color = Color.white;
            _chargeBar.gameObject.SetActive(false);
            _isAttacking = true;

            _animOverrideController["Attack"] = _normalAttacks[0].ReturnAttackAnimation(0);
            _animOverrideController["Recover"] = _normalAttacks[0].ReturnAttackAnimation(1);
            _anim.Play("Attack");

            GetComponent<PlayerVariables>().setMove?.Invoke(false);
            if (_bullet != null)
            {
                Projectile _proj;
                GameObject temp = ProjectileManager.instance.SummonProjectile(_bullet);

                temp.transform.position = new Vector3(transform.position.x,transform.position.y + 0.25f,transform.position.z);

                _proj = temp.GetComponent<Projectile>();

                _proj.SetUpProjectile(_playerStats,_normalAttacks[0]);

                temp.transform.rotation = _aimTelegraph.transform.rotation;

                _proj.FireProjectile(_firingDirection.normalized,_projectileForce,1.5f);
            }

            if (_aimTelegraph != null)
            {
                _aimTelegraph.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not enough bolt power");
        }
    }

    public override void ChargeAttack()
    {
        if (_boltPower < 1.0f)
        {
            Debug.Log("Winding up a bolt");
            _charging = true;
            _boltPower += _chargeRate * Time.deltaTime * (_hasFamiliarSummoned ? 2 : 1);

            if (_chargeBar)
            {
                _chargeBar.fillAmount = _boltPower/1.0f;
            }

            if(!_chargeBar.gameObject.activeSelf)
            {
                _chargeBar.gameObject.SetActive(true);
            }

            if(_boltPower >= 1.0f)
            {
                _charging = false;
                _isReadyIcon.color = Color.black;
            }
        }
    }

    public void AimTracker()
    {
        _aimTelegraph.gameObject.SetActive(true);
        _aimTelegraph.transform.position = transform.position;
        
        Ray ray = _activeCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue,_mousePointMask))
        {
            _firingDirection = raycastHit.point - transform.position;
            _firingDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(_firingDirection);
            _aimTelegraph.transform.rotation = Quaternion.Slerp(_aimTelegraph.transform.rotation, rotation, Time.deltaTime * _damping);
        }

        

    }


    public override void SwitchOut()
    {
        base.SwitchOut();
        if (_aimTelegraph.gameObject.activeSelf)
            _aimTelegraph.gameObject.SetActive(false);
        _playerControls.Combat.ChargeAttack.performed -= SlowdownWhileCharging;
        _playerControls.Combat.ChargeAttack.canceled -= RevertSpeed;
    }

    public override void SwitchIn()
    {
        base.SwitchIn();

        if (!_aimTelegraph.gameObject.activeSelf && _boltPower >= 1f)
            _aimTelegraph.gameObject.SetActive(true);

        _playerControls.Combat.ChargeAttack.performed += SlowdownWhileCharging;
        _playerControls.Combat.ChargeAttack.canceled += RevertSpeed;
    }

    public override void OnDeath()
    {
        if (_aimTelegraph.gameObject.activeSelf)
        {
            _aimTelegraph.gameObject.SetActive(false);
        }

        SwitchOut();
    }

    void Update()
    {
        if (_chargedAttackAction.IsPressed() && _canAttack)
        {
            ChargeAttack();
        }

        if (_boltPower >= 1.0f && _canAttack)
        {
            AimTracker();
        }
    }
}