using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]Transform _barriers;
    [SerializeField]Transform _enemyWaves;
    [SerializeField]Transform _battleZoneParent;
    BattleZones[] _battleZones;
    public static LevelManager instance;

    [SerializeField]int _currentWaveCount = 0, _currentWave = 0;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _battleZones = new BattleZones[_battleZoneParent.childCount];

        for(int i = 0; i < _battleZoneParent.childCount; i++)
        {
            _battleZones[i] = _battleZoneParent.GetChild(i).GetComponent<BattleZones>();
        }
        _currentWaveCount = _enemyWaves.GetChild(_currentWave).transform.childCount;

        if(_currentWaveCount == 0)
        {
            DecreaseTally();
        }
    }

    public void DecreaseTally()
    {
        _currentWaveCount--;
        if (_currentWaveCount <= 0)
        {
            _battleZones[_currentWave].Release();  
        }
    }

    public void SpawnWave(int index)
    {
        if (index < 0)
            return;
        
        _enemyWaves.GetChild(index).gameObject.SetActive(true);
        _currentWaveCount = _enemyWaves.GetChild(index).childCount;
        _currentWave = index;
        _battleZones[index].LockDown();
    }
}
