using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waitTime = 2f;

    private int touchCount = 0;
    private Transform currentTarget;
    private bool isWaiting = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (patrolPoints.Length > 0)
        {
            currentTarget = GetRandomPoint(null); // Comienza con uno aleatorio
        }
    }

    private void Update()
    {
        if (patrolPoints.Length == 0 || isWaiting || currentTarget == null)
            return;

        // Movimiento
        Vector3 direction = currentTarget.position - transform.position;
        Vector3 move = direction.normalized;

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Animaciones de movimiento
        bool isWalking = move != Vector3.zero;
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isSide", false);
        animator.SetBool("isBack", false);

        if (isWalking)
        {
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                animator.SetBool("isSide", true);
                spriteRenderer.flipX = move.x < 0;
            }
            else if (move.y > 0)
            {
                animator.SetBool("isBack", true);
            }
        }

        // Llegada al punto
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            StartCoroutine(WaitBeforeNextPoint());
        }
    }

    private IEnumerator WaitBeforeNextPoint()
    {
        isWaiting = true;

        // Forzar a idle
        animator.SetBool("isWalking", false);
        animator.SetBool("isSide", false);
        animator.SetBool("isBack", false);

        yield return new WaitForSeconds(waitTime);

        currentTarget = GetRandomPoint(currentTarget);
        isWaiting = false;
    }

    private Transform GetRandomPoint(Transform exclude)
    {
        if (patrolPoints.Length <= 1)
            return patrolPoints[0];

        Transform nextPoint;
        do
        {
            nextPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        } while (nextPoint == exclude);

        return nextPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchCount++;
            if (touchCount >= 3)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            foreach (var point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}