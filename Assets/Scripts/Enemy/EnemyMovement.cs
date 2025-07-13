using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    private Transform targetPlayer;

    private void Awake()
    {
       GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            targetPlayer = playerObject.transform;
        }
    }

    private void FixedUpdate()
    {
        MovementEnemy();
    }

    public void  MovementEnemy()
    {
        if (targetPlayer != null && targetPlayer.gameObject != null)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
        }

        Vector3 scale = transform.localScale;
        scale.x = targetPlayer.position.x < transform.position.x ? -1 : 1;
       // transform.localScale = scale;
    }
}
