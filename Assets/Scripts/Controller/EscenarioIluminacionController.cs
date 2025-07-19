using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EscenarioIluminacionController : MonoBehaviour
{
    [SerializeField] private GameObject padreLuces;

    public void ActivarLucesMaximas()
    {
        if (padreLuces == null)
        {
            Debug.LogWarning("No se ha asignado el objeto padre de luces.");
            return;
        }

        // Obtener TODAS las luces 2D hijas, incluso si están desactivadas
        Light2D[] luces = padreLuces.GetComponentsInChildren<Light2D>(true);
        int total = 0;

        Lightis[] scripts = padreLuces.GetComponentsInChildren<Lightis>(true);
        foreach (var s in scripts)
        {
            s.controlExterno = true;
        }

        foreach (Light2D luz in luces)
        {
            luz.gameObject.SetActive(true); // Asegura que el GameObject está activo
            luz.enabled = true;             // Activa el componente Light2D
            luz.intensity = 1f;             // Fuerza intensidad al máximo
            total++;
        }

        Debug.Log($"Se forzaron {total} luces 2D a encenderse con intensidad 1.");
    }
}