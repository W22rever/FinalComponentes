using UnityEngine;
using TMPro;

public class PuertaInteractiva : MonoBehaviour
{
    [Header("Configuraciones")]
    public KeyCode teclaAccion = KeyCode.P;
    public int cantidadPulsacionesNecesarias = 10;

    [Header("Referencias")]
    public GameObject enemigoAEliminar;
    public GameObject canvasInfo; // Canvas general
    public TextMeshProUGUI textoVecesTocado; // Hijo que muestra las pulsaciones

    private int pulsacionesActuales = 0;
    private bool jugadorEnZona = false;

    public static bool retoCompletado = false;

    private void Start()
    {
        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    private void Update()
    {
        if (!jugadorEnZona) return;

        if (Input.GetKeyDown(teclaAccion))
        {
            pulsacionesActuales++;
            ActualizarTexto();

            if (pulsacionesActuales >= cantidadPulsacionesNecesarias)
            {
                if (enemigoAEliminar != null)
                    Destroy(enemigoAEliminar);

                retoCompletado = true;
                Destroy(this); // Desactiva este script
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            if (canvasInfo != null)
                canvasInfo.SetActive(true);
            ActualizarTexto();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            if (canvasInfo != null)
                canvasInfo.SetActive(false);
        }
    }

    private void ActualizarTexto()
    {
        if (textoVecesTocado != null)
        {
            textoVecesTocado.text = $"Toca la puerta ({pulsacionesActuales}/{cantidadPulsacionesNecesarias})";
        }
    }
}
