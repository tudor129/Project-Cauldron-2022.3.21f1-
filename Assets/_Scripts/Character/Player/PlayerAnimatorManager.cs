using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    Player _player;
    
    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
    }

    void OnAnimatorMove()
    {
        if (_player._applyRootMotion)
        {
            _player._characterController.Move(_animator.deltaPosition);
        }
    }


    public void UpdateAnimatorMovementParameters(float movementInputZ, float movementInputX)
    {
        _animator.SetFloat("Vertical", movementInputZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("Horizontal", movementInputX, 0.1f, Time.deltaTime);
    }
    public void UpdateAnimatorSpellCastParameters(bool isSpellCasting)
    {
        _animator.SetBool("IsCasting", isSpellCasting);
    }
    
   
    
    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false)
    {
        _player._applyRootMotion = applyRootMotion;
        //_animator.SetBool(targetAnimation, true);
        _animator.CrossFade(targetAnimation, 0.2f);
        // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS WHILE PERFORMING AN ACTION
        // FOR EXAMPLE IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING TO PERFORM A NEW ACTION
        _player._isPerformingAction = isPerformingAction;
        _player._canMove = canMove;
        _player._canRotate = canRotate;
        
        
    }
    
    
}
