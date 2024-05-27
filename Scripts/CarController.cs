using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject closestCar;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Collider2D carCollider;

    public float maxSpeed = 5f;
    public float stoppingDistance = 2f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        closestCar = null;
        Collider2D currentCollider = null;

        float closestX = float.MaxValue;
        bool foundClosestCar = false;

        foreach (GameObject car in gameManager.cars)
        {
            if (car == gameObject)
            {
                continue;
            }

            float carX = car.transform.position.x;

            if (carX > transform.position.x && carX < closestX)
            {
                closestCar = car;
                closestX = carX;
                currentCollider = car.GetComponent<Collider2D>();
                foundClosestCar = true;
            }
        }

        float distance = float.MaxValue;

        if (foundClosestCar && currentCollider != null)
        {
            distance = Mathf.Abs(transform.position.x - closestCar.transform.position.x) - (carCollider.bounds.size.x / 2 + currentCollider.bounds.size.x / 2);
        }

        float speed = Mathf.Max(0, Mathf.Min(maxSpeed, distance - stoppingDistance));

        rb.velocity = new Vector2(speed, 0);
    }

}