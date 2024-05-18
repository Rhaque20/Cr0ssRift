using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [SerializeField]private PlayerBars _playerBars;

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


}