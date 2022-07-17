using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDice : AbilityDice
{
    public void Buff(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.buffed = c.magicAttack;
    }

    public void Confuse(Character target)
    {
        target.confused = 1;
    }
}