using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaggerSystem : MonoBehaviour
{
    protected Rigidbody _rigid;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _anim;
    protected bool _isArmored = false;

    [SerializeField] protected float _staggerDuration = 2f;

    [SerializeField] protected float _knockBackPower = 50f;

    protected Coroutine _staggerTimer = null;

    protected void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    protected IEnumerator StaggerTimer()
    {
        GetComponent<GlobalVariables>().setMove?.Invoke(false);
        _spriteRenderer.color = Color.red;
        _anim.Play("Stagger");
        _anim.SetBool("Staggered",true);

        yield return new WaitForSeconds(_staggerDuration);

        _anim.SetBool("Staggered",false);
        _spriteRenderer.color = Color.white;
        GetComponent<GlobalVariables>().setMove?.Invoke(true);
        _staggerTimer = null;

        GetComponent<CombatCore>().Recover();
    }

    public void KnockBack(Vector3 attackerPos)
    {
        _rigid.AddForce((attackerPos - transform.position) * -_knockBackPower);

        if(_staggerTimer != null)
        {
            StopCoroutine(_staggerTimer);
        }

        _staggerTimer = StartCoroutine(StaggerTimer());

    }
}