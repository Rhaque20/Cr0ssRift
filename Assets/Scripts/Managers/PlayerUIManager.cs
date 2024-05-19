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

    public void SetHealthBar(int _currentHP, int _maxHP)
    {
        _playerBars.SetHealthBar(_currentHP, _maxHP);
    }

    public void SetArmorBar(float ratio)
    {
        _playerBars.SetArmorBar(ratio);
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