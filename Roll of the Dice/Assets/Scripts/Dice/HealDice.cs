using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDice : AbilityDice
{
    public void MinorHeal(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(-c.magicAttack/2);
        c.Damage(-c.magicAttack/4);
    }

    public void Heal(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(-c.magicAttack);
        c.Damage(-c.magicAttack/2);
    }

    public void MajorHeal(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(-c.magicAttack*2);
        c.Damage(-c.magicAttack);
    }
}