using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLogic : MonoBehaviour
{
    public Animator chestAnim;
    public GameObject particlesObj;

    [SerializeField] GameObject[] chests;

    private void Awake()
    {
        chests[0].SetActive(false);
        chests[1].SetActive(false);
        chests[2].SetActive(false);
        chests[3].SetActive(false);
        if (PlayerPrefs.GetInt("levelCount", 1) % 50 == 0)
            chests[3].SetActive(true);
        else if (PlayerPrefs.GetInt("levelCount", 1) % 20 == 0)
            chests[2].SetActive(true);
        else if (PlayerPrefs.GetInt("levelCount", 1) % 10 == 0)
            chests[1].SetActive(true);
        else
            chests[0].SetActive(true);
    }
}
