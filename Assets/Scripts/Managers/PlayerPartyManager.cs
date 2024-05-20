using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPartyManager : MonoBehaviour
{
    [SerializeField]GameObject[] players = new GameObject[3];
    public static PlayerPartyManager instance;

    private int _active = 0;

    void Awake()
    {
        instance = this;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == 3)
                break;
            
            players[i] = transform.GetChild(i).gameObject;
        }
    }

    public GameObject getActivePlayer
    {
        get{return players[_active];}
    }
}