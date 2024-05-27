using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float rotationSpeed = 1f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
    }
}
