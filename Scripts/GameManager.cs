using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnInterval = 2f;
    public Transform spawnPoint;
    public float chunkWidth = 10f;
    public int chunkExpand = 1;

    private GameObject[] carPrefabs;
    private GameObject[] chunkPrefabs;
    private float nextSpawnTime;

    [HideInInspector]
    public List<GameObject> cars = new List<GameObject>();

    private Camera mainCamera = null;

    Dictionary<int, GameObject> chunks = new Dictionary<int, GameObject>();

    void LoadChunk(int index)
    {
        if (!chunks.ContainsKey(index))
        {
            GameObject chunk = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)];

            chunks.Add(index, Instantiate(chunk, new Vector3(index * chunkWidth, 0, 0), Quaternion.identity));
        }
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        carPrefabs = Resources.LoadAll<GameObject>("Cars");
        chunkPrefabs = Resources.LoadAll<GameObject>("Chunks");
        
        if (carPrefabs.Length == 0)
        {
            Debug.LogWarning("No Car Prefabs");
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && carPrefabs.Length > 0)
        {
            SpawnCar();
            nextSpawnTime = Time.time + spawnInterval;
        }

        int chunkPosition = (int)Mathf.Floor((mainCamera.transform.position.x/chunkWidth) + 0.5f);

        for (int i = 0; i < 1 + (chunkExpand * 2); i++)
        {
            LoadChunk(chunkPosition + i - chunkExpand);
        }
    }

    void SpawnCar()
    {
        GameObject car = carPrefabs[UnityEngine.Random.Range(0, carPrefabs.Length)];

        cars.Add(Instantiate(car, spawnPoint.position, spawnPoint.rotation));
    }
}
