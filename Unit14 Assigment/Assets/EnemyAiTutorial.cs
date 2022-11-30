using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyAiTutorial : MonoBehaviour
{
    public TextMeshPro m_Text;
    private TextContainer m_TextContainer;

    public Animator animator;

    public NavMeshAgent agent;

    public Transform player;
    public Transform gunPosition;
    public GameObject gun;

    public GameObject blood;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 100;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    
    public Transform shootFromPosition;


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool _isDead = false;

    private void Awake()
    {


        m_Text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        m_Text.autoSizeTextContainer = true;

        // m_TextContainer = GetComponent<TextContainer>();
        // m_TextContainer.width = 25f;
        // m_TextContainer.height = 3f;
        // // Set the point size
        m_Text.fontSize = 5;
        m_Text.text = ""+health;

        player = GameObject.Find("PlayerController").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    public bool isDead {
        get {
            return _isDead;
        }

        set {
            _isDead = value;
        }
    }

    void Start() {
        //Instantiate(gun, transform.position, transform.rotation);
    }
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling(); //instead of patrolling
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();


    }
    private void idle()
    {

        animator.SetFloat("Speed_f", 0.0f);
        // animator.SetInteger("Animation_int", 8); //leaning against bar

    }

    private void RunToPointAndShoot() {
        // animator.SetBool("Shoot_b", false);
        // animator.SetInteger("WeaponType_int", 0); 
        if(_isDead) return;

        // if (!walkPointSet) SearchWalkPoint();

        // if (walkPointSet) {

        // }

        agent.Stop();
        agent.ResetPath();
        agent.SetDestination(shootFromPosition.position);

        Vector3 distanceToWalkPoint = transform.position - shootFromPosition.position;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            // agent.speed = 0.0f;
            // // animator.SetFloat("Speed_f", 0.0f);
            // animator.SetInteger("WeaponType_int", 1); //change to non shoot pose
            // animator.SetBool("Shoot_b", true);
            //DestroyEnemy();
            AttackPlayer();
            //walkPointSet = false;
        } else {
            animateToMoving(1.0f, 3.0f);
        }
    }

    private void Patroling()
    {
        animator.SetBool("Shoot_b", false);
        animator.SetInteger("WeaponType_int", 0); 
        if(_isDead) return;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) {
            agent.Stop();
            agent.ResetPath();
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animateToMoving(0.5f,1.0f);

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            agent.speed = 0.0f;
            animator.SetFloat("Speed_f", 0.0f);
            walkPointSet = false;
        } 
    }

    private void animateToMoving(float speed, float agentSpeed) {

        animator.SetInteger("WeaponType_int", 0); //change to shoot pose
        if(animator.GetFloat("Speed_f") > 1.0f) {
            agent.speed = 0.0f;
            animator.SetFloat("Speed_f", 0.0f);
        } else {
            agent.speed = agentSpeed;
            animator.SetFloat("Speed_f", speed);
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if(_isDead) return;
        animateRunning();
        agent.SetDestination(player.position);
    }

    private void animateRunning() {
        agent.speed = 3.0f;
        animator.SetFloat("Speed_f", 1.5f);
    }

    private void animateStoppingAndShooting() {
        agent.speed = 0.0f;
        animator.SetFloat("Speed_f", 0.0f);
        animator.SetInteger("WeaponType_int", 1); 
        animator.SetBool("Shoot_b", true);
    }


    private void AttackPlayer()
    {
        if(_isDead) return;
        animateStoppingAndShooting();   
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // transform.LookAt(player);
        var target_point = player.transform.position;
        target_point.y = 0;
     
         //transform.LookAt(target_con.transform.position,transform.up);
        transform.LookAt(target_point);

        if (!alreadyAttacked)
        {
            ///Attack code here 
            //gun.GetComponent<SimpleShoot>().Shoot();
            // Rigidbody rb = Instantiate(projectile, gunPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(gunPosition.forward * 10f, ForceMode.Impulse);
            // rb.AddForce(gunPosition.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        m_Text.text = ""+health;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        //if(isDead) return;

        Debug.Log("DestroyEnemy");
        // animator.SetBool("Shoot_b", false);
        // animator.SetInteger("WeaponType_int", 0); //change to non shoot pose
        animator.SetBool("Death_b", true);
        isDead = true;
        Destroy(gameObject, 2);
    }
    
    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Bullet"){
            Invoke(nameof(DestroyEnemy), 0.0f);
            TakeDamage(100);
            Instantiate(blood, other.transform);
        } 
        if(other.gameObject.tag == "Knife"){
            Invoke(nameof(DestroyEnemy), 0.0f);
            TakeDamage(100);
            Instantiate(blood, other.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void setupAnimations() {
        if(agent != null) {
             Debug.Log("agent.velocity.magnitude  " + agent.velocity.magnitude);
             // agent.speed = 0f;
            // animator.SetInteger("Animation_int", 4); 
            // animator.SetInteger("Animation_int", 4); //dancing
            // animator.SetInteger("Animation_int", 8); //leaning against bar

        }

        if(gameObject.tag == "SaloonGirl"){
            animator.SetInteger("Animation_int", 4); //dancing

        }

        if(gameObject.tag == "Sheriff"){
            animator.SetInteger("Animation_int", 6); //salooting

        }

        if(gameObject.tag == "BadGuy"){
            animator.SetInteger("Animation_int", 8); //leaning against bar

        }

        if(gameObject.tag == "GunMan"){

            animator.SetInteger("Animation_int", 7); //wiping mouth

        }

        if(gameObject.tag == "BusinessMan"){
            animator.SetInteger("Animation_int", 9); 
            //5- smoking

        }

        if(gameObject.tag == "Woman"){
            // animator.SetInteger("Animation_int", 9); //sitting on ground
            animator.SetInteger("Animation_int", 2); //Hands on hips

        }

        if(gameObject.tag == "CowGirl"){
            animator.SetInteger("Animation_int", 1); //crossed arms

        }
    }
}