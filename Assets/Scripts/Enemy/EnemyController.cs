using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    private SpriteRenderer spriteRenderer;

    [Header("Movimiento")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRange = 5f;

    [Header("Desvanecimiento")]
    private float fadeSpeed = 1;
    [SerializeField, Tooltip("Mayor a 1 se desvanecera antes de llegar al player, menor a 1 desvanecera despues de llegar al player. Con 1 como valor, se desvaneceará apenas llegue al jugador. Así que recomendado usara de 1 a 1.3")] 
    private float fadeFactorMultiplier = 1f;

    [Header("Lógica")]
    [SerializeField] private float tiempoDeEsperaAfuera = 3f;
    [SerializeField] private int vecesAtacarPlayer;
    private int toquesAPlayerRealizados = 0;

    [Header("Referencias")]
    [SerializeField] private Collider2D enemyCollider;

    [SerializeField] private Vector3[] boundsPointsMapTop;
    [SerializeField] private Vector3[] boundsPointsMapDown;

    private Vector3 spawnPoint;
    private bool isAttacking = true;
    private bool isFinalAttack = false;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spawnPoint = transform.position;
        enemyCollider.isTrigger = true;
        fadeSpeed = fadeFactorMultiplier / (detectionRange / speed);
        StartCoroutine(EnemyCycle());
    }

    private bool PlayerIsInUpperZone()
    {
        return player.position.y > 31.9; // Ajusta si el pasillo está en otra posición
    }

    private Vector3 GetRandomExitPoint()
    {
        if (PlayerIsInUpperZone())
        {
            int index = Random.Range(0, boundsPointsMapTop.Length);
            return boundsPointsMapTop[index];
        }
        else
        {
            int index = Random.Range(0, boundsPointsMapDown.Length);
            return boundsPointsMapDown[index];
        }
    }

    IEnumerator EnemyCycle()
    {
        // Este bucle maneja los ataques normales del enemigo
        while (toquesAPlayerRealizados < vecesAtacarPlayer)
        {
            isAttacking = true;

            // Aseguramos que el enemigo sea completamente visible al inicio de cada ciclo de ataque normal
            // Esto es crucial para que no haya un "fade" residual de la última vez que apareció.
            Color fullOpacity = spriteRenderer.color;
            fullOpacity.a = 1f;
            spriteRenderer.color = fullOpacity;

            while (isAttacking)
            {
                if (player == null) yield break;

                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Lógica de desvanecimiento: SÓLO si NO es el ataque final.
                // Esta es la condición principal para el desvanecimiento.
                if (!isFinalAttack) // Ya no necesitamos distanceToPlayer < detectionRange aquí
                {
                    // Si el jugador está dentro del rango de detección, desvanecerse.
                    if (distanceToPlayer < detectionRange)
                    {
                        // Se mantiene tu lógica original de fade para los ataques normales.
                        float newAlpha = Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime);
                        Color c = spriteRenderer.color;
                        c.a = newAlpha;
                        spriteRenderer.color = c;
                    }
                    else // Si el jugador NO está dentro del rango de detección, asegúrate de que el enemigo sea visible (o se esté volviendo visible).
                    {
                        // Esta parte ayuda a que el enemigo no esté invisible cuando está fuera del rango
                        // y se espera que esté "cargando" para el siguiente ataque.
                        Color c = spriteRenderer.color;
                        c.a = Mathf.MoveTowards(spriteRenderer.color.a, 1f, fadeSpeed * Time.deltaTime); // Asegura que se vuelva visible si está fuera de rango
                        spriteRenderer.color = c;
                    }
                }

                /// IMPORTANTE: Ya no hay un 'else if (isFinalAttack)' aquí para la opacidad
                /// porque tan pronto como 'isFinalAttack' se vuelve true, salimos del ciclo de ataque normal
                /// y pasamos directamente a FinalAttack().

                // Movimiento hacia el jugador
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // Llegó al jugador
                if (distanceToPlayer < 0.2f)
                {
                    toquesAPlayerRealizados++;

                    // Si este es el último ataque permitido, preparar ataque final
                    if (toquesAPlayerRealizados == vecesAtacarPlayer)
                    {
                        isFinalAttack = true;
                        isAttacking = false; // Salir del bucle de ataque normal
                        break; // Salir del while(isAttacking)
                    }

                    isAttacking = false; // El ataque normal ha terminado, salir del bucle interno
                    break;
                }

                yield return null;
            }

            // Si todavía no es el último ataque, hacer salida y regreso
            if (!isFinalAttack)
            {
                Vector3 exitPoint = GetRandomExitPoint();

                // Asegurar que el sprite se desvanece por completo antes de irse
                spriteRenderer.color = new Color(1, 1, 1, 0);

                while (Vector3.Distance(transform.position, exitPoint) > 0.5f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, exitPoint, speed * 2 * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(tiempoDeEsperaAfuera); // Espera invisible
                // La opacidad se restaurará a 1 al inicio del próximo ciclo de ataque normal
            }
            else // Si isFinalAttack es true, salimos del bucle principal para ir al ataque final
            {
                break;
            }
        }

        // ATAQUE FINAL
        // Una vez que el bucle de ataques normales termina (ya sea por toques completados o isFinalAttack=true)
        // se activa el ataque final.
        if (isFinalAttack)
        {
            StartCoroutine(FinalAttack()); // Inicia la corrutina del ataque final.
            // Es importante detener EnemyCycle si ya no lo necesitamos, para evitar comportamientos inesperados.
            yield break; // Salir de la corrutina EnemyCycle
        }
    }

    // Corrutina dedicada exclusivamente al ataque final
    IEnumerator FinalAttack()
    {
        // Asegurar que el sprite esté completamente visible al inicio del ataque final
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Opacidad máxima asegurada
        enemyCollider.isTrigger = false;// Cambiar a colisión física

        while (player != null && Vector3.Distance(transform.position, player.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            yield return null;
        }

        if (player != null)
        {
            Debug.Log("¡Jugador eliminado en ataque final!");
            EscenarioIluminacionController iluminacion = FindFirstObjectByType<EscenarioIluminacionController>();
            if (iluminacion != null)
            {
                iluminacion.ActivarLucesMaximas();
            }
            else
            {
                Debug.LogWarning("No se encontró EscenarioIluminacionController en la escena.");
            }

            yield return null; // Espera un frame antes de destruir

            Destroy(player.gameObject);
            
        }

        Debug.Log("¡El jugador perdió!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        foreach (var point in boundsPointsMapTop)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }

        Gizmos.color = Color.red;
        foreach (var point in boundsPointsMapDown)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }
}

