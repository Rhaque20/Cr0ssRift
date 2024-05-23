using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]Transform _barriers;
    [SerializeField]GameObject[] _enemyWaves = new GameObject[0];

    public static LevelManager instance;

    [SerializeField]int _currentWaveCount = 0, _currentWave = 0;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentWaveCount = _enemyWaves[_currentWave].transform.childCount;
    }

    public void DecreaseTally()
    {
        _currentWaveCount--;
        if (_currentWaveCount == 0)
        {
            if(_currentWave < _barriers.childCount)
            {
                _barriers.GetChild(_currentWave).GetComponent<BoxCollider>().isTrigger = true;
                _currentWave++;
                if (_currentWave < _enemyWaves.Length)
                    _currentWaveCount = _enemyWaves[_currentWave].transform.childCount;
            }
            
                
        }
    }

    public void SpawnWave(int index)
    {
        _enemyWaves[index].SetActive(true);
    }
}
