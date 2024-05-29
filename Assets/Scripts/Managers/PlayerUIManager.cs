using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    const int MAINBAR = 0;
    public static PlayerUIManager instance;

    [SerializeField]private PlayerBars[] _playerBars = new PlayerBars[4];
    [SerializeField]private StatusUI[] _statusUI = new StatusUI[4];

    [SerializeField]private GameObject _gameOverScreen,_pauseScreen,_victoryScreen,_challengeScreen;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetPlayerBarsDisplay();
        PlayerPartyManager.instance.onPlayerSwitched += ResetPlayerBarsDisplay;
        PlayerPartyManager.instance.onGameOver += TriggerGameOverScreen;

        _gameOverScreen.SetActive(false);
        _pauseScreen.SetActive(false);
    }

    public void ToggleMiniBarDisplay(int index, bool value)
    {
        _playerBars[index + 1].gameObject.SetActive(value);
    }

    void ResetPlayerBarsDisplay()
    {
        PlayerStats stats = PlayerPartyManager.instance.getActivePlayer.GetComponent<PlayerStats>();
        SetHealthBar(stats.currentHP,stats.maxHP);
        SetArmorBar((float)stats.currentArmor/(float)stats.maxArmor);
    }

    public void SetMiniBarCooldown(float ratio, int index)
    {
        _playerBars[index + 1].SetCoolDown(ratio);
    }

    public void SetMiniBarCooldownColor(EnumLib.Status status, int index)
    {
        _playerBars[index + 1].SetCoolDownColor(status);
    }

    public void SetHealthBar(int _currentHP, int _maxHP)
    {
        _playerBars[MAINBAR].SetHealthBar(_currentHP, _maxHP);
    }

    public void SetHealthBar(int _currentHP, int _maxHP, int index)
    {
        _playerBars[index + 1].SetHealthBar(_currentHP, _maxHP);
    }

    public void SetArmorBar(float ratio)
    {
        _playerBars[MAINBAR].SetArmorBar(ratio);
    }

    public void SetArmorBar(float ratio, int index)
    {
        _playerBars[index + 1].SetArmorBar(ratio);
    }

    public void SetSPBar(float ratio)
    {
        _playerBars[MAINBAR].SetSPBar(ratio);
    }

    public void SetSPBar(float ratio, int index)
    {
        _playerBars[index + 1].SetSPBar(ratio);
    }

    public void SetSPDrainBar(float ratio)
    {
        _playerBars[MAINBAR].SetSPDrainBar(ratio);
    }

    public void SetSPDrainBar(float ratio, int index)
    {
        _playerBars[index + 1].SetSPDrainBar(ratio);
    }

    public void StatusBuildUpDisplay(EnumLib.Status status, float ratio)
    {
        _statusUI[0].SetStatusBuildUpDisplay(status,ratio);
    }

    public void StatusBuildUpDisplay(EnumLib.Status status, float ratio, int index)
    {
        _statusUI[index + 1].SetStatusBuildUpDisplay(status,ratio);
    }

    public void StatusDisplayTick(EnumLib.Status status,float ratio)
    {
        _statusUI[0].SetStatusDisplayTick(status,ratio);
    }

    public void StatusDisplayTick(EnumLib.Status status,float ratio,int index)
    {
        _statusUI[index + 1].SetStatusDisplayTick(status,ratio);
    }

    // Edge cases of duration is 0 but has some build up for when swapping around
    public void StatusDisplayTick(EnumLib.Status status,float ratio,float buildUpRatio)
    {
        _statusUI[0].SetStatusDisplayTick(status,ratio,buildUpRatio);
    }

    public void StatusDisplayTick(EnumLib.Status status,float ratio,float buildUpRatio,int index)
    {
        _statusUI[index + 1].SetStatusDisplayTick(status,ratio,buildUpRatio);
    }

    public void RefreshStatusDisplay(PlayerStatusTracker _playerStatus)
    {
        float ratio;
        for(int i = 0; i < 4; i++)
        {
            
            if(_playerStatus.activeStatuses[i].triggered)
            {
                ratio = (float)_playerStatus.activeStatuses[i].duration/15;
                Debug.Log(((EnumLib.Status)i).ToString()+" is triggered");
                StatusBuildUpDisplay((EnumLib.Status)i,1f);
                StatusDisplayTick((EnumLib.Status)i,ratio,1f);
            }
            else
            {
                ratio = (float)_playerStatus.activeStatuses[i].statusBuildUp/_playerStatus.statusBuildUps[i].maxBuildUp;
                if (ratio > 0f)
                {
                    Debug.Log(((EnumLib.Status)i).ToString()+" build up for main display "+ratio);
                }
                else
                {
                    Debug.Log(((EnumLib.Status)i).ToString()+" status is completely gone");
                }
                StatusBuildUpDisplay((EnumLib.Status)i,ratio);
                StatusDisplayTick((EnumLib.Status)i,0f,ratio);
            }
        }
    }

    public void RefreshStatusDisplay(PlayerStatusTracker _playerStatus,int index)
    {
        float ratio;
        for(int i = 0; i < 4; i++)
        {
            if(_playerStatus.activeStatuses[i].triggered)
            {
                Debug.Log("Displaying activated status on mini "+((EnumLib.Status)i).ToString()+" on Index "+index);
                ratio = (float)_playerStatus.activeStatuses[i].duration/15;
                StatusBuildUpDisplay((EnumLib.Status)i,1f,index);
                StatusDisplayTick((EnumLib.Status)i,ratio,1f,index);
            }
            else
            {
                ratio = (float)_playerStatus.activeStatuses[i].statusBuildUp/_playerStatus.statusBuildUps[i].maxBuildUp;
                Debug.Log("Displaying filling "+((EnumLib.Status)i).ToString()+" status on mini: "+ratio);
                StatusBuildUpDisplay((EnumLib.Status)i,ratio,index);
                StatusDisplayTick((EnumLib.Status)i,0,ratio,index);
            }
        }
    }

    public void TriggerPauseScreen(bool value)
    {
        _pauseScreen.SetActive(value);
    }

    public void TriggerVictoryScreen(bool value)
    {
        _victoryScreen.SetActive(value);
    }

    public void TriggerChallengeScreen(bool value)
    {
        _challengeScreen.SetActive(value);
    }

    public void TriggerGameOverScreen(bool value)
    {
        _gameOverScreen.SetActive(value);
    }


}