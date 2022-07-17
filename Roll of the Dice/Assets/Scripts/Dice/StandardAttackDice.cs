using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAttackDice : AbilityDice
{
    public void StandardAttack(Character target) {
        Character c = GetComponentInParent<Character>();
        target.Damage(c.physicalAttack+c.Buffed);
    }

    public void CriticalHit(Character target) {
        Character c = GetComponentInParent<Character>();
        target.Damage(Mathf.FloorToInt((c.physicalAttack+c.Buffed) * c.critMultiplier));
    }

    public void ClumsyAttack(Character target) {
        Character c = GetComponentInParent<Character>();
        target.Damage(Mathf.FloorToInt((c.physicalAttack+c.Buffed) / c.clumsyPenalty));
    }
}
