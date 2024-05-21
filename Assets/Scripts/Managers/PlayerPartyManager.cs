using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPartyManager : MonoBehaviour
{
    [SerializeField]List<GameObject> _players = new List<GameObject>();
    [SerializeField]List<GameObject> _playerStates = new List<GameObject>();
    public static PlayerPartyManager instance;

    PlayerControls _playerControls;

    public Action onPlayerSwitched;

    private int _active = 0;

    List<OffFieldRecovery> _offFieldRecovery = new ();

    void Awake()
    {
        instance = this;

        _playerControls = new PlayerControls();
        _playerControls.Combat.Enable();
        _playerControls.Combat.SwitchLeft.performed += ctx => SwitchDirection(true);

        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == 3)
                break;
            
            _players.Add(transform.GetChild(i).gameObject);

            _players[i].GetComponent<PlayerVariables>().Initialize(_playerControls);

            _offFieldRecovery.Add(_playerStates[i].GetComponent<OffFieldRecovery>());

            _offFieldRecovery[i].SetOwner(_players[i].GetComponent<PlayerStats>());

            if(i == 0)
            {
                ISwitchCharacter[] switchComps = _players[i].GetComponentsInChildren<ISwitchCharacter>(false);

                foreach(ISwitchCharacter comp in switchComps)
                {
                    comp.SwitchIn();
                }
                _offFieldRecovery[i].SwitchIn();
                _players[i].GetComponent<PlayerVariables>().onForcedUnSummon += _offFieldRecovery[i].SetPenalize;
                PlayerUIManager.instance.ToggleMiniBarDisplay(i,false);
            }
            else
            {
                _offFieldRecovery[i].SwitchOut();
                _players[i].SetActive(false);
            }
        }

    }

    void SetCharacterPosition(int tempactive)
    {
        _players[tempactive].transform.position = _players[_active].transform.position;
    }

    public void SwitchDirection(bool _switchLeft)
    {
        int _tempActive = 0;

        if (_switchLeft)
            _tempActive = (_active - 1 < 0 ? _players.Count - 1 : _active - 1);
        else
            _tempActive = (_active + 1) % _players.Count;

        if (_tempActive == _active)
            return;
        
        SwitchCharacter(_tempActive);
    }

    public void SwitchCharacter(int _tempActive)
    {
        ISwitchCharacter[] switchComps = _players[_active].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching out of "+_players[_active].name);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchOut();
        }

        _players[_active].SetActive(false);

        _offFieldRecovery[_active].SwitchOut();

        switchComps = _players[_tempActive].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching in to "+_players[_tempActive].name);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchIn();
        }

        _players[_tempActive].SetActive(true);

        SetCharacterPosition(_tempActive);

        _offFieldRecovery[_tempActive].SwitchIn();

        _players[_tempActive].GetComponent<PlayerVariables>().onForcedUnSummon += _offFieldRecovery[_tempActive].SetPenalize;
        _players[_active].GetComponent<PlayerVariables>().onForcedUnSummon -= _offFieldRecovery[_active].SetPenalize;

        PlayerUIManager.instance.ToggleMiniBarDisplay(_active,true);
        PlayerUIManager.instance.ToggleMiniBarDisplay(_tempActive,false);

        _active = _tempActive;

        onPlayerSwitched?.Invoke();

    }

    public GameObject getActivePlayer
    {
        get{return _players[_active];}
    }
}