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

    protected Forces _forces;

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
        _forces = GetComponent<Forces>();
        GetComponent<GlobalVariables>().onArmorBreak += SetIsArmored;
        GetComponent<Stats>().onDeath += OnDeath;
        GetComponent<GlobalVariables>().onBeingCountered += Stagger;
    }

    public void SetIsArmored(bool value)
    {
        _isArmored = value;
    }

    protected virtual void Recovery()
    {
        if(!_isArmored || _anim.GetBool("Staggered"))
        {
            _anim.SetBool("Staggered",false);
            GetComponent<GlobalVariables>().setMove?.Invoke(true);
            GetComponent<CombatCore>().Recover();
        }
        
        _staggerTimer = null;

        _spriteRenderer.color = Color.white;
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

        Debug.Log("Ending stagger");

        Recovery();
    }
    
    public void Stagger()
    {
        if(_staggerTimer != null)
        {
            StopCoroutine(_staggerTimer);
            Debug.Log("Canceling stagger");
        }
        Debug.Log("Starting stagger");
        _staggerTimer = StartCoroutine(StaggerTimer());
    }

    public virtual void KnockBack(Vector3 attackerPos)
    {
        if(!_isArmored)
            _forces.Knockback(_knockBackPower,attackerPos,_staggerDuration);

        Stagger();

    }

    public void OnDeath()
    {
        if (_staggerTimer != null)
            StopCoroutine(_staggerTimer);

        GetComponent<GlobalVariables>().onArmorBreak -= SetIsArmored;
    }
}