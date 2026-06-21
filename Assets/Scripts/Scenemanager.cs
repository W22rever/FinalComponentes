using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevel1 : MonoBehaviour
{
    [SerializeField] string lvName;
    public void LoadLevel1()
    {
        SceneManager.LoadScene(lvName);
    }
}