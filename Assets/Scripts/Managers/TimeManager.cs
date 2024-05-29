using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    // To be used for pausing the game and game over
    private bool _priorityPause = false;
    private Coroutine _hitStopTimer = null;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayerPartyManager.instance.onGameOver += PauseGame;
    }

    private IEnumerator HitStopTimer(float duration)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(duration);

        if (!_priorityPause)
            Time.timeScale = 1.0f;
        
        _hitStopTimer = null;
    }

    public void PauseGame(bool value)
    {
        _priorityPause = value;

        if(_priorityPause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void HitStop(float duration)
    {
        if (_hitStopTimer == null)
        {
            _hitStopTimer = StartCoroutine(HitStopTimer(duration));
        }
    }


}
