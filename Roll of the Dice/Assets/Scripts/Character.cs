using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isPlayerCharacter;

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;

    public int physicalAttack;
    public int magicAttack;
    //public int defense;
    public float critMultiplier = 2f;
    public float clumsyPenalty = 3f;

    public Animator anim;

    public GameObject moveRangeObj;
    public GameObject hitRangeObj;
    public GameObject equipment;

    //[SerializeField]
    public float moveSpeed = 5f;

    protected AbilityCard[] deck; // only used by cards
    protected Vector3 targetPosition;

    private float epsilon = 0.05f;

    void Start()
    {
        deck = equipment.GetComponentsInChildren<AbilityCard>();
    }

    public void Damage(int hp) {
        if(hp > 0) {
            health -= hp;
            if(health <= 0) Die();
            // TODO: flash character
        }
    }

    public void Die() {
        // TODO
        anim.SetTrigger("Die");
        gameObject.SetActive(false);
    }

    public AbilityDice[] GetEquippedDice() {
        return equipment.GetComponentsInChildren<AbilityDice>();
    }

    public virtual void DoTurn() {} // for AI use

    protected Character[] GetAllPlayerCharacters()
    {
        List<Character> playerCharacters = new List<Character>();
        foreach(Character c in transform.parent.GetComponentsInChildren<Character>())
        {
            if(c.isPlayerCharacter) playerCharacters.Add(c);
        }
        return playerCharacters.ToArray();
    }

    protected Character GetClosestPlayerCharacter()
    {
        Character character = null;
        float minDistance = Mathf.Infinity;
        foreach(Character c in transform.parent.GetComponentsInChildren<Character>())
        {
            if(c.isPlayerCharacter)
            {
                float newDistance = Vector2.Distance(c.transform.position, transform.position);
                if(newDistance < minDistance)
                {
                    minDistance = newDistance;
                    character = c;
                }
            }
        }
        return character;
    }

    protected void DoAbility(AbilityCard card)
    {
        StartCoroutine(MoveTowardsPoint(card));
    }

    IEnumerator MoveTowardsPoint(AbilityCard card)
    {
        anim.SetTrigger("DoSpin");
        while(!anim.GetBool("BlankOver"))
        {
            yield return null;
        }
        anim.SetBool("BlankOver", false);
        Debug.Log(targetPosition);
        //while (Vector3.Distance(transform.position, targetPosition) > epsilon)
        while (Vector3.Distance(transform.position, targetPosition) > epsilon)
        {
            //player is not done moving yet

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            yield return null;
        }

        //Close enough to target position
        transform.position = targetPosition;
        anim.SetTrigger("SpinOver");

        // hit all characters within range except the one doing the action
        Transform hitRangeTransform = hitRangeObj.transform;
        Collider2D[] hitArr = Physics2D.OverlapCircleAll(targetPosition, hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.DrawLine(hitRangeTransform.position, hitRangeTransform.position + Vector3.right * hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.Log(hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //List<Collider2D> hits = new List<Collider2D>();
        //ContactFilter2D filter = new ContactFilter2D();
        //GetComponentInChildren<Collider2D>().OverlapCollider(filter, hits);
        foreach(Collider2D col in hitArr)
        {
            Character charHit = col.GetComponentInParent<Character>();
            if(charHit != null)
            {
                if(!charHit.isPlayerCharacter) continue;
                if(charHit != this) card.DoAction(charHit);
            }
        }
    }
}  
