using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [SerializeField]private PlayerBars _playerBars;
    [SerializeField]private StatusUI _statusUI;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetPlayerBarsDisplay();
        PlayerPartyManager.instance.onPlayerSwitched += ResetPlayerBarsDisplay;
    }

    void ResetPlayerBarsDisplay()
    {
        PlayerStats stats = PlayerPartyManager.instance.getActivePlayer.GetComponent<PlayerStats>();
        SetHealthBar(stats.currentHP,stats.maxHP);
        SetArmorBar((float)stats.currentArmor/(float)stats.maxArmor);
    }

    public void SetHealthBar(int _currentHP, int _maxHP)
    {
        _playerBars.SetHealthBar(_currentHP, _maxHP);
    }

    public void SetArmorBar(float ratio)
    {
        _playerBars.SetArmorBar(ratio);
    }

    public void SetSPBar(float ratio)
    {
        _playerBars.SetSPBar(ratio);
    }

    public void SetSPDrainBar(float ratio)
    {
        _playerBars.SetSPDrainBar(ratio);
    }
    public void StatusBuildUpDisplay(EnumLib.Status status, float ratio)
    {
        _statusUI.SetStatusBuildUpDisplay(status,ratio);
    }

    public void StatusDisplayTick(EnumLib.Status status,float ratio)
    {
        _statusUI.SetStatusDisplayTick(status,ratio);
    }


}