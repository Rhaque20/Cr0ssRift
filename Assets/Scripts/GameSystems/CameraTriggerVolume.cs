using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CameraTriggerVolume : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private Vector3 _boxSize;

    BoxCollider _box;
    Rigidbody _rigid;

    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
        _rigid = GetComponent<Rigidbody>();
        
        _box.isTrigger = true;
        _box.size = _boxSize;

        _rigid.isKinematic = true;

        if(_cam == null)
        {
            _cam = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position,_boxSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (CameraSwapper.activeCamera != _cam)
            {
                Debug.Log("Swapping Cameras with "+this.name);
                CameraSwapper.SwitchCamera(_cam);
            }
        }
    }

}