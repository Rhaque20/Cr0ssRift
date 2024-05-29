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
    Coroutine[] _switchCoolDown = new Coroutine[3];

    public Action<bool> onGameOver;

    bool _gamePaused = false;

    void Awake()
    {
        instance = this;

        _playerControls = new PlayerControls();
        _playerControls.Combat.Enable();
        _playerControls.Combat.Switch1.performed += ctx =>SwitchToCharacter(0);
        _playerControls.Combat.Switch2.performed += ctx => SwitchToCharacter(1);
        _playerControls.Combat.Switch3.performed += ctx => SwitchToCharacter(2);


        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == 3 || !transform.GetChild(i).CompareTag("Player"))
                break;

            _players.Add(transform.GetChild(i).gameObject);

            PlayerVariables _playerVar = _players[i].GetComponent<PlayerVariables>();
            
            _switchCoolDown[i] = null;

            _playerVar.Initialize(_playerControls);
            _playerVar.onDeath += ForceSwitch;

            _playerStates[i].GetComponent<PlayerStatusTracker>().SetUpTracker(_playerVar,
            _players[i].transform.Find("Status Sprite").GetComponent<SpriteRenderer>());

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
                _playerVar.onForcedUnSummon += _offFieldRecovery[i].SetPenalize;
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
        _switchCoolDown[index] = null;
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

    public void ExitGame()
    {
        Application.Quit();
    }

    void SetCharacterPosition(int tempactive)
    {
        _players[tempactive].transform.position = _players[_active].transform.position;
    }

    public void ForceSwitch()
    {
        int tempActive = (_active + 1) % _players.Count;
        PlayerStats _newStats = _players[tempActive].GetComponent<PlayerStats>();

        PlayerUIManager.instance.SetHealthBar(0,1,_active);
        PlayerUIManager.instance.SetMiniBarCooldown(1f,_active);
        PlayerUIManager.instance.SetMiniBarCooldownColor(EnumLib.Status.Dead,_active);

        if (_newStats.isDead)
        {
            tempActive = (tempActive + 1) % _players.Count;
            _newStats = _players[tempActive].GetComponent<PlayerStats>();

            if (_newStats.isDead)
            {
                // Trigger GAME OVER;
                // Maybe still trigger switch out?
                onGameOver?.Invoke(true);
                Debug.Log("Gameover!");
            }
            else
            {
                SwitchCharacter(tempActive);
            }
        }
        else
        {
            SwitchCharacter(tempActive);
        }

    }

    public void SwitchToCharacter(int index)
    {
        if (index == _active)
            return;

        if(index >= _players.Count || index < 0)
            return;

        if (_switchCoolDown[index] != null)
        {
            Debug.Log("Switch under cooldown");
            return;
        }

        PlayerStats _newStats = _players[index].GetComponent<PlayerStats>();

        if(!_newStats.isDead)
        {
            SwitchCharacter(index);
        }
        
    }

    public void SwitchDirection(bool _switchLeft)
    {
        int _leftActive,_rightActive;

            _leftActive = (_active - 1 < 0 ? _players.Count - 1 : _active - 1);
            _rightActive = (_active + 1) % _players.Count;

        PlayerStats _newStats;

        if (_switchLeft)
        {
            if (_switchCoolDown[_leftActive] != null)
            {
                Debug.Log("Switching under cooldown left side");

                return;
            }
        }
        else
        {
            if (_switchCoolDown[_rightActive] != null)
            {
                Debug.Log("Switching under cooldown right side");

                return;
            }
        }

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

    public void SwitchCharacter(int tempActive)
    {
        ISwitchCharacter[] switchComps = _players[_active].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching out of "+_players[_active].name);

        PlayerStats _newStats;

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchOut();
        }

        _players[_active].SetActive(false);

        _offFieldRecovery[_active].SwitchOut();

        switchComps = _players[tempActive].GetComponentsInChildren<ISwitchCharacter>(false);

        Debug.Log("Switching in to "+_players[tempActive].name);

        _players[tempActive].SetActive(true);

        foreach(ISwitchCharacter comp in switchComps)
        {
            comp.SwitchIn();
        }

        SetCharacterPosition(tempActive);

        _offFieldRecovery[tempActive].SwitchIn();
        _playerStates[tempActive].GetComponent<PlayerStatusTracker>().CheckActiveCCStatus();

        _players[tempActive].GetComponent<PlayerVariables>().onForcedUnSummon += _offFieldRecovery[tempActive].SetPenalize;
        _players[_active].GetComponent<PlayerVariables>().onForcedUnSummon -= _offFieldRecovery[_active].SetPenalize;

        PlayerUIManager.instance.ToggleMiniBarDisplay(_active,true);
        PlayerUIManager.instance.ToggleMiniBarDisplay(tempActive,false);

        _newStats = _players[_active].GetComponent<PlayerStats>();

        if(_switchCoolDown[_active] == null && !_newStats.isDead)
        {
            _switchCoolDown[_active] = StartCoroutine(SwitchCoolDown(_active));
        }

        _active = tempActive;

        onPlayerSwitched?.Invoke();

    }

    public GameObject getActivePlayer
    {
        get{return _players[_active];}
    }
}