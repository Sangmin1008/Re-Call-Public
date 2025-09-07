using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    
    public float startTime = 24f;
    [SerializeField]
    private float currentTime;
    
    private bool isRunning = false;
    private float timeScale = 1f;
    public event Action<float> OnTick;
    public event Action OnTimerEnd;

    Action<object> timerInfo;
    private List<float> targetTimes = new List<float>();
    private HashSet<float> triggerdeTimes = new HashSet<float>();
    public float CurrentTime { get { return currentTime; } }

    public float duration = 4f;
    public void Init(float start, List<float> targets)
    {
        startTime = start;
        currentTime = startTime * 60;
        timeScale = 1f;
        isRunning = false;
        targetTimes = targets;
        triggerdeTimes.Clear();
        timerInfo=GameManager.Instance.timerInfo;
    }
    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;

    public void AddTargetTime(float time)
    {
        if(triggerdeTimes.Contains(time))
             return;
        targetTimes.Add(time);
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }
    public string ShowTime()
    {
        string time = "";
        time = $"{(int)(currentTime / 60)}:{(int)(currentTime % 60)}";
        return time;
    }

    private void Update()
    {
        if (!isRunning) return;

        float delta = Time.deltaTime * timeScale*GameManager.GameSpeedFloat;
        currentTime -= delta;
        foreach (int i in targetTimes)
        {
            if (!triggerdeTimes.Contains(i) && currentTime <= i)
            {
                triggerdeTimes.Add(i);
                OnTick?.Invoke(i);
                Debug.Log("목표 도달");
            }
        }
        if (currentTime <= Constants.Normalized_Value_ZERO)
        {
            currentTime = Constants.Normalized_Value_ZERO;
            isRunning = false;
            OnTimerEnd?.Invoke();
        }
        timerInfo?.Invoke(ShowTime());
    }

    public void ReturnTime()
    {
        //currentTime
        DoTweenExtensions.TweenFloat(currentTime, startTime * 60, duration, AddTime);
    }

    public void AddTime(float value)
    {
        currentTime = value;
    }

}