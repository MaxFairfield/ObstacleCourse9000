using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);

            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

            transform.position = targetPosition;
        }
    }
}
