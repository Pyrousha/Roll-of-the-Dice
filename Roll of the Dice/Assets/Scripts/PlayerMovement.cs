using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public List<Character> allAllies;

    public GameObject diceSelectContainer;
    public GameObject diceInfoContainer;

    private Vector3 targetPosition;

    private float epsilon = 0.05f;

    bool playerTurn = false;
    int allyIndex = 0;

    BattleManager battleManager;
    int activeDiceIndex = 0;
    Vector3 oldHitRangePosition;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTurn) {
            RaycastHit2D[] hitArr = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach(RaycastHit2D hit in hitArr)
            {
                //Check if the player clicked within their movement range
                //if(hit.collider.gameObject.layer == gameObject.layer)
                if(hit.collider.gameObject == allAllies[allyIndex].moveRangeObj)
                {
                    if(!allAllies[allyIndex].hitRangeObj.activeSelf) allAllies[allyIndex].hitRangeObj.SetActive(true);
                    Vector3 rangePos = hit.point;
                    rangePos.z = allAllies[allyIndex].hitRangeObj.transform.position.z;
                    allAllies[allyIndex].hitRangeObj.transform.position = rangePos;

                    if(Input.GetMouseButtonDown(0))
                    {
                        //RaycastHit2D[] hitArr = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        //if(hit.collider.gameObject.layer == gameObject.layer)
                        //Clicked on a valid point
                        targetPosition = hit.point;
                        //targetPosition.z = transform.position.z;
                        targetPosition.z = allAllies[allyIndex].transform.position.z;

                        //disable move range obj
                        //moveRangeObj.SetActive(false);
                        EndAllyTurn(allyIndex);
                        allAllies[allyIndex].hitRangeObj.SetActive(false);

                        // TODO: dummy dice object
                        //AbilityDice activeDice = new AbilityDice();

                        //Start moving character
                        oldHitRangePosition = allAllies[allyIndex].hitRangeObj.transform.position;
                        StartCoroutine(MoveTowardsPoint(allAllies[allyIndex].GetEquippedDice()[activeDiceIndex]));
                        allAllies[allyIndex].anim.SetTrigger("DoSpin");

                        return;
                    }
                } 
                else if(allAllies[allyIndex].hitRangeObj.activeSelf) allAllies[allyIndex].hitRangeObj.SetActive(false);
            }
        }
    }

    public void StartPlayerTurn()
    {
        playerTurn = true;
        allyIndex = 0;
        StartAllyTurn(0);
        //InitDiceSelect();
    }

    public void ChooseDice(int index)
    {
        HideDiceInfo();
        activeDiceIndex = index;
        diceSelectContainer.SetActive(false);
        allAllies[allyIndex].moveRangeObj.SetActive(true);
    }

    public void ShowDiceInfo(int index)
    {
        int i = 0;
        foreach(AbilityDice.DiceAction action in allAllies[allyIndex].GetEquippedDice()[index].DiceSides)
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

    void InitDiceSelect()
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
        AbilityDice[] dice = allAllies[allyIndex].GetEquippedDice();
        int i = 0;
        for(int childIndex = 0; childIndex < diceSelectContainer.transform.childCount; childIndex++)
        {
            GameObject go = diceSelectContainer.transform.GetChild(childIndex).gameObject;
            Button b = go.GetComponent<Button>();
            b.clicked.RemoveAllListeners();
        }
        */
        int i = 0;
        foreach(AbilityDice dice in allAllies[allyIndex].GetEquippedDice())
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
        //allAllies[index].moveRangeObj.SetActive(true);
        //allAllies[index].hitRangeObj.SetActive(true);
        InitDiceSelect();
    }

    void EndAllyTurn(int index) {
        allAllies[index].moveRangeObj.SetActive(false);
        //allAllies[index].hitRangeObj.SetActive(false);
    }

    IEnumerator MoveTowardsPoint(AbilityDice dice)
    {
        //while (Vector3.Distance(transform.position, targetPosition) > epsilon)
        while (Vector3.Distance(allAllies[allyIndex].transform.position, targetPosition) > epsilon)
        {
            //player is not done moving yet

            allAllies[allyIndex].transform.position = Vector3.Lerp(allAllies[allyIndex].transform.position, targetPosition, Time.deltaTime * moveSpeed);

            yield return null;
        }

        //Close enough to target position
        allAllies[allyIndex].transform.position = targetPosition;
        allAllies[allyIndex].anim.SetTrigger("SpinOver");

        // hit all characters within range except the one doing the action
        Transform hitRangeTransform = allAllies[allyIndex].hitRangeObj.transform;
        Collider2D[] hitArr = Physics2D.OverlapCircleAll(oldHitRangePosition, hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.DrawLine(hitRangeTransform.position, hitRangeTransform.position + Vector3.right * hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //Debug.Log(hitRangeTransform.GetComponent<CircleCollider2D>().radius * hitRangeTransform.localScale.x);
        //List<Collider2D> hits = new List<Collider2D>();
        //ContactFilter2D filter = new ContactFilter2D();
        //hitRangeTransform.GetComponent<Collider2D>().OverlapCollider(filter, hits);
        foreach(Collider2D col in hitArr)
        {
            Character charHit = col.GetComponentInParent<Character>();
            if(charHit != null)
            {
                if(charHit.isPlayerCharacter) continue;
                if(charHit != allAllies[allyIndex]) dice.DoAction(charHit);
            }
        }

        //show move range again (just for testing)
        //moveRangeObj.SetActive(true);
        allyIndex++;
        if(allyIndex == allAllies.Count)
        {
            battleManager.EndPlayerTurn();
            playerTurn = false;
        }
        else StartAllyTurn(allyIndex);
    }
}
