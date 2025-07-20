using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public SpriteRenderer spriteRenderer;

    [Header("Movimiento")]
    public float speed = 3f;
    public float detectionRange = 5f;

    [Header("Desvanecimiento")]
    public LayerMask obstaculoMask; // Capa de obstáculos
    public float fadeSpeed = 2f;

    [Header("Lógica")]
    public float waitTimeOutside = 3f;
    public int toquesAPlayer = 0;

    private Vector3 spawnPoint;
    private bool isAttacking = true;
    private bool isFinalAttack = false;

    private void Start()
    {
        spawnPoint = transform.position;
        StartCoroutine(EnemyCycle());
    }

    IEnumerator EnemyCycle()
    {
        while (toquesAPlayer < 4)
        {
            isAttacking = true;

            while (isAttacking)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Si no hay obstáculos y está cerca del jugador, empieza a desvanecerse
                if (distanceToPlayer < detectionRange && !isFinalAttack)
                {
                    if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstaculoMask))
                    {
                        float newAlpha = Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime);
                        Color c = spriteRenderer.color;
                        c.a = newAlpha;
                        spriteRenderer.color = c;
                    }
                }

                // Movimiento hacia el jugador
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // Detecta colisión cercana con el jugador
                if (distanceToPlayer < 0.5f)
                {
                    toquesAPlayer++;

                    if (toquesAPlayer == 3)
                        isFinalAttack = true;

                    isAttacking = false;
                    break;
                }

                yield return null;
            }

            if (toquesAPlayer < 4)
            {
                // Se aleja a un punto fuera del mapa
                Vector3 exitPoint = GetRandomPointOutsideMap();

                while (Vector3.Distance(transform.position, exitPoint) > 0.5f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, exitPoint, speed * Time.deltaTime);
                    yield return null;
                }

                // Espera con el sprite invisible
                spriteRenderer.color = new Color(1, 1, 1, 0);
                yield return new WaitForSeconds(waitTimeOutside);

                // Vuelve a hacerse visible
                if (!isFinalAttack)
                {
                    spriteRenderer.color = new Color(1, 1, 1, 1);
                }
            }
        }

        // Ataque final: sin desvanecimiento
        spriteRenderer.color = new Color(1, 1, 1, 1);
        while (Vector3.Distance(transform.position, player.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            yield return null;
        }

        // Toca al jugador en el ataque final: derrota
        Debug.Log("¡El jugador perdió!");
        // Aquí puedes colocar evento de derrota
    }

    Vector3 GetRandomPointOutsideMap()
    {
        float distance = 15f; // Distancia fuera del mapa
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        return player.position + new Vector3(randomDir.x, 0, randomDir.y) * distance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

