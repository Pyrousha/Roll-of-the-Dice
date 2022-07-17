using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDice : AbilityDice
{
    public void Buff(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.SetBuffed(c.magicAttack/2);
    }

    public void Confuse(Character target)
    {
        target.SetConfused(1);
    }
}