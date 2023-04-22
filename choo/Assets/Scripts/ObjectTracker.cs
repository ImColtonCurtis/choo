using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    [SerializeField] Transform target;
    Transform myTransform;

    Vector3 offset = new Vector3(-0.0523f, -2.908001f, 8.628502f);

    [SerializeField] bool onlyVert;

    private void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myTransform.position = target.position + offset;
        if (onlyVert)
            myTransform.position = new Vector3(0, 0, myTransform.position.z);
    }
}
