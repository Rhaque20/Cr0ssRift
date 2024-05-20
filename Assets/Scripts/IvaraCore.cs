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

    [SerializeField]Image _chargeBar;

    Image _isReadyIcon;

    private bool _charging = false;

    private float _heldTime = 0;

    [SerializeField]GameObject _bullet;

    Vector3 _firingDirection = Vector3.zero;

    Camera _activeCamera;

    void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        PlayerControls playerInputActions = GetComponent<PlayerVariables>().playerInputActions;


        playerInputActions.Combat.NormalAttack.performed += ctx => Attack();
        //playerInputActions.Combat.NormalAttack.canceled += ctx => Attack();
        //playerInputActions.Combat.ChargeAttack.performed += HoldInput;

        _animOverrideController = GetComponent<PlayerVariables>().animOverrideController;

        _chargedAttackAction = playerInputActions.Combat.ChargeAttack;
        _normalAttackAction = playerInputActions.Combat.NormalAttack;

        _activeCamera = Camera.main;

        if (_aimTelegraph != null)
        {
            _aimTelegraph.gameObject.SetActive(false);
        }

        if(_chargeBar)
        {
            _isReadyIcon = _chargeBar.transform.GetChild(0).GetComponent<Image>();
        }
    }

    public override void Attack()
    {
        // if (_heldTime < _maxHeldTime)
        //     Debug.Log("Releasing a bolt");
        // else
        //     Debug.Log("Too long to be a tap");

        // _heldTime = 0;

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
                GameObject temp = Instantiate(_bullet);

                temp.transform.position = new Vector3(transform.position.x,transform.position.y + 0.25f,transform.position.z);

                Destroy(temp,1.5f);

                temp.transform.rotation = _aimTelegraph.transform.rotation;

                temp.GetComponent<Rigidbody>().AddForce(_projectileForce*_firingDirection.normalized, ForceMode.Impulse);
            }

            if (_aimTelegraph != null)
            {
                _aimTelegraph.gameObject.SetActive(false);
            }
        }
    }

    public override void ChargeAttack()
    {
        if (_boltPower < 1.0f)
        {
            Debug.Log("Winding up a bolt");
            _charging = true;
            _boltPower += _chargeRate * Time.deltaTime;

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

    void Update()
    {
        if (_chargedAttackAction.IsPressed())
        {
            ChargeAttack();
        }

        if (_boltPower >= 1.0f)
        {
            AimTracker();
        }

        // if (_normalAttackAction.IsPressed() && _heldTime <= _maxHeldTime)
        // {
        //     _heldTime += Time.deltaTime;
        // }
    }
}