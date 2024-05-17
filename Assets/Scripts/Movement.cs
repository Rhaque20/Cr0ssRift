using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 10f;
    protected Animator _anim;
    protected Rigidbody _rigid;

    protected Vector3 _direction = Vector3.zero;
    // Start is called before the first frame update
    protected void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    protected virtual void Move()
    {

    }

    protected virtual void FaceDirection(float x)
    {
        transform.localScale = new Vector3(Mathf.Sign(x),transform.localScale.y,transform.localScale.z);
    }
}
