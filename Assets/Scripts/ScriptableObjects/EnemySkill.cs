using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySkill", menuName = "EnemySkill")]
public class EnemySkill : Skill
{
    [SerializeField]private float _coolDown = 0f;
    [SerializeField]private float _idleTime = 2f;
}