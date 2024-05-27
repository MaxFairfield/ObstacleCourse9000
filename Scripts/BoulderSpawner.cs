using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    public GameObject boulder;
    public float spawnInterval = 1f;
    public float variation = 5f;

    private List<GameObject> boulders = new List<GameObject>();

    IEnumerator SpawnBoulders()
    {
        while (true)
        {
            GameObject boulderino = Instantiate(boulder, transform.position, transform.rotation);
            boulderino.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-variation, variation), 0, 0);
            boulders.Add(boulderino);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Start()
    {
        StartCoroutine(SpawnBoulders());
    }

    void Update()
    {
        List<GameObject> bouldersToRemove = new List<GameObject>();

        foreach (GameObject boulderino in boulders)
        {
            if (boulderino.transform.position.y < -5f)
            {
                bouldersToRemove.Add(boulderino);
            }
        }

        foreach (GameObject boulderino in bouldersToRemove)
        {
            boulders.Remove(boulderino);
            Destroy(boulderino);
        }
    }
}
