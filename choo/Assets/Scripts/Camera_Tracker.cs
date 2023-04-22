using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Tracker : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    float smoothTime = 0.05f;
    Vector3 velocity = Vector3.zero;

    Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(offset);

        // Smoothly move the camera towards that target position
        myTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        myTransform.position = new Vector3(Mathf.Clamp(transform.position.x, -18, 13), 0, transform.position.z);
    }
}
