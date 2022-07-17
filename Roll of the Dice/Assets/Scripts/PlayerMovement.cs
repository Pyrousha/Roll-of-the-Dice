using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField]
    //private float moveSpeed;

    //public List<Character> allAllies;
    List<Character> allAllies;

    public GameObject diceSelectContainer;
    public GameObject diceInfoContainer;

    private Vector3 targetPosition;

    private float epsilon = 0.05f;

    public bool playerTurn = false;
    int allyIndex = 0;

    BattleManager battleManager;
    public int activeDiceIndex = -1;

    public Character currentAlly;
    public Character waitingAlly;

    private float minDistance = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingAlly != null)
        {
            if(waitingAlly.anim.GetBool("BlankOver"))
            {
                waitingAlly.anim.SetBool("BlankOver", false);
                waitingAlly.iconSprite.gameObject.SetActive(false);
            }
        }

        if(playerTurn && currentAlly != null) {
            //bool diceSelectOn = (activeDiceindex == -1) && (currentAlly != null);
            RaycastHit2D[] hitArr = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach(RaycastHit2D hit in hitArr)
            {
                
                //Check if the player clicked within their movement range
                //if(hit.collider.gameObject.layer == gameObject.layer)
                if(hit.collider.gameObject == currentAlly.moveRangeObj)
                {
                    if(!currentAlly.hitRangeObj.activeSelf && activeDiceIndex != -1) currentAlly.hitRangeObj.SetActive(true);
                    Vector3 rangePos = hit.point;
                    rangePos.z = currentAlly.hitRangeObj.transform.position.z;
                    currentAlly.hitRangeObj.transform.position = rangePos;

                    if(Input.GetMouseButtonDown(0) && activeDiceIndex != -1)
                    {
                        Color color = currentAlly.outline.GetComponent<SpriteRenderer>().color;
                        color.a = 0;
                        currentAlly.outline.GetComponent<SpriteRenderer>().color = color;
                        //RaycastHit2D[] hitArr = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        //if(hit.collider.gameObject.layer == gameObject.layer)
                        //Clicked on a valid point
                        if(currentAlly.confused  >0)
                        {
                            Character closest = currentAlly.GetClosestPlayerCharacter();
                            Vector2 unit = (new Vector2(currentAlly.transform.position.x, currentAlly.transform.position.y) - new Vector2(closest.transform.position.x, closest.transform.position.y)).normalized;
                            Vector2 bestPos = new Vector2(closest.transform.position.x, closest.transform.position.y) + unit * minDistance;
                            if(currentAlly.moveRangeObj.GetComponent<Collider2D>().OverlapPoint(bestPos)) targetPosition = bestPos;
                            else targetPosition = currentAlly.moveRangeObj.GetComponent<Collider2D>().ClosestPoint(bestPos);
                            targetPosition.z = closest.transform.position.z;
                            currentAlly.confused--;
                        }
                        else
                        {
                            targetPosition = hit.point;
                            //targetPosition.z = transform.position.z;
                            targetPosition.z = currentAlly.transform.position.z;
                        }
                        //disable move range obj
                        //moveRangeObj.SetActive(false);
                        EndAllyTurn(allyIndex);
                        currentAlly.hitRangeObj.SetActive(false);

                        // TODO: dummy dice object
                        //AbilityDice activeDice = new AbilityDice();

                        //Start moving character
                        StartCoroutine(MoveTowardsPoint(currentAlly.GetEquippedDice()[activeDiceIndex]));
                        currentAlly.anim.SetTrigger("DoSpin");
                        currentAlly.outline.GetComponent<Animator>().SetTrigger("DoSpin");

                        return;
                    }
                } 
                else if(currentAlly.hitRangeObj.activeSelf) currentAlly.hitRangeObj.SetActive(false);
            }
        }
    }

    public void StartPlayerTurn()
    {
        playerTurn = true;
        allyIndex = 0;
        StartAllyTurn(0);
        currentAlly = null;
        foreach(Character c in battleManager.GetAllPlayerCharacters())
        {
            Color color = c.outline.GetComponent<SpriteRenderer>().color;
            color.a = 1;
            c.outline.GetComponent<SpriteRenderer>().color = color;
        }
        //InitDiceSelect();
    }

    public void ChooseDice(int index)
    {
        HideDiceInfo();
        activeDiceIndex = index;
        diceSelectContainer.SetActive(false);
        currentAlly.moveRangeObj.SetActive(true);
    }

    public void ShowDiceInfo(int index)
    {
        int i = 0;
        foreach(AbilityDice.DiceAction action in currentAlly.GetEquippedDice()[index].DiceSides)
        {
            GameObject go = diceInfoContainer.transform.GetChild(i).gameObject;
            go.GetComponentInChildren<UnityEngine.UI.Image>().sprite = action.icon;
            go.GetComponentInChildren<TextMeshProUGUI>().SetText(System.String.Format("{0} x{1}", action.name, action.numOfSides));
            go.SetActive(true);
            i++;
        }
        for(; i < diceInfoContainer.transform.childCount; i++)
        {
            GameObject go = diceInfoContainer.transform.GetChild(i).gameObject;
            go.SetActive(false);
        }
        diceInfoContainer.SetActive(true);
    }

    public void HideDiceInfo()
    {
        diceInfoContainer.SetActive(false);
    }

    public void InitDiceSelect()
    {
        // How the actual FUCK does the new Unity UI system work? The old one was so much easier
        /*
        // hade all buttons
        for(int childIndex = 0; childIndex < diceSelectContainer.transform.childCount; childIndex++)
        {
            GameObject go = diceSelectContainer.transform.GetChild(childIndex).gameObject;
            go.SetActive(false);
        }

        diceSelectContainer.SetActive(true);

        // create new buttons
        AbilityDice[] dice = currentAlly.GetEquippedDice();
        int i = 0;
        for(int childIndex = 0; childIndex < diceSelectContainer.transform.childCount; childIndex++)
        {
            GameObject go = diceSelectContainer.transform.GetChild(childIndex).gameObject;
            Button b = go.GetComponent<Button>();
            b.clicked.RemoveAllListeners();
        }
        */
        int i = 0;
        foreach(AbilityDice dice in currentAlly.GetEquippedDice())
        {
            GameObject go = diceSelectContainer.transform.GetChild(i).gameObject;
            go.GetComponentInChildren<TextMeshProUGUI>(true).SetText(dice.name);
            go.SetActive(true);
            i++;
        }
        for(; i < diceSelectContainer.transform.childCount; i++)
        {
            GameObject go = diceSelectContainer.transform.GetChild(i).gameObject;
            go.SetActive(false);
        }
        diceSelectContainer.SetActive(true);
    }

    void StartAllyTurn(int index)
    {
        //currentAlly = battleManager.GetAllPlayerCharacters()[index];
        //allAllies[index].moveRangeObj.SetActive(true);
        //allAllies[index].hitRangeObj.SetActive(true);
        //InitDiceSelect();
    }

    void EndAllyTurn(int index) {
        currentAlly.moveRangeObj.SetActive(false);
        Color color = currentAlly.outline.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        currentAlly.outline.GetComponent<SpriteRenderer>().color = color;
        //allAllies[index].hitRangeObj.SetActive(false);
    }

    IEnumerator MoveTowardsPoint(AbilityDice dice)
    {
        //while (Vector3.Distance(transform.position, targetPosition) > epsilon)
        while (Vector3.Distance(currentAlly.transform.position, targetPosition) > epsilon)
        {
            //player is not done moving yet

            //currentAlly.transform.position = Vector3.Lerp(currentAlly.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            currentAlly.transform.position = Vector3.Lerp(currentAlly.transform.position, targetPosition, Time.deltaTime * currentAlly.moveSpeed);

            yield return null;
        }

        //Close enough to target position
        currentAlly.transform.position = targetPosition;
        currentAlly.anim.SetTrigger("SpinOver");
        currentAlly.outline.GetComponent<Animator>().SetTrigger("SpinOver");

        // hit all characters within range except the one doing the action
        Transform hitRangeTransform = currentAlly.hitRangeObj.transform;
        Collider2D[] hitArr = Physics2D.OverlapCircleAll(targetPosition, hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.DrawLine(hitRangeTransform.position, hitRangeTransform.position + Vector3.right * hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.Log(hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //List<Collider2D> hits = new List<Collider2D>();
        //ContactFilter2D filter = new ContactFilter2D();
        //hitRangeTransform.GetComponent<Collider2D>().OverlapCollider(filter, hits);
        
        AbilityDice.DiceAction action = dice.RollAction();
        foreach(Collider2D col in hitArr)
        {
            Character charHit = col.GetComponentInParent<Character>();
            if(charHit != null)
            {
                //if(charHit.isPlayerCharacter) continue;
                if(charHit != currentAlly) dice.DoAction(action, charHit);
            }
        }

        currentAlly.iconSprite.sprite = action.icon;
        currentAlly.iconSprite.gameObject.SetActive(true);
        waitingAlly = currentAlly;

        //show move range again (just for testing)
        //moveRangeObj.SetActive(true);
        allyIndex++;
        //if(allyIndex == allAllies.Count)
        if(allyIndex == battleManager.GetAllPlayerCharacters().Length)
        {
            playerTurn = false;
            battleManager.EndPlayerTurn();
        }
        else StartAllyTurn(allyIndex);
        activeDiceIndex = -1;
        currentAlly = null;
    }
}
