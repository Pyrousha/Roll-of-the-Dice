using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructDice : AbilityDice
{
    public void SelfDestruct(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(c.health*5);
        c.Damage(c.health);
    }

    public void ControlledExplosion(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(c.health*5);
    }
}