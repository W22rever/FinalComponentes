using UnityEngine;

public class Llave : MonoBehaviour
{
    public string llaveID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario inventario = other.GetComponent<Inventario>();
            if (inventario != null)
            {
                inventario.AgregarLlave(llaveID);
                Destroy(gameObject);
            }
        }
    }
}