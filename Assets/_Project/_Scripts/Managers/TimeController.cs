using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }
    
    public Action OnSunrise;
    public Action OnSunset;
    public Action<TimeState> OnTimeStateChanged;
    public enum TimeState
    {
        EarlyMorning, // 6am - 9am
        Morning, // 9am - 12pm
        EarlyAfternoon, // 12pm - 3pm
        Afternoon, // 3pm - 6pm
        Evening, // 6pm - 9pm
        LateEvening, // 9pm - 12am
        Midnight, // 12am - 3am
        LateNight, // 3am - 6am
    }
    public TimeState CurrentTimeState { get; private set; }
    
    [Tooltip("Time multiplier for debugging purposes.")]
    [Range(100, 8000)]
    [SerializeField] float _timeMultiplier;
    [SerializeField] float _startHour;
    [SerializeField] TextMeshProUGUI _timetext;
    [SerializeField] TextMeshProUGUI _dateText;
    [SerializeField] Light _sunLight;
    [SerializeField] int _sunriseHour;
    [SerializeField] int _sunsetHour;
    [SerializeField] Color _dayAmbientLight;
    [SerializeField] Color _nightAmbientLight;
    [SerializeField] AnimationCurve _lightChangeCurve;
    [SerializeField] float _maxSunlightIntensity;
    [SerializeField] Light _moonLight;
    [SerializeField] float _maxMoonLightIntensity;
    [SerializeField] Cauldron _cauldron;
    [SerializeField] RenderSettings _renderSettings;
    
    DateTime _currentTime;
    DateTime _currentDate;
    
    TimeSpan _sunriseTime;
    TimeSpan _sunsetTime;
    
    float _sunLightRotationY;
    int _previousHour;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);

    }

    void Start()
    {
        UpdateDate();
        
        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);
        
        OnSunrise += () => Debug.Log("Sunrise event triggered");
        OnSunset += () => Debug.Log("Sunset event triggered");
    }

    void Update()
    {
        int previousHour = _currentTime.Hour;
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateDate();
        UpdateTimeState();

        if (previousHour != _currentTime.Hour)  // Check if the hour has changed
        {
            if (_currentTime.Hour == _sunriseHour)
            {
                OnSunrise?.Invoke();
            }
            else if (_currentTime.Hour == _sunsetHour)
            {
                OnSunset?.Invoke();
            }
        }
    }

    void UpdateTimeOfDay()
    {
        _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
        
        if (_timetext != null)
        {
            _timetext.text = _currentTime.ToString("HH:mm");
        }
    }
    
    void UpdateDate()
    {
        if (_currentDate != _currentTime.Date)  // Check if a new day has started
        {
            _currentDate = _currentTime.Date;  // Update _currentDate to the new date
            
            if (_dateText != null)
            {
                _dateText.text = _currentTime.ToString("dddd, MMMM dd, yyyy");
            }
        }
    }

    void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
        _sunLight.intensity = Mathf.Lerp(0, _maxSunlightIntensity, _lightChangeCurve.Evaluate(dotProduct));
        _moonLight.intensity = Mathf.Lerp(_maxMoonLightIntensity, 0, _lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(_nightAmbientLight, _dayAmbientLight, _lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientIntensity = Mathf.Lerp(0f, 0.5f, _lightChangeCurve.Evaluate(dotProduct));
    }

    void RotateSun()
    {
        float sunLightRotation;
        
        if (_currentTime.TimeOfDay >= _sunriseTime && _currentTime.TimeOfDay <= _sunsetTime)
        {
            TimeSpan sunRiseToSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _currentTime.TimeOfDay);
            
            double percentage = timeSinceSunrise.TotalMinutes / sunRiseToSunsetDuration.TotalMinutes;
            
            sunLightRotation = Mathf.Lerp(0, 180, (float) percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(_currentTime.TimeOfDay, _sunsetTime);
            
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            
            sunLightRotation = Mathf.Lerp(180, 360, (float) percentage);
        }
        
        //_sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
        _sunLight.transform.rotation = Quaternion.Euler(sunLightRotation, -82f, 0f);
    }

    TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;
        
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
    void UpdateTimeState()
    {
        int hour = _currentTime.Hour;
        if (hour >= 6 && hour < 9)
        {
            ChangeTimeState(TimeState.EarlyMorning);
        }
        else if (hour >= 9 && hour < 12)
        {
            ChangeTimeState(TimeState.Morning);
        }
        else if (hour >= 12 && hour < 15)
        {
            ChangeTimeState(TimeState.EarlyAfternoon);
        }
        else if (hour >= 15 && hour < 18)
        {
            ChangeTimeState(TimeState.Afternoon);
        }
        else if (hour >= 18 && hour < 21)
        {
            ChangeTimeState(TimeState.Evening);
        }
        else if (hour >= 21 && hour < 24)
        {
            ChangeTimeState(TimeState.LateEvening);
        }
        else if (hour >= 0 && hour < 3)
        {
            ChangeTimeState(TimeState.Midnight);
        }
        else if (hour >= 3 && hour < 6)
        {
            ChangeTimeState(TimeState.LateNight);
        }
    }
    
    void ChangeTimeState(TimeState newState)
    {
        if (CurrentTimeState != newState)
        {
            CurrentTimeState = newState;
            OnTimeStateChanged?.Invoke(newState);
        }
    }

    
    public bool IsDayTime()
    {
        return _currentTime.Hour >= _sunriseHour && _currentTime.Hour < _sunsetHour;
    }
    
    public bool IsNightTime()
    {
        return !IsDayTime();
    }

}
