using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Character
{
    public Character[] wave1;
    public Character[] wave2;

    bool hide = true;
    bool stop = false;
    bool appear = false;

    int wave = 0;


    void Update()
    {
        if(wave == 0)
        {
            bool allDead = true;
            foreach(Character c in wave1)
            {
                if(c.health > 0)
                {
                    allDead = false;
                    break;
                }
            }
            if(allDead)
            {
                //appear = true;
                wave++;
                foreach(Character c in wave2)
                {
                    c.gameObject.SetActive(true);
                }
                
            }
        }
        else if(wave == 1)
        {
            bool allDead = true;
            foreach(Character c in wave2)
            {
                if(c.health > 0)
                {
                    allDead = false;
                    break;
                }
            }
            if(allDead)
            {
                //appear = true;
                stop = false;
                
            }
        }
    }

    public override void DoTurn()
    {
        /*
        if(Confused > 0) TargetConfused();
        else
        {
        */
            // Summon goonies in multiple waves
            // stay away from d20 because
            if(hide)
            {
                hide = false;
                stop = true;
                targetPosition = transform.position + Vector3.up*7;
                DoAbility(deck[0]);
            }
            else if(stop)
            {
                targetPosition = transform.position;
                DoAbility(deck[0]);
            }
            else
            {
                Character closest = GetClosestPlayerCharacter();
                Vector2 unit = (new Vector2(transform.position.x, transform.position.y) - new Vector2(closest.transform.position.x, closest.transform.position.y)).normalized;
                Vector2 bestPos = new Vector2(closest.transform.position.x, closest.transform.position.y) + unit * minDistance;
                moveRangeObj.SetActive(true);
                if(moveRangeObj.GetComponent<Collider2D>().OverlapPoint(bestPos)) targetPosition = bestPos;
                else targetPosition = moveRangeObj.GetComponent<Collider2D>().ClosestPoint(bestPos);
                moveRangeObj.SetActive(false);
                targetPosition.z = closest.transform.position.z;

                int abilityIndex = Mathf.FloorToInt(Random.value * deck.Length);
                DoAbility(deck[abilityIndex]);
            }
        //}
    }
}