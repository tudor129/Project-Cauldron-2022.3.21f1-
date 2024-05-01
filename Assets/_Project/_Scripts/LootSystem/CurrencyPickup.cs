using UnityEngine;
using DG.Tweening;
using System; 

public class CurrencyPickup : MonoBehaviour
{
    public enum CurrencyType { Coin, Gem }

    public CurrencyType currencyType;
    public int amount = 1;
    float _magnetRadius = 1f;

    bool _isBeingAttracted = false;
    Transform _playerTransform;
    Player _player;
    bool _isInitialized;


    void OnEnable()
    {
        _player = Player.Instance;

        transform.localScale = new Vector3(4, 4, 4);
        
        
    }

    void Start()
    {
        // Create trigger collider for magnet attraction.
        SphereCollider magnetCollider = gameObject.AddComponent<SphereCollider>();
        magnetCollider.radius = _magnetRadius;
        magnetCollider.isTrigger = true;
        
        //AttractToPlayer();
        
    }
    
    public void Initialize()
    {
        if (_isInitialized) return;  // Prevent re-initialization

        
      
        _isInitialized = true;

        

        // Perform any operations previously in Awake or Start that depend on initialization here
        PostInitialization();
    }

    void PostInitialization()
    {
        //StartCoroutine(DespawnAfterDelay(_currentStats.ProjectileLifetime));
    }

   
    
    

    void Update()
    {
        if (_isBeingAttracted)
        {
            AttractToPlayer();
        
            // Check if the item is close enough to the player
            if (Vector3.Distance(transform.position, _playerTransform.position) <= 1f) 
            {
                HandlePickup(_player);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !_isBeingAttracted)
        {
            _playerTransform = player.transform;
            _isBeingAttracted = true;
        }
    }


    void AttractToPlayer()
    {
        
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        float duration = Mathf.Clamp(distance / 4f, 0.1f, 1f); // Adjust the divisor to your needs
        gameObject.transform.DOMove(_playerTransform.position, duration);

        gameObject.transform.DOScale(1.5f, duration / 2);
        
        gameObject.transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

    }

    void HandlePickup(Player player)
    {
        switch (currencyType)
        {
            case CurrencyType.Coin:
                CurrencyManager.Instance.AddCoins(amount);
                break;
            case CurrencyType.Gem:
                CurrencyManager.Instance.AddGems(amount);
                ExperienceManager.Instance.AddExperience(10);
                break;
        }
        transform.DOKill();
        if(transform.parent != null) // check if there is a parent object
        {
            //Destroy(transform.parent.gameObject);
            //ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            ObjectPoolManager.Instance._gameObjectPool.Release(gameObject);
            
        }
        else
        {
            // Destroy the current object if there's no parent
            //Destroy(gameObject);
            //ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            ObjectPoolManager.Instance._gameObjectPool.Release(gameObject);
        }
        _isBeingAttracted = false;
    }
    
    
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _magnetRadius);
    }
}
