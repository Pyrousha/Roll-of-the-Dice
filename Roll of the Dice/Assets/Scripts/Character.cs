using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public bool isPlayerCharacter;

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    public int health {get; private set;}

    public int physicalAttack;
    public int magicAttack;
    //public int defense;
    public float critMultiplier = 2f;
    public float clumsyPenalty = 3f;

    public Animator anim;

    public GameObject moveRangeObj;
    public GameObject hitRangeObj;
    public GameObject equipment;
    public GameObject outline;
    public SpriteRenderer iconSprite;

    public Color color1; //dark color
    public Color color2; //light color

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpFill;

    public float moveSpeed = 5f;

    protected AbilityCard[] deck; // only used by cards
    public Vector3 targetPosition;

    public float epsilon = 0.05f;
    private BattleManager battleManager;

    void Start()
    {   
        battleManager = GetComponentInParent<BattleManager>();
        deck = equipment.GetComponentsInChildren<AbilityCard>();
        health = maxHealth;

        if(moveRangeObj != null)
        {
            moveRangeObj.GetComponent<SpriteRenderer>().color = color2;
        }

        UpdateMyHpBar();
        hpFill.color = color1;
    }

    public void Update()
    {   
        // cheese cuz I can't be bothered to actually fix it
        if(!moveRangeObj.activeSelf && hitRangeObj.activeSelf) hitRangeObj.SetActive(false);

        bool rangeOn = (battleManager.playerController.currentAlly == this && outline.GetComponent<SpriteRenderer>().color.a > 0);
        if(!rangeOn && battleManager.playerController.playerTurn && battleManager.playerController.activeDiceIndex == -1)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hitArr = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            foreach(RaycastHit2D hit in hitArr)
            {
                if(hit.transform.gameObject == GetComponentInChildren<SpriteRenderer>().gameObject)
                {
                    if(isPlayerCharacter && outline.GetComponent<SpriteRenderer>().color.a > 0 && Input.GetMouseButtonDown(0)) 
                    {
                        battleManager.playerController.currentAlly = this;
                        battleManager.playerController.InitDiceSelect();
                    }

                    UpdateBigHPBar();

                    rangeOn = true;
                    break;
                }
            }
        }
        if(rangeOn != moveRangeObj.activeSelf) moveRangeObj.SetActive(rangeOn);
    }

    public void Damage(int hp) 
    {
        if(hp > 0) 
        {
            //take damage

            health -= hp;
            if(health <= 0) 
            {
                health = 0;
                Die();
            }

            // TODO: flash character
        }
        else
        {
            //heal

            int missingHealth = maxHealth - health;
            hp = -Mathf.Min(missingHealth, -hp);
            health -= hp; 
        }

        UpdateMyHpBar();
        UpdateBigHPBar();

        DamageNumberAndFXCanvas.Instance.SpawnDamageNumber(transform, hp);
    }

    public void UpdateMyHpBar()
    {
        hpSlider.value = ((float)health)/maxHealth;
    }

    public void UpdateBigHPBar()
    {
        battleManager.healthSlider.gameObject.SetActive(true);
        battleManager.healthSlider.maxValue = maxHealth;
        battleManager.healthSlider.value = health;
        UnityEngine.UI.Image[] sliderImgs = battleManager.healthSlider.GetComponentsInChildren<UnityEngine.UI.Image>();
        sliderImgs[0].color = Color.black; // = color2;
        sliderImgs[1].color = color2; // = color1;
    }

    public void Die() {
        // TODO
        anim.SetTrigger("Die");
        gameObject.SetActive(false);
        battleManager.NotifyDeath(this);
    }

    public AbilityDice[] GetEquippedDice() {
        return equipment.GetComponentsInChildren<AbilityDice>();
    }

    public virtual void DoTurn() {} // for AI use

    Character[] currentChain;

    public void DoTurn(Character[] chain)
    {
        currentChain = chain;
        DoTurn();
    }

    protected Character[] GetAllPlayerCharacters()
    {
        return battleManager.GetAllPlayerCharacters();
    }

    protected Character[] GetAllAICharacters()
    {
        return battleManager.GetAllAICharacters();
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

    public void DoAbility(AbilityCard card)
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

        if(currentChain == null) transform.parent.GetComponent<BattleManager>().StartPlayerTurn();
        else if(currentChain.Length == 1) currentChain[0].DoTurn(null);
        else
        {
            Character[] chain = new Character[currentChain.Length-1];
            System.Array.Copy(currentChain, 1, chain, 0, currentChain.Length-1);
            currentChain[0].DoTurn(chain);
        }
    }
}  
