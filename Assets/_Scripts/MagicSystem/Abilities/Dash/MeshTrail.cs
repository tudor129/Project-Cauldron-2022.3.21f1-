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
    

    
    bool _isTrailActive = false;
    
    SkinnedMeshRenderer[] _skinnedMeshRenderer;
    MeshTrail _component;
    
    void Start()
    {
        _shaderVarRef = abilityData.ShaderVarRef;
        _shaderVarRate = abilityData.ShaderVarRate;
        _shaderVarRefreshRate = abilityData.ShaderVarRefreshRate;
        _activeTimeAbilitySO = abilityData.TrailVisibleTime;
        _meshRefreshRate = abilityData.TrailRefreshRate;
        _mat = abilityData.AbilityMaterial;
        _component = this.GetComponent<MeshTrail>();
        if (!_isTrailActive)
        {
            _isTrailActive = true;
            StartCoroutine(ActivateTrail(_activeTime)); 
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
            
            yield return new WaitForSeconds(_meshRefreshRate);
        }
        if (_activeTime <= 0)
        {
            
            Destroy(_component);
        }
        
        _isTrailActive = false;
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
