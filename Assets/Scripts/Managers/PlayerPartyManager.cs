using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPartyManager : MonoBehaviour
{
    [SerializeField]List<GameObject> _players = new List<GameObject>();
    [SerializeField]List<GameObject> _playerStates = new List<GameObject>();
    public static PlayerPartyManager instance;

    PlayerControls _playerControls;

    public Action onPlayerSwitched;

    private int _active = 0;

    List<OffFieldRecovery> _offFieldRecovery = new ();
    Coroutine _switchCoolDown = null;

    public Action<bool> onGameOver;

    bool _gamePaused = false;

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
            _players[i].GetComponent<PlayerVariables>().onDeath += ForceSwitch;

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

    private IEnumerator SwitchCoolDown(int index)
    {
        float timeRemaining = 4f;
        while (timeRemaining > 0)
        {
            PlayerUIManager.instance.SetMiniBarCooldown(timeRemaining/4f,index);
            yield return new WaitForSeconds(Time.deltaTime);
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                PlayerUIManager.instance.SetMiniBarCooldown(0/4f,index);
            }
        }
        _switchCoolDown = null;
    }

    void Start()
    {
        _playerControls.Combat.PauseGame.performed += ctx => {
            _gamePaused = !_gamePaused;
            PlayerUIManager.instance.TriggerPauseScreen(_gamePaused);
            TimeManager.instance.PauseGame(_gamePaused);
            };
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        TimeManager.instance.PauseGame(false);
        SceneManager.LoadScene(scene.name);
    }

    void SetCharacterPosition(int tempactive)
    {
        _players[tempactive].transform.position = _players[_active].transform.position;
    }

    public void ForceSwitch()
    {
        int _tempActive = (_active + 1) % _players.Count;
        PlayerStats _newStats = _players[_tempActive].GetComponent<PlayerStats>();

        PlayerUIManager.instance.SetHealthBar(0,1,_active);

        if (_newStats.isDead)
        {
            _tempActive = (_tempActive + 1) % _players.Count;
            _newStats = _players[_tempActive].GetComponent<PlayerStats>();

            if (_newStats.isDead)
            {
                // Trigger GAME OVER;
                // Maybe still trigger switch out?
                onGameOver?.Invoke(true);
                Debug.Log("Gameover!");
            }
            else
            {
                SwitchCharacter(_tempActive);
            }
        }
        else
        {
            SwitchCharacter(_tempActive);
        }

    }

    public void SwitchDirection(bool _switchLeft)
    {
        if (_switchCoolDown != null)
        {
            Debug.Log("Switching under cooldown");

            return;
        }
        int _leftActive,_rightActive;

            _leftActive = (_active - 1 < 0 ? _players.Count - 1 : _active - 1);
            _rightActive = (_active + 1) % _players.Count;

        PlayerStats _newStats;

        if(_switchLeft)
        {
            _newStats = _players[_leftActive].GetComponent<PlayerStats>();
            if (!_newStats.isDead)
            {
                SwitchCharacter(_leftActive);
                return;
            }
            else
            {
                _newStats = _players[_rightActive].GetComponent<PlayerStats>();
                if(!_newStats.isDead)
                {
                    SwitchCharacter(_rightActive);
                    return;
                }
            }
        }
        else
        {
            _newStats = _players[_rightActive].GetComponent<PlayerStats>();
            if (!_newStats.isDead)
            {
                SwitchCharacter(_rightActive);
                return;
            }
            else
            {
                _newStats = _players[_leftActive].GetComponent<PlayerStats>();
                if (!_newStats.isDead)
                {
                    SwitchCharacter(_leftActive);
                    return;
                }
            }
        }
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

        _players[_tempActive].SetActive(true);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchIn();
        }

        SetCharacterPosition(_tempActive);

        _offFieldRecovery[_tempActive].SwitchIn();

        _players[_tempActive].GetComponent<PlayerVariables>().onForcedUnSummon += _offFieldRecovery[_tempActive].SetPenalize;
        _players[_active].GetComponent<PlayerVariables>().onForcedUnSummon -= _offFieldRecovery[_active].SetPenalize;

        PlayerUIManager.instance.ToggleMiniBarDisplay(_active,true);
        PlayerUIManager.instance.ToggleMiniBarDisplay(_tempActive,false);

        if(_switchCoolDown == null)
        {
            _switchCoolDown = StartCoroutine(SwitchCoolDown(_active));
        }

        _active = _tempActive;

        onPlayerSwitched?.Invoke();

    }

    public GameObject getActivePlayer
    {
        get{return _players[_active];}
    }
}