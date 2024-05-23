using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    const int MAINBAR = 0;
    public static PlayerUIManager instance;

    [SerializeField]private PlayerBars[] _playerBars = new PlayerBars[4];
    [SerializeField]private StatusUI _statusUI;

    [SerializeField]private GameObject _gameOverScreen,_pauseScreen;

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
        _statusUI.SetStatusBuildUpDisplay(status,ratio);
    }

    public void StatusDisplayTick(EnumLib.Status status,float ratio)
    {
        _statusUI.SetStatusDisplayTick(status,ratio);
    }

    public void TriggerPauseScreen(bool value)
    {
        _pauseScreen.SetActive(value);
    }

    public void TriggerGameOverScreen(bool value)
    {
        _gameOverScreen.SetActive(value);
    }


}