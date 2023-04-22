using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    Transform myTransform;

    public int direction = 1;

    [SerializeField] AudioSource trainSounds;

    // Start is called before the first frame update
    void Awake()
    {
        myTransform = transform;

        trainSounds.volume = Random.Range(0.05f, 0.1f);
        trainSounds.pitch += Random.Range(-0.02f, 0.02f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myTransform.localPosition += new Vector3(0, 0, moveSpeed/65f);
    }

    private void Update()
    {
        // mange despawning
        if (myTransform.position.x > 35 || myTransform.position.x < -35)
            Destroy(gameObject);
    }

}
