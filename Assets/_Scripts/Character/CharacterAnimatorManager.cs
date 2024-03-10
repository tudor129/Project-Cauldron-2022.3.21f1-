using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    [NonSerialized] public Animator _animator;
    [NonSerialized] public AnimatorOverrideController _animatorOverrideController;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayWalkingAnimation()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsWalking", true);
        }
    }
    public void StopWalkingAnimation()
    {
        _animator.SetBool("IsWalking", false);
    }
    public void PlayAttackAnimation()
    {
        _animator.SetBool("IsAttacking", true);
    }
    public void StopAttackAnimation()
    {
        _animator.SetBool("IsAttacking", false);
    }
    
    public void PlayTakeDamageAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("TakeDamage");
        }
        else
        {
            Debug.LogError("Animator is null!");
        }
    }

    public void PlayDeathAnimation()
    {
        _animator.SetBool("Die", true);
    }
    
    public void Revive()
    {
        _animator.SetBool("Die", false);
        _animator.SetTrigger("Revive");
    }
    
}
