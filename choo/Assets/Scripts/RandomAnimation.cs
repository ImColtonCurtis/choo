using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] bool isPlane, isRetryHolder, isKnife;
    // Start is called before the first frame update
    void Awake()
    {
        // start idle anim from random spot
        if (!isPlane && !isRetryHolder)
            anim.Play("CoinAnim", 0, Random.Range(0f, 1f));
        else if (isRetryHolder)
            anim.Play("retry_anim", 0, Random.Range(0f, 1f));
        else if (isKnife)
        {
            anim = transform.GetComponent<Animator>();
            anim.Play("knifeSpin", 0, Random.Range(0f, 1f));
        }
        else
            anim.Play("IdleStorePlane", 0, Random.Range(0f, 1f));
    }
}
