using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private bool _startTimer;

    [SerializeField] private float _timer = 0;
    [SerializeField] private float _lastTime = 0;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _savedTime;
    [SerializeField] private TextMeshProUGUI _congratsText;
    

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_startTimer)
        {
            _timer += Time.deltaTime;
            _timerText.text = $"{_timer:F2}";
        }
        
    }

    public void StartTimer()
    {
        _startTimer = true;
    }

    public void StopTimer()
    {
        _startTimer = false;
        _lastTime = _timer;
        _savedTime.text = $"SAVED TIME: {_lastTime:F2}";
        _timer = 0;
    }

    public void TimerCongratulazioni()
    {
        StartTimer();
        // while(_timer < 5)
        // {
        //     _congratsText.text = "Congratulazioni";
        // }
        if(_timer < 5)
        {
            _congratsText.text = "Congratulazioni";
        }
        else
        {
            _congratsText.text = " ";
        }
    }
    
    public bool IsTimerStarted()
    {
        return _startTimer;
    }
}
