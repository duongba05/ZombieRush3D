using UnityEngine;

public class EnemyChaseAttack : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1f;
    public CapsuleCollider handCollider;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            animator.SetBool("isRunning", false);
        }
        else if (distance <= detectionRange && distance > attackRange)
        {
            animator.SetBool("isRunning", true);
            animator.ResetTrigger("isAttack");

            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.rotation = Quaternion.LookRotation(direction);
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
        else if (distance <= attackRange)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("isAttack");
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("isDead");

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        Destroy(gameObject, 3f);
    }
    public void EnableHandCollider()
    {
        handCollider.enabled = true;
    }

    public void DisableHandCollider()
    {
        handCollider.enabled = false;
    }
}
