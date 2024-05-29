using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerSkill", menuName = "PlayerSkill")]
public class PlayerSkill : Skill
{
    [SerializeField]
    private int SPCost = 0;

    [SerializeField]
    private bool _canOverrideElement = false;

    public bool canOverrideElement
    {
        get { return _canOverrideElement;}
    }
}