using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour
{
    public Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject particles;

    float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    Transform myTransform;

    public bool keyTriggered, chestTriggered;

    bool shrinkBegun, moveBegin;

    [SerializeField] GameObject[] keys;

    private void Awake()
    {
        myTransform = transform;
        keyTriggered = false;
        chestTriggered = false;
        shrinkBegun = false;
        moveBegin = false;

        keys[0].SetActive(false);
        keys[1].SetActive(false);
        keys[2].SetActive(false);
        keys[3].SetActive(false);
        if (PlayerPrefs.GetInt("levelCount", 1) % 50 == 0)
            keys[3].SetActive(true);
        else if (PlayerPrefs.GetInt("levelCount", 1) % 20 == 0)
            keys[2].SetActive(true);
        else if (PlayerPrefs.GetInt("levelCount", 1) % 10 == 0)
            keys[1].SetActive(true);
        else
            keys[0].SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (keyTriggered && !chestTriggered)
        {
            particles.SetActive(true);

            // Define a target position above and behind the target transform
            Vector3 targetPosition = target.TransformPoint(offset);

            // Smoothly move the camera towards that target position
            myTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else if (chestTriggered)
        {
            if (!shrinkBegun)
            {
                StartCoroutine(ShrinkKey());
                shrinkBegun = true;
            }

            if (moveBegin)
            {
                // Define a target position above and behind the target transform
                Vector3 targetPosition = target.position;

                // Smoothly move the camera towards that target position
                myTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }

    IEnumerator ShrinkKey()
    {
        float timer = 0, totalTime = 50;
        
        Vector3 startinScale = myTransform.localScale;
        yield return new WaitForSeconds(0.4f);
        moveBegin = true;
        while (timer <= totalTime)
        {
            myTransform.localScale = Vector3.Lerp(startinScale, Vector3.zero, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }
}