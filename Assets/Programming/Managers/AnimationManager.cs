using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public Animator mainAnimator;
    public RuntimeAnimatorController playerRuntimeAnimator;
    public RuntimeAnimatorController personRuntimeAnimator;
    Dictionary<string, float> animationDictionary = new Dictionary<string, float>();

    void Start()
    {
        mainAnimator = GetComponent<Animator>();
        GetAnimationClipsFromAnimator();

        // setup RuntimeController for Animator based on being player or npc
        var character = GetComponent<Character>();
        if(character != null) {
            playerRuntimeAnimator = character.personalData.playerRuntimeAnimator;
            personRuntimeAnimator = character.personalData.personRuntimeAnimator;
            if(mainAnimator.gameObject.CompareTag("Player")) {
                mainAnimator.runtimeAnimatorController = playerRuntimeAnimator;
            } else {
                mainAnimator.runtimeAnimatorController = personRuntimeAnimator;
            }
        }
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
        // condition check to set parameters for player and person
        if(gameObject.CompareTag("Player")) {

        } else {
            switch (parameterName)
            {
                case "Velocity":
                case "HorizontalMovement":
                case "VerticalMovement":
                case "AttackNumber":
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
        } else if (attackEventParameter == "off") {
            SetParameter("Attacking", false);
        }
    }

    public void AttackNumberEvent(string attackEventParameter) {
        if(attackEventParameter == "on") {
            SetParameter("Attacking", true);
            if((GetComponent<Person>().GetInfo() as PersonInfo).combatType == CombatType.Range) {
                GetComponent<EffectManager>().attackEffect.canCreateEffect = true;
            }
        } else if(attackEventParameter == "off") {
            Debug.Log("" + gameObject.name + " AttackNumberEvent - off");
            SetParameter("Attacking", false);
            gameObject.GetComponent<Person>().attackNumberUpdate = true;
        }
    }

    public void CombatEvent(string combatEventParameter)
    {
        var combatManager = GetComponent<CombatManager>();
        // late because of "hit", try changing to "start"
        if (combatEventParameter == "hit")
        {
            if (combatManager.characterInfo is VanguardInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCombat.Invoke(combatManager.currentTarget);
                // combat particles
            }
        }
        else if (combatEventParameter == "end")
        {
            if (combatManager.characterInfo is VanguardInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCombatEnd.Invoke(combatManager.currentTarget);
            }
            else
            {
                combatManager.OnEnemyCombatHurt.Invoke((combatManager.characterInfo as PersonInfo).person as Enemy);
            }
        }
    }

    public void CounterEvent(string counterEventParameter)
    {
        var combatManager = GetComponent<CombatManager>();
        if (counterEventParameter == "start")
        {
            if (combatManager.characterInfo is VanguardInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCounter.Invoke(combatManager.currentTarget);
            }
        }
        else if (counterEventParameter == "end")
        {
            if (combatManager.characterInfo is VanguardInfo || combatManager.characterInfo is PlayerInfo)
            {
                combatManager.OnPlayerCounterEnd.Invoke(combatManager.currentTarget);
            }
            else
            {
                combatManager.OnEnemyCounterHurt.Invoke((combatManager.characterInfo as PersonInfo).person as Enemy);
            }
        }
    }
}