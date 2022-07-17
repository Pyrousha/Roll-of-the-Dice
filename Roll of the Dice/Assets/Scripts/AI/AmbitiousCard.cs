using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbitiousCard : Character
{
    public override void DoTurn()
    {
        if(Confused > 0) TargetConfused();
        else
        {
            Character[] allAllies = GetAllPlayerCharacters();
            Character target = allAllies[0];
            for(int i = 1; i < allAllies.Length; i++)
            {
                if(allAllies[i].health > target.health)
                {
                    target = allAllies[i];
                }
            }

            Vector2 unit = (new Vector2(transform.position.x, transform.position.y) - new Vector2(target.transform.position.x, target.transform.position.y)).normalized;
            Vector2 bestPos = new Vector2(target.transform.position.x, target.transform.position.y) + unit * minDistance;
            if(moveRangeObj.GetComponent<Collider2D>().OverlapPoint(bestPos)) targetPosition = bestPos;
            else targetPosition = moveRangeObj.GetComponent<Collider2D>().ClosestPoint(bestPos);
            targetPosition.z = target.transform.position.z;
        }
        int abilityIndex = Mathf.FloorToInt(Random.value * deck.Length);
        DoAbility(deck[abilityIndex]);
    }
}