using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text _timerText = null;
    
    private bool _timeIsRunning = false;

    public Stopwatch StopWatch { get; private set; } = null;

    private void Start()
    {
        _timeIsRunning = false;
    }

    private void Update()
    {
        if(_timeIsRunning)
        {
            _timerText.text = GetCurrentTime();
        }
    }

    public void StartTimer()
    {
        if (StopWatch == null)
        {
            StopWatch = new Stopwatch();
        }

        StopWatch.Start();
        _timeIsRunning = true;
    }

    public void StopTimer()
    {
        if(StopWatch == null)
        {
            return;
        }

        StopWatch.Stop();
        _timeIsRunning = false;
    }

    public void ResetTimer()
    {
        if(StopWatch == null)
        {
            return;
        }

        StopWatch.Reset();
        _timeIsRunning = false;
        _timerText.text = GetCurrentTime();
    }

    public string GetCurrentTime()
    {
        if(StopWatch == null)
        {
            return "00:00:00";
        }

        var elapsedTime = StopWatch.Elapsed;
        return string.Format("{0:00}:{1:00}:{2:00}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
    }
}
