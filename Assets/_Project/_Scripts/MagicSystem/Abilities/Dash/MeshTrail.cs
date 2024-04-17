using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshTrail : MonoBehaviour
{
    [FormerlySerializedAs("AbilitySO")] public AbilityData abilityData;
    public Material _mat;
    
    float _activeTime = 0.25f;
    float _meshRefreshRate = 0.05f;
    float _activeTimeAbilitySO;

    string _shaderVarRef = "_Alpha";
    float _shaderVarRate = 0.1f;
    float _shaderVarRefreshRate = 0.05f;
    float _elementalTrailRefreshRate;


    
    bool _isTrailActive = false;
    
    SkinnedMeshRenderer[] _skinnedMeshRenderer;
    MeshTrail _component;
    int _numberOfCharges = 1;
    [SerializeField] GameObject _firePrefab;

    void Start()
    {
        _shaderVarRef = abilityData.ShaderVarRef;
        _shaderVarRate = abilityData.ShaderVarRate;
        _shaderVarRefreshRate = abilityData.ShaderVarRefreshRate;
        _activeTimeAbilitySO = abilityData.TrailVisibleTime;
        _meshRefreshRate = abilityData.TrailRefreshRate;
        _elementalTrailRefreshRate = abilityData.ElementTrailRefreshRate;
        _mat = abilityData.AbilityMaterial;
        _component = this.GetComponent<MeshTrail>();
        if (!_isTrailActive)
        {
            _isTrailActive = true;
            StartCoroutine(ActivateTrail(_activeTime));
            StartCoroutine(ActivateElementTrail(_activeTime));
        }
         
    }
    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= _meshRefreshRate;
            _activeTime -= _meshRefreshRate; 
            _activeTimeAbilitySO -= _meshRefreshRate;
            
            if (_skinnedMeshRenderer == null)
            {
                _skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
            }
            
            for (int i = 0; i < _skinnedMeshRenderer.Length; i++)
            {
                
                GameObject gameObject = new GameObject();
                gameObject.transform.SetPositionAndRotation(Player.Instance.transform.position, Player.Instance.transform.rotation);

                MeshRenderer mR = gameObject.AddComponent<MeshRenderer>();
                MeshFilter mF = gameObject.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                _skinnedMeshRenderer[i].BakeMesh(mesh);
                
                mF.mesh = mesh;
                mR.material = _mat;

                StartCoroutine(AnimateMaterialFloat(mR.material, 0, _shaderVarRate, _shaderVarRefreshRate));
                
                Destroy(gameObject, _activeTimeAbilitySO);
                Destroy(mesh, _activeTimeAbilitySO);
            }
            
            /*if (abilityData.IsFireDash)
            {
                float spacing = 100f;
                Vector3 direction = -Player.Instance.transform.forward;
                for (int i = 0; i < _numberOfCharges; i++)
                {
                    Vector3 offsetPosition = transform.position + new Vector3(0, -0.75f, 0) + direction * spacing * i;
                        
                    GameObject fireObject = ObjectPoolManager.Instance.SpawnStatusEffect(
                        abilityData.FirePrefab,
                        null,
                        offsetPosition,
                        Quaternion.identity,
                        ObjectPoolManager.PoolType.StatusEffects);
                    
                        
                    //fireObject.transform.SetPositionAndRotation(Player.Instance.transform.position, Player.Instance.transform.rotation);
                        
                    CoroutineManager.Instance.StartManagedCoroutine(ReturnStatusEffectToPoolAfterDelay(10f, fireObject));
                }
            }*/
            
            yield return new WaitForSeconds(_meshRefreshRate);
        }
        if (_activeTime <= 0)
        {
            
            Destroy(_component);
        }
        
        _isTrailActive = false;
    }

    IEnumerator ActivateElementTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= _elementalTrailRefreshRate;
            _activeTime -= _elementalTrailRefreshRate; 
            _activeTimeAbilitySO -= _elementalTrailRefreshRate;
            
            if (abilityData.IsFireDash)
            {
                float spacing = 100f;
                Vector3 direction = -Player.Instance.transform.forward;
                for (int i = 0; i < _numberOfCharges; i++)
                {
                    Vector3 offsetPosition = transform.position + new Vector3(0, -0.75f, 0) + direction * spacing * i;
                        
                    GameObject fireObject = ObjectPoolManager.Instance.SpawnStatusEffect(
                        abilityData.FirePrefab,
                        null,
                        offsetPosition,
                        Quaternion.identity,
                        ObjectPoolManager.PoolType.StatusEffects);
                    
                        
                    //fireObject.transform.SetPositionAndRotation(Player.Instance.transform.position, Player.Instance.transform.rotation);
                        
                    CoroutineManager.Instance.StartManagedCoroutine(ReturnStatusEffectToPoolAfterDelay(10f, fireObject));
                }
            }
            
            yield return new WaitForSeconds(_elementalTrailRefreshRate);
        }
     
        _isTrailActive = false;
    }
    
    IEnumerator ReturnStatusEffectToPoolAfterDelay(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnStatusEffectToPool(objectToReturn);
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(_shaderVarRef);
        
        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(_shaderVarRef, valueToAnimate);
        
            yield return new WaitForSeconds(refreshRate);
        }
    }

}
