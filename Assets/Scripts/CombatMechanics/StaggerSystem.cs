using System.Collections;
using UnityEngine;

public class StaggerSystem : MonoBehaviour, IOnDeath
{
    protected Rigidbody _rigid;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _anim;
    [SerializeField]protected bool _isArmored = false;

    [SerializeField] protected float _staggerDuration = 2f;

    [SerializeField] protected float _knockBackPower = 50f;

    protected Coroutine _staggerTimer = null;

    public float staggerDuration
    {
        get { return _staggerDuration; }
    }

    public bool isStaggered
    {
        get { return !_isArmored; }
    }

    protected void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        GetComponent<GlobalVariables>().onArmorBreak += SetIsArmored;
        GetComponent<Stats>().onDeath += OnDeath;
    }

    public void SetIsArmored(bool value)
    {
        _isArmored = value;
    }

    protected virtual void Recovery()
    {
        if(!_isArmored)
        {
            _anim.SetBool("Staggered",false);
            GetComponent<GlobalVariables>().setMove?.Invoke(true);
            _staggerTimer = null;
        }
        

        _spriteRenderer.color = Color.white;

        GetComponent<CombatCore>().Recover();
    }

    protected virtual IEnumerator StaggerTimer()
    {
        if (!_isArmored)
        {
            GetComponent<GlobalVariables>().setMove?.Invoke(false);
            _anim.Play("Stagger");
            _anim.SetBool("Staggered",true);
        }
        
        _spriteRenderer.color = Color.red;
        

        yield return new WaitForSeconds(_staggerDuration);

        Recovery();
    }

    public virtual void KnockBack(Vector3 attackerPos)
    {
        if (!_isArmored)
            _rigid.AddForce((attackerPos - transform.position) * -_knockBackPower);

        if(_staggerTimer != null)
        {
            StopCoroutine(_staggerTimer);
        }

        _staggerTimer = StartCoroutine(StaggerTimer());

    }

    public void OnDeath()
    {
        if (_staggerTimer != null)
            StopCoroutine(_staggerTimer);

        GetComponent<GlobalVariables>().onArmorBreak -= SetIsArmored;
    }
}