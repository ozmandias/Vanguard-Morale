using UnityEngine;

public class Boss : Person
{
    public delegate void SpecialAbilityDelegate();
    public SpecialAbilityDelegate OnSpecialAbility;
}