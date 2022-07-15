using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject moveRangeObj;

    [SerializeField] private float moveSpeed;

    private Vector3 targetPosition;

    private float epsilon = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Check if the player clicked within their movement range

            RaycastHit2D[] hitArr = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach(RaycastHit2D hit in hitArr)
            {
                if(hit.collider.gameObject.layer == gameObject.layer)
                {
                    //Clicked on a valid point
                    targetPosition = hit.point;
                    targetPosition.z = transform.position.z;

                    //disable move range obj
                    moveRangeObj.SetActive(false);

                    // TODO: dummy dice object
                    AbilityDice activeDice = new AbilityDice();

                    //Start moving character
                    StartCoroutine(MoveTowardsPoint(activeDice));
                    anim.SetTrigger("DoSpin");

                    return;
                }
            }
        }
    }

    IEnumerator MoveTowardsPoint(AbilityDice dice)
    {
        while (Vector3.Distance(transform.position, targetPosition) > epsilon)
        {
            //player is not done moving yet

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            yield return null;
        }

        //Close enough to target position
        transform.position = targetPosition;
        anim.SetTrigger("SpinOver");

        //show move range again (just for testing)
        moveRangeObj.SetActive(true);

        //TODO: Do action
        // TODO: dummy character object
        Character target = new Character();
        dice.DoAction(target);
    }
}
