using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 4;
    private int currentLives;

    [Header("Damage Particles")]
    public GameObject damageParticles;

    private void Start()
    {
        currentLives = maxLives;

        if (damageParticles != null)
        {
            damageParticles.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        currentLives--;

        if (damageParticles != null)
        {
            damageParticles.SetActive(true);
        }

        if (currentLives <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}