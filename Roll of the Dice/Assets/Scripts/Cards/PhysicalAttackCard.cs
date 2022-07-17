using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAttackCard : AbilityCard
{
    public override void DoAction(Character target)
    {
        Character c = GetComponentInParent<Character>();
        target.Damage(c.physicalAttack+c.Buffed);
    }
}