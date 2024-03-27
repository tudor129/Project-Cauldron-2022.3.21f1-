using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TransformMotion : MonoBehaviour
{
    public int AttackDamage { get; set; }

    
    [SerializeField] float Distance = 30;
    [SerializeField] float Speed = 1;
    [SerializeField] float TimeDelay = 0;
    [SerializeField] float RandomMoveRadius = 0;
    [SerializeField] float RandomMoveSpeedScale = 0;
    [SerializeField] int _maxHitCount = 5; // Maximum number of hits before stopping projectile
    int _currentHitCount = 0; // Track the current hit count
    public GameObject Target;
 
    public LayerMask CollidesWith = ~0;
   
   
    public GameObject[] EffectsOnCollision;
    public float CollisionOffset = 0;
    public float DestroyTimeDelay = 5;
    public bool CollisionEffectInWorldSpace = true;
    public GameObject[] DeactivatedObjectsOnCollision;
    [HideInInspector] public float HUE = -1;
    [HideInInspector] public List<GameObject> CollidedInstances;

    Vector3 _startPosition;
    Vector3 _startPositionLocal;
    Transform _t;
    Transform _targetT;
    Vector3 _oldPos;
    bool _isCollided;
    bool _isOutDistance;
    Quaternion _startQuaternion;
    float _currentDelay;
    const float _RayCastTolerance = 0.15f;
    bool _isInitialized;
    bool _dropFirstFrameForFixUnityBugWithParticles;
    
    public event EventHandler<RFX1_CollisionInfo> CollisionEnter;
    
    Vector3 _randomTimeOffset;
    int _attackDamage;

    void Start()
    {
        _t = transform;
        if (Target != null) _targetT = Target.transform;
        _startQuaternion = _t.rotation;
        _startPositionLocal = _t.localPosition;
        _startPosition = _t.position;
        _oldPos = _t.TransformPoint(_startPositionLocal);
        Initialize();
        _isInitialized = true;
        _currentHitCount = 0;
    }

    void OnEnable()
    {
        if (_isInitialized) Initialize();
    }

    void OnDisable()
    {
        if (_isInitialized) Initialize();
    }

    void Initialize()
    {
        _isCollided = false;
        _isOutDistance = false;
        //currentSpeed = Speed;
        _currentDelay = 0;
        _startQuaternion = _t.rotation;
        _t.localPosition = _startPositionLocal;
        _oldPos = _t.TransformPoint(_startPositionLocal);
        OnCollisionDeactivateBehaviour(true);
        _dropFirstFrameForFixUnityBugWithParticles = true;
        _randomTimeOffset = Random.insideUnitSphere * 10;
        _currentHitCount = 0;
    }

    void Update()
    {
        if (!_dropFirstFrameForFixUnityBugWithParticles)
        {
            UpdateWorldPosition();
        }
        else _dropFirstFrameForFixUnityBugWithParticles = false;
    }

    void UpdateWorldPosition()
    {
        _currentDelay += Time.deltaTime;
        if (_currentDelay < TimeDelay)
            return;

        Vector3 randomOffset = Vector3.zero;
        if (RandomMoveRadius > 0)
        {
        
            randomOffset = GetRadiusRandomVector() * RandomMoveRadius;
            if (Target != null)
            {
                if(_targetT==null) _targetT = Target.transform;
                var fade = Vector3.Distance(_t.position, _targetT.position) / Vector3.Distance(_startPosition, _targetT.position);
                randomOffset *= fade;
            }
        }

        var frameMoveOffset = Vector3.zero;
        var frameMoveOffsetWorld = Vector3.zero;
        if (!_isCollided && !_isOutDistance)
        {
            if (Target == null)
            {
                var currentForwardVector = (Vector3.forward + randomOffset)* Speed * Time.deltaTime;
                frameMoveOffset = _t.localRotation*currentForwardVector;
                frameMoveOffsetWorld = _startQuaternion*currentForwardVector;
            }
            else
            {
                var forwardVec = (_targetT.position - _t.position).normalized;
                var currentForwardVector = (forwardVec + randomOffset) * Speed * Time.deltaTime;
                frameMoveOffset = currentForwardVector;
                frameMoveOffsetWorld = currentForwardVector;
            }
        }

        var currentDistance = (_t.localPosition + frameMoveOffset - _startPositionLocal).magnitude;
        
        
        RaycastHit[] hits;
        if (!_isCollided && (hits = Physics.RaycastAll(_t.position, frameMoveOffsetWorld.normalized, Distance, CollidesWith)).Length > 0)
        {
            foreach (var hit in hits)
            {
                if (frameMoveOffset.magnitude + _RayCastTolerance > hit.distance)
                {
                    _t.position = hit.point;
                    _oldPos = _t.position;
                    OnCollisionBehaviour(hit);
                    OnCollisionDeactivateBehaviour(false);
                    return;
                }
            }
        }
      
        if (!_isOutDistance && currentDistance + _RayCastTolerance > Distance)
        {
            _isOutDistance = true;
            OnCollisionDeactivateBehaviour(false);
            ObjectPoolManager.Instance.ReturnParentObjectToPool(gameObject);

            if (Target == null)
                _t.localPosition = _startPositionLocal + _t.localRotation*(Vector3.forward + randomOffset)*Distance;
            else
            {
                var forwardVec = (_targetT.position - _t.position).normalized;
                _t.position = _startPosition + forwardVec * Distance;
            }
            _oldPos = _t.position;
            return;
        }
      
        _t.position = _oldPos + frameMoveOffsetWorld;
        _oldPos = _t.position;
    }

    Vector3 GetRadiusRandomVector()
    {
        var x = Time.time * RandomMoveSpeedScale + _randomTimeOffset.x;
        var vecX = Mathf.Sin(x / 7 + Mathf.Cos(x / 2)) * Mathf.Cos(x / 5 + Mathf.Sin(x));

        x = Time.time * RandomMoveSpeedScale + _randomTimeOffset.y;
        var vecY = Mathf.Cos(x / 8 + Mathf.Sin(x / 2)) * Mathf.Sin(Mathf.Sin(x / 1.2f) + x * 1.2f);

        x = Time.time * RandomMoveSpeedScale + _randomTimeOffset.z;
        var vecZ = Mathf.Cos(x * 0.7f + Mathf.Cos(x * 0.5f)) * Mathf.Cos(Mathf.Sin(x * 0.8f) + x * 0.3f);


        return new Vector3(vecX, vecY, vecZ);
    }
    
    // int CalculateAttackDamage(int hitCount)
    // {
    //     // Set base damage
    //     int baseDamage = 30;
    //     // Subtract 10 damage for each subsequent hit (without going below 0)
    //     int damageSubtraction = 10 * (hitCount -1);
    //     // Calculate the actual attack damage
    //     AttackDamage = Math.Max(0, baseDamage - damageSubtraction);
    //     return AttackDamage;
    // }
    
    void OnCollisionBehaviour(RaycastHit hit)
    {
        var handler = CollisionEnter;
        if (handler != null)
            handler(this, new RFX1_CollisionInfo {Hit = hit});
        CollidedInstances.Clear();
        // Damage the enemy
        var attackable = hit.collider.gameObject.GetComponent<IAttackable>();
        if (attackable != null)
        {
            // Increase the hit count
            _currentHitCount++;

            // (int damage, bool isCritical) = SpellSO.CalculateAttackDamage(_currentHitCount, 0.3f, 1.5f);
            //
            // attackable.TakeDamage(damage, isCritical);

            if (_currentHitCount >= _maxHitCount)
            {
                foreach (var effect in EffectsOnCollision)
                {
                    //var instance = Instantiate(effect, hit.point + hit.normal * CollisionOffset, new Quaternion()) as GameObject;
                    var instance = ObjectPoolManager.Instance.SpawnObject(effect, hit.point + hit.normal * CollisionOffset, new Quaternion(), ObjectPoolManager.PoolType.GameObject);
                    CollidedInstances.Add(instance);
                    if (HUE > -0.9f)
                    {
                        // var color = instance.AddComponent<RFX1_EffectSettingColor>();
                        // var hsv = RFX1_ColorHelper.ColorToHSV(color.Color);
                        // hsv.H = HUE;
                        // color.Color = RFX1_ColorHelper.HSVToColor(hsv);
                    }
                    instance.transform.LookAt(hit.point + hit.normal + hit.normal * CollisionOffset);
                    if (!CollisionEffectInWorldSpace) instance.transform.parent = transform;
                    CoroutineManager.Instance.StartCoroutine(ReturnChildToPoolAfterDelay(instance, CollidedInstances, DestroyTimeDelay, ObjectPoolManager.PoolType.GameObject, 1));
                    //Destroy(instance, DestroyTimeDelay);
                }
                ObjectPoolManager.Instance.ReturnParentObjectToPool(gameObject);
                _currentHitCount = 0;
            }
        }
    }
    
    IEnumerator ReturnChildToPoolAfterDelay(GameObject obj, List<GameObject> children, float delay, ObjectPoolManager.PoolType poolType, int childIndex)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Return to pool has been called ha bibi");
        // Make sure the child index is valid.
        if (obj != null && childIndex < obj.transform.childCount)
        {
            GameObject child = obj.transform.GetChild(childIndex).gameObject;
            ObjectPoolManager.Instance.ReturnParentObjectToPool(child);
            Debug.Log("Return to pool has been called bibi");
        }
        else
        {
            Debug.LogError("Invalid child index or parent object is null.");
        }
    }
    
    void OnCollisionDeactivateBehaviour(bool active)
    {
        foreach (var effect in DeactivatedObjectsOnCollision)
        {
           if(effect!=null) effect.SetActive(active);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        _t = transform;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_t.position, _t.position + _t.forward*Distance);
        
    }

    public enum RFX4_SimulationSpace
    {
        Local,
        World
    }

    public class RFX1_CollisionInfo : EventArgs
    {
        public RaycastHit Hit;
    }
}