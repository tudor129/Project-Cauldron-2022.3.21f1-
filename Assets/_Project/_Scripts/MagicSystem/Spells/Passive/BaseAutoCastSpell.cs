using UnityEngine;
public class BaseAutoCastSpell : Spell
{
    [SerializeField] protected GameObject[] _effectsOnCollision;
    [SerializeField] protected float _destroyTimeDelay = 5;
    [SerializeField] protected bool _useWorldSpacePosition;
    [SerializeField] protected float _offset = 0;
    [SerializeField] protected Vector3 _rotationOffset = new Vector3(0,0,0);
    [SerializeField] protected bool _useOnlyRotationOffset = true;
    [SerializeField] protected bool _useFirePointRotation;
    [SerializeField] protected bool _destoyMainEffect = false;


    protected float _currentCooldown;
}
