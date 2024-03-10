using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerData _playerStat;
    [FormerlySerializedAs("_playerManager")] public Player player;
    public MMFeedbacks _mmFeedbacks;
    public Transform _playerCastPoint;
    
    Transform _playerTransform;

    [SerializeField] float _timeScale;
    [SerializeField] int _frameRate;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one GameManager instance!" + gameObject.name + " is being destroyed!");
            Destroy(gameObject);
        }

        Instance = this;
        
        Enemy.ActiveEnemies.Clear();
        EventManager<EventArgs>.ClearAllEvents();
    }

    void Start()
    {
        Application.targetFrameRate = _frameRate;
        Time.timeScale = _timeScale;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        Application.targetFrameRate = 1;
    }
    public void ResumeGame()
    {
        Time.timeScale = _timeScale;
        Application.targetFrameRate = _frameRate;
    }
   
    public void RegisterPlayer(Transform playerTransform)
    {
        this._playerTransform = playerTransform;
    }
    public void SetPlayerStats(PlayerHealth playerHealth)
    {
        if (playerHealth != null && _playerStat != null)
        {
            playerHealth.SetPlayerStatForHealth(_playerStat);
        }
        else
        {
            Debug.LogError("Failed to set player's stats. PlayerHealth is: " + playerHealth + ", PlayerStatSO is: " + _playerStat);
        }
    }
    

    public Transform GetPlayerTransform()
    {
        return _playerTransform;
    }
    
    // Method to detect mouse position in world space
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            mouseWorldPosition = hit.point;
        }
        return mouseWorldPosition;
    }
   
}
