using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnPrefab;
    Transform[] SpawnPoints;

    void Start()
    {
        SpawnPoints = GetComponentsInChildren<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        // This starts at index 1 because Unity is stupid and GetComponentsInChildren returns itself at index 0
        for (int i = 1; i<SpawnPoints.Length; ++i)
        {
            Instantiate(SpawnPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation);
        }
        Destroy(gameObject);
    }
}
