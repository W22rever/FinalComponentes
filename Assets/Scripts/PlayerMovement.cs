using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(2.5f, 6.5f)] private float velocidad = 5f;
    [SerializeField] private KeyCode teclaArriba = KeyCode.W;
    [SerializeField] private KeyCode teclaAbajo = KeyCode.S;
    [SerializeField] private KeyCode teclaIzquierda = KeyCode.A;
    [SerializeField] private KeyCode teclaDerecha = KeyCode.D;

    private Rigidbody2D rb;
    private Vector2 direccion;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direccion = Vector2.zero;

        // Verificar teclas verticales primero
        if (Input.GetKey(teclaArriba))
        {
            direccion.y = 1;
        }
        else if (Input.GetKey(teclaAbajo))
        {
            direccion.y = -1;
        }
        else if (Input.GetKey(teclaIzquierda))
        {
            direccion.x = -1;
        }
        else if (Input.GetKey(teclaDerecha))
        {
            direccion.x = 1;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direccion * velocidad;
    }
}
