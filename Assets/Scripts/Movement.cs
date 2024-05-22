using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour,IOnDeath
{
    [SerializeField] protected float _moveSpeed = 10f;
    protected Animator _anim;
    protected Rigidbody _rigid;

    protected bool _canMove = true;

    protected Vector3 _direction = Vector3.zero;

    public Vector3 direction
    {
        get { return _direction; }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        GetComponent<GlobalVariables>().setMove += SetMove;
    }

    protected virtual void Move()
    {

    }

    protected void SetMove(bool value)
    {
        _canMove = value;
    }

    protected virtual void FaceDirection(float x)
    {
        transform.localScale = new Vector3(Mathf.Sign(x),transform.localScale.y,transform.localScale.z);
    }

    public virtual void OnDeath()
    {
        
    }
}
