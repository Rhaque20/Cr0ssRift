using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPartyManager : MonoBehaviour
{
    [SerializeField]List<GameObject> _players = new List<GameObject>();
    public static PlayerPartyManager instance;

    PlayerControls _playerControls;

    public Action onPlayerSwitched;

    private int _active = 0;

    void Awake()
    {
        instance = this;

        _playerControls = new PlayerControls();
        _playerControls.Combat.Enable();
        _playerControls.Combat.SwitchLeft.performed += ctx => SwitchCharacter(true);

        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == 3)
                break;
            
            _players.Add(transform.GetChild(i).gameObject);

            _players[i].GetComponent<PlayerVariables>().Initialize(_playerControls);

            if(i == 0)
            {
                ISwitchCharacter[] switchComps = _players[i].GetComponentsInChildren<ISwitchCharacter>(false);

                foreach(ISwitchCharacter comp in switchComps)
                {
                    comp.SwitchIn();
                }
            }
            else
            {
                _players[i].SetActive(false);
            }
        }

    }

    void SetCharacterPosition(int tempactive)
    {
        _players[tempactive].transform.position = _players[_active].transform.position;
    }

    public void SwitchCharacter(bool _switchLeft)
    {
        int _tempActive = (_active - 1 < 0 ? _players.Count - 1 : _active - 1);

        if (_tempActive == _active)
            return;

        ISwitchCharacter[] switchComps = _players[_active].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching out of "+_players[_active].name);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchOut();
        }

        _players[_active].SetActive(false);

        switchComps = _players[_tempActive].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching in to "+_players[_tempActive].name);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchIn();
        }

        _players[_tempActive].SetActive(true);

        SetCharacterPosition(_tempActive);

        _active = _tempActive;

        onPlayerSwitched?.Invoke();

    }

    public GameObject getActivePlayer
    {
        get{return _players[_active];}
    }
}