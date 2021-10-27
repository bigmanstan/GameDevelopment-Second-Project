   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseController1 : MonoBehaviour
{
    //NavMeshAgent variable control player movement
    public NavMeshAgent playerNavMeshAgent;
    public DamageReceiver player;

    //follow player movement
    public Camera playerCamera;

    public Animator playerAnimator;
    public bool isRunning;
    public bool isAttacking;
    public bool canMove;
    public float enemyhealth;

   
    //How many enemies we already eliminated in the current wave




    public Transform enemyTransform;

    void Start()
    {
        canMove = true;
        enemyhealth = 100;
    }

    private void Update()
    {

        //print(player.playerHP);
        //print((enemyTransform.position - transform.position).magnitude);
        //print(isRunning);

        if (isAttacking)
        {
            if ((enemyTransform.position - transform.position).magnitude < 1.6)
            {
                playerAnimator.SetBool("isAttacking", true);
                
            }
            else
            {
                playerAnimator.SetBool("isAttacking", false);
            }
           
        }
        
        //if the left button of is clicked
        if (Input.GetMouseButton(0) && canMove)
        {
            //Unity cast a ray from the position of mouse cursor on-screen toward the 3D scene.
            Ray myRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit myRaycastHit;

            if (Physics.Raycast(myRay, out myRaycastHit))
            {
   
                //Assign ray hit point as Destination of Navemesh Agent (Player)
                playerNavMeshAgent.SetDestination(myRaycastHit.point);

            }

            if (myRaycastHit.transform.tag == "Enemy")
            {
                Vector3 between = transform.position - enemyTransform.position;
                Vector3 newPosition = between.normalized * 1.5f + enemyTransform.position;
                playerNavMeshAgent.destination = newPosition;
                isAttacking = true;
               // if ((enemyTransform.position - transform.position).magnitude < 1.6)
               // {
                    //Debug.Log("Hit Enemy");
               //     enemyhealth = enemyhealth - 15;
               // }
                //Debug.Log("Clicked on z Enemy");
            }
        }

        if (playerNavMeshAgent.remainingDistance <= playerNavMeshAgent.stoppingDistance)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }

        playerAnimator.SetBool("isRunning", isRunning);
    }

}
