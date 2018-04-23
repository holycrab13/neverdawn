using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;
using System;

/// <summary>
/// Time scaling is different from real world time. One full game-day cycle lasts 1,5 hours in real time
/// 
/// 1 day lasts 1h 30m
/// 1 hour lasts 3m 45s
/// 1 min lasts 3s 750ms
/// 1 sec lasts 62.5ms
/// </summary>
public class PlayerTime : FrameComponent {

    /// <summary>
    /// dayTime runs from 0 to 60 * 60 * 1,5 = 5400
    /// </summary>
    [SerializeField]
    private float _dayTimeInRealSeconds;

    [SerializeField]
    private float _timeScale = 16.0f;

    [SerializeField]
    private float _dayTimeInGameHours;

    private static  PlayerTime instance;

    private bool _isRunning;

    public static float dayLengthInSeconds
    {
        get { return 60.0f * 60.0f * 24.0f / instance._timeScale; }
    }

    public static float dayProgress
    {
        get { return instance != null ? instance._dayTimeInRealSeconds / dayLengthInSeconds : 0.0f; }
    }

    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    public void Run()
    {
        _isRunning = true;
    }

    public void Pause()
    {
        _isRunning = false;
    }

    void Update()
    {
        if (_isRunning)
        {
            _dayTimeInRealSeconds = Mathf.Repeat(_dayTimeInRealSeconds + Time.deltaTime, dayLengthInSeconds);
            _dayTimeInGameHours = ScaleTime(_dayTimeInRealSeconds / 3600.0f);
        }
    }

    protected override void readData(IMessageReader reader)
    {
        _dayTimeInGameHours = reader.ReadFloat();
        _dayTimeInRealSeconds = (_dayTimeInGameHours / _timeScale) * 3600.0f;
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteFloat(_dayTimeInGameHours);
    }

    internal static float ScaleTime(float p)
    {
        return p * instance._timeScale;
    }

    public static string GetTimeString()
    {
        if (instance)
        {
            float realSeconds = instance._dayTimeInRealSeconds * instance._timeScale;

            TimeSpan t = TimeSpan.FromSeconds(realSeconds);

            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
        }

        return string.Empty;
    }

    public static void WaitForHours(int hours)
    {
        instance._dayTimeInRealSeconds += ScaleTime(hours * 3600.0f);
    }
}
