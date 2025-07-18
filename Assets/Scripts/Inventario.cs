using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    private HashSet<string> llaves = new HashSet<string>();

    public void AgregarLlave(string id)
    {
        llaves.Add(id);
    }

    public bool TieneLlave(string id)
    {
        return llaves.Contains(id);
    }

    public void EliminarLlave(string id)
    {
        if (llaves.Contains(id))
        {
            llaves.Remove(id);
        }
    }
}