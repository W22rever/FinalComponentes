using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaReinicio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PuertaInteractiva.retoCompletado)
        {
            Debug.Log("Reiniciando escena...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}