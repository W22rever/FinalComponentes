using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private void Start()
    {
        StartCoroutine(TimeToSpawn());
    }

    IEnumerator TimeToSpawn()
    {
        yield return new WaitForSecondsRealtime(10);
        GameObject spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

        BehaviourEnemy behaviourEnemy = spawnedEnemy.GetComponent<BehaviourEnemy>();
        if (behaviourEnemy != null)
        {
            behaviourEnemy.spawner = this; // Asignamos referencia al spawner
            StartCoroutine(behaviourEnemy.Disappear());
        }
    }

    public IEnumerator TimeToReSpawn()
    {
        yield return new WaitForSecondsRealtime(5);
        StartCoroutine(TimeToSpawn());
    }
}
