using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lightis : MonoBehaviour
{
    private Light2D luz;

    void Awake()
    {
        luz = GetComponent<Light2D>();
        luz.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            luz.enabled = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            luz.enabled = false;
    }
}