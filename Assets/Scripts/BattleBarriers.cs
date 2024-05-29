using UnityEngine;

public class BattleBarriers : MonoBehaviour
{
    // Start is called before the first frame update

    public enum TriggerDirection
    {
        PositiveX,NegativeX,PositiveZ,NegativeZ
    }

    [SerializeField]TriggerDirection _triggerDir;
    [SerializeField]int index = 1;

    BoxCollider _collisionBox;
    bool _activated = false;
    void Start()
    {
        _collisionBox = GetComponent<BoxCollider>();
    }

    bool Activated(Vector3 position)
    {
        bool value = false;
        switch(_triggerDir)
        {
            case TriggerDirection.PositiveX:
                value = position.x > transform.position.x;
                break;
             case TriggerDirection.NegativeX:
                value = position.x < transform.position.x;
                break;
            case TriggerDirection.PositiveZ:
                value = position.z > transform.position.z;
                break;
            case TriggerDirection.NegativeZ:
                value = position.z < transform.position.z;
                break;
        }

        return value;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && !_activated)
        {
            if (Activated(other.transform.position))
            {
                LevelManager.instance.SpawnWave(index);
                _collisionBox.isTrigger = false;
                _activated = true;
            }
        }
    }
}
