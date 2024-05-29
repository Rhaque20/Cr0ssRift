using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyMovement : Movement
{
    [SerializeField] private Transform _targetPos;
    [SerializeField] private float _bubbleDistance = 1f;
    [SerializeField] private bool _ignoreBubble = false;
    private CapsuleCollider _capsuleCollider;

    public Transform targetPos
    {
        get { return _targetPos; }
    }
    protected void Start()
    {
        base.Start();
        RelocatePlayer();
        PlayerPartyManager.instance.onPlayerSwitched += RelocatePlayer;
        GetComponent<EnemyStats>().onDeath += OnDeath;
        GetComponent<EnemyVariables>().readyToAttack += SetIgnoreBubble;
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public override void OnDeath()
    {
        PlayerPartyManager.instance.onPlayerSwitched -= RelocatePlayer;
    }

    void RelocatePlayer()
    {
        _targetPos = PlayerPartyManager.instance.getActivePlayer.transform;
    }

    public void SetIgnoreBubble(bool value)
    {
        _ignoreBubble = value;
    }

    protected override void Move()
    {
        Vector3 targetDir = _targetPos.position - transform.position;
        if (targetDir.x != 0)
            FaceDirection(targetDir.x);

        if (Vector3.Distance(transform.position, _targetPos.position) > _bubbleDistance + _capsuleCollider.radius || _ignoreBubble)
        {
            targetDir = targetDir.normalized;
            _rigid.MovePosition(transform.position + (_moveSpeed * Time.deltaTime * targetDir));
            _anim.SetFloat("MoveIntensity",targetDir.magnitude);
        }
        
    }

    void FixedUpdate()
    {
        if(_canMove)
            Move();
    }
}