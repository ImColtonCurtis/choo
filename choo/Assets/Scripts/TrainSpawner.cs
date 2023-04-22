using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] trains = new GameObject[4];

    int mainTrain = 0;
    int thisSpawnerDirection, startAmnt;
    float tracksMoveSpeed = 2;

    float startingX = 0;

    float minTime, maxTime;

    private void OnEnable()
    {
        GameManager.tradcksSpawned++;

        // determine tracks direciton
        thisSpawnerDirection = GameManager.tradcksSpawned % 2;
        // determine tracks speed
        tracksMoveSpeed = Random.Range(2.5f, 7.2f);

        // decide track's main train
        if (PlayerPrefs.GetInt("levelCount", 1) % 5 == 0 && Random.Range(0, 5) > 0)
            mainTrain = 3;
        else
            mainTrain = Random.Range(0, trains.Length-1);

        StartCoroutine(TrainSpawnerLogic());

        if (thisSpawnerDirection == 0)
            startingX = 21;
        else
            startingX = -21;

        // spawn some trains on the track
        startAmnt = Random.Range(2, 5);
        for (int i = 0; i < startAmnt; i++)
            StartingTrain();

        // determine min & max spawn time
        minTime = (((((tracksMoveSpeed-2.5f)/4.7f)*-1)+1)*2.9f)+2; // (when move speed is minium, min spawn time is 4.9f) (when move speed is max, min spawn time is 2)
        maxTime = minTime+(((((tracksMoveSpeed - 2.5f)/4.7f)*-1)+1)*0.5f)+0.5f;
    }

    IEnumerator TrainSpawnerLogic()
    {        
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        SpawnTrain();
        if (!GameManager.levelFailed)
            StartCoroutine(TrainSpawnerLogic());
    }

    void SpawnTrain()
    {
        GameObject tempObj;
        if (Random.Range(0, 4) == 0)
            tempObj = Instantiate(trains[Random.Range(0, trains.Length-1)], Vector3.zero, Quaternion.identity, transform);
        else
            tempObj = Instantiate(trains[mainTrain], Vector3.zero, Quaternion.identity, transform);

        // set position and direction
        if (thisSpawnerDirection == 0) {
            tempObj.GetComponent<TrainMovement>().direction = -1; // going left
            tempObj.transform.localPosition = new Vector3(0, -6.3f, 32f);
            tempObj.transform.localEulerAngles = new Vector3(tempObj.transform.localEulerAngles.x, 90, tempObj.transform.localEulerAngles.z);
        }
        else
        {
            tempObj.transform.localPosition = new Vector3(0, -6.3f, -32f);
        }

        // set trains move speed
        tempObj.GetComponent<TrainMovement>().moveSpeed = tracksMoveSpeed * tempObj.GetComponent<TrainMovement>().direction;
    }

    void StartingTrain()
    {
        GameObject tempObj;
        if (Random.Range(0, 4) == 0)
            tempObj = Instantiate(trains[Random.Range(0, trains.Length-1)], Vector3.zero, Quaternion.identity, transform);
        else
            tempObj = Instantiate(trains[mainTrain], Vector3.zero, Quaternion.identity, transform);

        // set position and direction
        if (thisSpawnerDirection == 0)
        {
            tempObj.GetComponent<TrainMovement>().direction = -1; // going left
            tempObj.transform.localPosition = new Vector3(0, -6.3f, startingX);

            startingX -= 30f / (startAmnt*0.65f);

            tempObj.transform.localEulerAngles = new Vector3(tempObj.transform.localEulerAngles.x, 90, tempObj.transform.localEulerAngles.z);
        }
        else
        {
            tempObj.transform.localPosition = new Vector3(0, -6.3f, startingX);
            tempObj.transform.localEulerAngles = new Vector3(tempObj.transform.localEulerAngles.x, 90, tempObj.transform.localEulerAngles.z);

            startingX += 30f / (startAmnt * 0.65f);
        }

        // set trains move speed
        tempObj.GetComponent<TrainMovement>().moveSpeed = tracksMoveSpeed * tempObj.GetComponent<TrainMovement>().direction;
    }
}
