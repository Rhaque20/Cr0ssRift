using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaggerSystem : MonoBehaviour
{
    protected Rigidbody _rigid;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _anim;
    [SerializeField]protected bool _isArmored = false;

    [SerializeField] protected float _staggerDuration = 2f;

    [SerializeField] protected float _knockBackPower = 50f;

    protected Coroutine _staggerTimer = null;

    protected void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        GetComponent<GlobalVariables>().onArmorBreak += value => _isArmored = value;
    }

    protected void Recovery()
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

    protected IEnumerator StaggerTimer()
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
}