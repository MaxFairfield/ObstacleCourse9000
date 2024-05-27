using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public bool self = false;

    public float rotationSpeed = 1f;

    void FixedUpdate()
    {
        if (self)
        {
            transform.Rotate(Vector3.up * rotationSpeed);
        }
        else
        {
            transform.Find("Whacker").Rotate(Vector3.right * rotationSpeed);
        }
    }
}
