using UnityEngine;

public class Llave : MonoBehaviour
{
    [Tooltip("ID único de esta llave")]
    [SerializeField] private string llaveID;

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