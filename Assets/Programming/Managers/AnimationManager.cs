using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator mainAnimator;
    Dictionary<string, float> animationDictionary = new Dictionary<string, float>();

    void Start()
    {
        mainAnimator = GetComponent<Animator>();
        GetAnimationClipsFromAnimator();
    }

    public void Play(string animationName)
    {
        mainAnimator.Play(animationName);
    }

    public void PlayWithParameter(string animationName, string parameterName, object parameterValue)
    {
        SetParameter(parameterName, parameterValue);
        mainAnimator.Play(animationName);
    }

    public void PlayByFrame(string animationName, float normalizedTime)
    {
        mainAnimator.Play("Base Layer." + animationName, 0, normalizedTime);
    }

    public void SetParameter(string parameterName, object value)
    {
        switch (parameterName)
        {
            case "Velocity":
            case "HorizontalMovement":
            case "VerticalMovement":
                mainAnimator.SetFloat(parameterName, (float)value);
                break;
            case "Attacking":
            case "ReduceHealth":
                mainAnimator.SetBool(parameterName, (bool)value);
                break;
            case "HurtAmount":
                mainAnimator.SetInteger(parameterName, (int)value);
                break;
            default:
                break;
        }
    }

    public void GetAnimationClipsFromAnimator()
    {
        foreach (AnimationClip animationClip in mainAnimator.runtimeAnimatorController.animationClips)
        {
            if (animationDictionary.ContainsKey(animationClip.name) == false)
            {
                animationDictionary.Add(animationClip.name, animationClip.length);
            }
        }
    }

    public float GetAnimationLength(string animationName)
    {
        if (animationDictionary.ContainsKey("animationName"))
        {
            return animationDictionary[animationName];
        }
        return 0.0f;
    }

    public void AttackEvent(string attackEventParameter)
    {
        if (attackEventParameter == "on")
        {
            SetParameter("Attacking", true);
        }
        else if (attackEventParameter == "off")
        {
            SetParameter("Attacking", false);
        }
    }

    public void CombatEvent(string combatEventParameter)
    {
        var combatManager = GetComponent<CombatManager>();
        if (combatEventParameter == "hit")
        {
            if (combatManager.characterInfo is MasterKnightInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCombat.Invoke(combatManager.currentTarget);
                // combat particles
            }
        }
        else if (combatEventParameter == "end")
        {
            if (combatManager.characterInfo is MasterKnightInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.managingMove = false;
                Play("Default");
            }
            else
            {
                combatManager.enemyIsAttacking = false;
                Play("CombatIdle"); //<- this is causing Enemies stop after CombatHit
            }
        }
    }

    public void CounterEvent(string counterEventParameter)
    {
        var combatManager = GetComponent<CombatManager>();
        if (counterEventParameter == "start")
        {
            if (combatManager.characterInfo is MasterKnightInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCounter.Invoke(combatManager.currentTarget);
            }
        }
        else if (counterEventParameter == "end")
        {
            if (combatManager.characterInfo is MasterKnightInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.managingMove = false;
                combatManager.managingAttack = false;
                combatManager.isCombating = false;
                Play("Default");
            }
            else
            {
                combatManager.enemyIsStunned = false;
                GetComponent<PersonInfo>().CombatReduceHealth(10);
                if (combatManager.characterInfo.isDead) { combatManager.enemyIsAttackable = false; return; }
                Play("CombatIdle");
            }
        }
    }
}