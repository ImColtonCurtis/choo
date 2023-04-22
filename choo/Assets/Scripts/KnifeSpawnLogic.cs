using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnLogic : MonoBehaviour
{
    [SerializeField] GameObject[] knifes;
    float moveSpeed;
    bool spawnedOne;

    private void Awake()
    {
        spawnedOne = false;
        moveSpeed = Random.Range(5f + (2.5f * (Mathf.Clamp((float)miniGameManager.miniGameScore, 0, 100f) / 100f)), 7.5f +  (2.5f* (Mathf.Clamp((float)miniGameManager.miniGameScore,0,100f)/100f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject knife in knifes)
            knife.SetActive(true);

        knifes[Random.Range(0, knifes.Length)].SetActive(false);
    }

    private void FixedUpdate()
    {
        if (transform.position.y > -5)
        {
            transform.Translate(new Vector3(0, -moveSpeed, 0) * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
        if (!spawnedOne && transform.position.y < -3.5f)
        {
            if (!miniGameManager.levelFailed2)
                miniGameManager.spawnKnife = true;
            spawnedOne = true;
        }
    }
}
