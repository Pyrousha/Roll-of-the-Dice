using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissAttackCard : AbilityCard
{
    public override void DoAction(Character target)
    {
        //Character c = GetComponentInParent<Character>();
        target.Damage(0);
    }
}