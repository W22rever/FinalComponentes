using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    [Tooltip("ID de la llave necesaria para abrir esta puerta")]
    [SerializeField] private string llaveNecesariaID = "LlavePuerta";
    [SerializeField] private KeyCode teclaAbrir = KeyCode.E;

    private bool jugadorCerca = false;
    private Inventario inventario;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            inventario = other.GetComponent<Inventario>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            inventario = null;
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaAbrir))
        {
            if (inventario != null && inventario.TieneLlave(llaveNecesariaID))
            {
                inventario.EliminarLlave(llaveNecesariaID);
                AbrirPuerta();
            }
            else
            {
                Debug.Log("No tienes la llave correcta: " + llaveNecesariaID);
            }
        }
    }

    void AbrirPuerta()
    {
        gameObject.SetActive(false);

        // Si esta puerta lleva al siguiente nivel:
        if (llaveNecesariaID == "LlaveFinal") // Ajusta el ID según tu lógica
        {
            SceneManager.LoadScene("level2"); // Asegúrate que esté bien escrito el nombre exacto
        }
    }
}
