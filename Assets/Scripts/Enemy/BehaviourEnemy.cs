using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourEnemy : MonoBehaviour
{
    private SpriteRenderer enemySR;
    [SerializeField] private float fadeSpeed = 1f;
    private Color targetColor;
    [HideInInspector] public SpawnerEnemy spawner; // Referencia al spawner
    private void Awake()
    {
        enemySR = GetComponent<SpriteRenderer>();
        targetColor = new Color(
            enemySR.color.r,
            enemySR.color.g,
            enemySR.color.b,
            0f // Alpha final
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSecondsRealtime(3);
        while (enemySR.color.a > 0.01f)
        {
            enemySR.color = Color.Lerp(enemySR.color, targetColor, fadeSpeed * Time.deltaTime);
            yield return null; 
        }

        enemySR.color = targetColor; // Asegura alpha final en 0

        if (spawner != null)
        {
            spawner.StartCoroutine(spawner.TimeToReSpawn());
        }

        Destroy(gameObject);
    }
}
