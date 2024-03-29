using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkDice : AbilityDice
{
    public void Berserk(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage((c.physicalAttack+c.Buffed)*4);
    }

    public void Miss(Character target)
    {
        target.Damage(0);
    }
}