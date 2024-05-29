using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    List<GameObject> _projectilesList = new List<GameObject>();

    Dictionary<GameObject,Queue<GameObject>> _queueList = new ();
    
    public static ProjectileManager instance;

    void Awake()
    {
        instance = this;
    }

    public void AddProjectile(GameObject projectile, int quantity)
    {
        GameObject temp;

        if (_queueList.ContainsKey(projectile))
        {
            for(int i = 0; i < quantity; i++)
            {
                temp = Instantiate(projectile, transform);
                _queueList[projectile].Enqueue(temp);
                temp.GetComponent<Projectile>().SetUpPrefab(projectile);
                temp.SetActive(false);
            }
            return;
        }

        Queue<GameObject> queue = new Queue<GameObject>();

        for(int i = 0; i < quantity; i++)
        {
            temp = Instantiate(projectile, transform);
            queue.Enqueue(temp);
            temp.GetComponent<Projectile>().SetUpPrefab(projectile);
            temp.SetActive(false);
        }

        _queueList.Add(projectile,queue);
        
    }

    public void ReturnProjectile(GameObject objectRef,GameObject originalPrefab)
    {
        if(_queueList.ContainsKey(originalPrefab))
        {
            Debug.Log("Returned projectile "+objectRef);
            _queueList[originalPrefab].Enqueue(objectRef);
        }
        else
        {
            Debug.Log("Can't return the projectile");
        }
    }

    public GameObject SummonProjectile(GameObject projectilePrefab)
    {
        GameObject projectile = null;
        if(_queueList.ContainsKey(projectilePrefab))
        {
            projectile = _queueList[projectilePrefab].Dequeue();
        }
        
        return projectile;
    }
}