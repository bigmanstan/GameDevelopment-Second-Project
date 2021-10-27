using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBehaviorScript : MonoBehaviour, IEntity
{
    private NavMeshAgent agent;
    private Vector3 prevPos;
    private Animator enemyAnimator;

    public Transform playerTransform;
    public EnemySpawner es;
    public float attackDistance = 10;
    public float movementSpeed = 2f;
    float nextAttackTime = 0;
    public float npcDamage = 5;
    public Transform firePoint;
    public float attackRate = 0.5f;
    public float npcHP = 100;
    Rigidbody r;

    // Start is called before the first frame update
 
    public AudioClip source;
    public AudioClip Lfoot;
    public AudioClip Rfoot;
    AudioSource audioSource;
    private MouseController1 MouseController;
    public GameObject ant;
    void Start()
    {
        //print(attackDistance);
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
        enemyAnimator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        MouseController = ant.GetComponent<MouseController1>(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (MouseController.enemyhealth == 0)
        {
            enemyAnimator.SetBool("isDead", true);
        }
       //print(Time.time > nextAttackTime);
       // print("attack v");
        
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                RaycastHit hit;
               
                //print(Physics.Raycast(firePoint.position, firePoint.forward, out hit));
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {   
                    //print(hit.transform.name);
                    //Debug.DrawRay(firePoint.position, firePoint.forward ,Color.green);
                    //Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                    //print("player tag");
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);


                        IEntity player = hit.transform.GetComponent<IEntity>();
                       // print("here 3");
                        player.ApplyDamage(npcDamage);

                        Debug.Log("enemy is hitting");

                        if (agent.remainingDistance <= attackDistance)
                        {
                            enemyAnimator.SetBool("isAttacking", true);
                        }

                        else
                        {
                            print("else");
                            enemyAnimator.SetBool("isAttacking", false);
                        }
                    }
                }
            }
        }

        //Move towardst he player
        agent.destination = playerTransform.position;
        //Always look at player
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
        //Gradually reduce rigidbody velocity if the force was applied by the bullet
        r.velocity *= 0.99f;
        if(agent.velocity[0] != 0 )
        {
            enemyAnimator.SetBool("isWalking",true);       

        }
        else
        {
            enemyAnimator.SetBool("isWalking", false);
        }

        print(MouseController.enemyhealth);
    }



    public void ApplyDamage(float points)
    {
        npcHP -= points;
        print(npcHP);
        print("here 2");
        if (npcHP <= 0)
        {
            Debug.Log("Zombie Dead");
            enemyAnimator.SetBool("isDead", true);
            Destroy(agent, 10);
            es.EnemyEliminated(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Triggered Entered");
        if( other.transform.name == "Bat")
        {
            MouseController.enemyhealth = MouseController.enemyhealth - 25;
            //Debug.Log("Triggered Entered");
            enemyAnimator.SetBool("hit", true);
            audioSource.clip = source;
            audioSource.Play();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        enemyAnimator.SetBool("hit", false);
    }

    void FootR()

    {
        audioSource.clip = Lfoot;
        audioSource.Play();
    }

    void FootL()
    {
        audioSource.clip = Rfoot;
        audioSource.Play();
   }

}

