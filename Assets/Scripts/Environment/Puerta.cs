using UnityEngine;

public class Puerta : MonoBehaviour
{
    public string llaveNecesariaID = "LlavePuerta";
    public KeyCode teclaAbrir = KeyCode.E;

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

    void Update()
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
                Debug.Log("Necesitas la llave: " + llaveNecesariaID);
            }
        }
    }

    void AbrirPuerta()
    {
        gameObject.SetActive(false);
    }
}
