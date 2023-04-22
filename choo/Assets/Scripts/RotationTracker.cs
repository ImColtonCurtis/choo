using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTracker : MonoBehaviour
{
    [SerializeField] Transform target;

    float smoothTime = 0;
    Vector3 velocity = Vector3.zero;

    Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // rotate
        myTransform.localEulerAngles = Vector3.SmoothDamp(myTransform.localEulerAngles, target.eulerAngles, ref velocity, smoothTime);
    }
}