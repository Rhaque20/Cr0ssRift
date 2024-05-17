using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerSkill", menuName = "PlayerSkill")]
public class PlayerSkill : Skill
{
    [SerializeField]
    private int SPCost = 0;
}