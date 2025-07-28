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

    public void PlayByFrame(string animationName, float normalizedTime)
    {
        mainAnimator.Play("Base Layer." + animationName, 0, normalizedTime);
    }

    public void SetParameter(string parameterName, object value)
    {
        switch (parameterName)
        {
            case "Velocity":
                mainAnimator.SetFloat(parameterName, (float)value);
                break;
            case "Attacking":
                mainAnimator.SetBool(parameterName, (bool)value);
                break;
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
            Enemy eventTarget = combatManager.currentTarget;
            eventTarget.personInfo.CombatReduceHealth(50);
            eventTarget.hurtFrames = 0;
            eventTarget.Hurt();
            combatManager.isCombating = false;
        }
        else if (combatEventParameter == "hurt")
        {
            
        }
        else if (combatEventParameter == "end")
        {
            combatManager.managingMove = false;
            Play("Default");
        }
    }
}