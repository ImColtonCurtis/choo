using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool levelStarted, levelFailed, levelPassed, restartTap, cheatOn, reloadLevel, inLoading;

    [SerializeField]
    Transform levelObjectsFolder;

    [SerializeField]
    Camera myCam;

    [SerializeField] Transform spawnFolder, tracksTransform;

    [SerializeField] SpriteRenderer chooTitle, chooBG, swipeTitle, swipeBG, whiteSquare, levelPassedSR, levelPassedBG, retryMainText, retryTextBG;

    [SerializeField] Transform[] retryTexts;

    bool restartLogic, startLogic, passedLogic, restartTapLogic, fasterLoad;

    public static int tradcksSpawned;

    [SerializeField] TextMeshPro currentScore;
    [SerializeField] GameObject chestWall, keyWall, singleTrack, starWall, wallBreak;

    [SerializeField] SpriteRenderer[] soundIcons;

    [SerializeField] AudioSource mainMenuMusic, miniGameMusic;

    [SerializeField] GameObject sombrero;

    [SerializeField] Material bgMat;

    // SOUNDS: PlayerController.cs

    // MAKE IT LEVEL BASED
    // on 5's and 10's, player needs to collect key and go back to start
    // max across is 7 (in one go before break)
    // min before a break is 2 (so player can go back and forth to land correctly)
    // sometimes thin landing (end place), other times thick
    // make the end place look cool (like an island or someting)
    // sometimes, break in middle, other times no break

    // first determine track count
    // then determine how many breaks
    // need to store level information on first load, so every reload is same level
    // store each tracks move speed
    // position of wall breaks

    void LevelSpawner()
    {
        bool hasKey = false;
        int tracksCount;
        int breaksCount;
        int[] breaksPosition = new int[3]; // after how many tracks is this break placed (max three breaks per level)
        int currentLevelInt = PlayerPrefs.GetInt("levelCount", 1);
        
        // set-up literature
        if (PlayerPrefs.GetInt("prevLevelCount", 0) != currentLevelInt)
        {
            if (currentLevelInt % 5 == 0)
                PlayerPrefs.SetInt("hasKey", 1);

            // 1 has 2
            // 2 has 3
            // 3 has 2 break 2
            // 4 has 2 break 3
            // 5 has 3 break 2 key
            // 6-9 have max of 1 break with max of 4 tracks in between and total max of 7
            // 10 has 2 break 3 break 2 key
            // 11-19 has max of 2 breaks with max of 4 tracks in between and total max of 9
            // 20-49 has max of 1 break with max of 5 tracks in between and total max of 10
            // 50-74 has max of 2 breaks with max of 4 tracks in between and total max of 12
            // 75-99 has max of 2 breaks with max of 5 tracks in between and total max of 14
            // 100+ has max of 3 breaks with max of 5 tracks in between and total max of 15

            // determine break and track counts
            // and break position
            // number define how many tracks are placed before the break is placed
            PlayerPrefs.SetInt("breaksPosition0", 0);
            PlayerPrefs.SetInt("breaksPosition1", 0);
            PlayerPrefs.SetInt("breaksPosition2", 0);
            if (currentLevelInt >= 100)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 4));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 6));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 11));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0) - 5), Mathf.Min(6, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    case 2:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(6, 16));

                        switch (PlayerPrefs.GetInt("tracksCount", 0))
                        {
                            case 6:
                                PlayerPrefs.SetInt("breaksPosition0", 2);
                                PlayerPrefs.SetInt("breaksPosition1", 4);
                                break;
                            case 7:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 4));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", 5);
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 6));
                                break;
                            case 8:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", 6);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 7));
                                break;
                            case 9:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                break;
                            case 10:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                break;
                            case 11:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                break;
                            case 12:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 11));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                break;
                            case 13:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(3, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(8, 11));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(8, 10));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 8);
                                break;
                            case 14:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(4, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(9, 11));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 9);
                                break;
                            case 15:
                                PlayerPrefs.SetInt("breaksPosition0", 5);
                                PlayerPrefs.SetInt("breaksPosition1", 10);
                                break;
                            default:
                                Debug.Log("error");
                                break;
                        }
                        break;
                    case 3:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(8, 16));

                        switch (PlayerPrefs.GetInt("tracksCount", 0))
                        {
                            case 8:
                                PlayerPrefs.SetInt("breaksPosition0", 2);
                                PlayerPrefs.SetInt("breaksPosition1", 4);
                                PlayerPrefs.SetInt("breaksPosition2", 6);
                                break;
                            case 9:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 4));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", 5);
                                    PlayerPrefs.SetInt("breaksPosition2", 7);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 6));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(6, 8));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 7);
                                }
                                break;
                            case 10:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", 6);
                                    PlayerPrefs.SetInt("breaksPosition2", 8);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 9));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 8);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 7));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(6, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 9));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 8);
                                }
                                break;
                            case 11:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                    PlayerPrefs.SetInt("breaksPosition2", 9);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 10));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 9);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 10));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 10));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 9);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(6, 10));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 10));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 10));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 9);
                                }
                                break;
                            case 12:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 10);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2",  10);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 10);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 10));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 11));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                }
                                break;
                            case 13:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 10));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 11);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 10));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 11);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(7, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 10));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(8, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                }
                                break;
                            case 14:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 11));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 13));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 9)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(11, 13));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", 12);
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 10));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 13));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(11, 13));
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 13));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 4)
                                        PlayerPrefs.SetInt("breaksPosition2", 9);
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 11));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 13));
                                }
                                break;
                            case 15:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 11));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(9, 13));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 14));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 9)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(11, 14));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(12, 14));
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 10));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 8)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 14));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(11, 14));
                                }
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", 10);
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 7)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 14));
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                    if (PlayerPrefs.GetInt("breaksPosition1", 0) == 5)
                                        PlayerPrefs.SetInt("breaksPosition2", 10);
                                    else if (PlayerPrefs.GetInt("breaksPosition1", 0) == 6)
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 12));
                                    else
                                        PlayerPrefs.SetInt("breaksPosition2", Random.Range(10, 13));
                                }
                                break;
                            default:
                                Debug.Log("error");
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt >= 75)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 3));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 6));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 11));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0) - 5), Mathf.Min(6, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    case 2:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(6, 15));

                        switch (PlayerPrefs.GetInt("tracksCount", 0))
                        {
                            case 6:
                                PlayerPrefs.SetInt("breaksPosition0", 2);
                                PlayerPrefs.SetInt("breaksPosition1", 4);
                                break;
                            case 7:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 4));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", 5);
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 6));
                                break;
                            case 8:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", 6);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 7));
                                break;
                            case 9:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                break;
                            case 10:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                break;
                            case 11:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                break;
                            case 12:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 11));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 10));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                break;
                            case 13:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(3, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(8, 11));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(8, 10));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 8);
                                break;
                            case 14:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(4, 6));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 5)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(9, 11));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 9);
                                break;
                            default:
                                Debug.Log("error");
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt >= 50)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 3));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 5));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 9));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0) - 4), Mathf.Min(5, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    case 2:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(6, 13));

                        switch (PlayerPrefs.GetInt("tracksCount", 0))
                        {
                            case 6:
                                PlayerPrefs.SetInt("breaksPosition0", 2);
                                PlayerPrefs.SetInt("breaksPosition1", 4);
                                break;
                            case 7:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 4));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", 5);
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 6));
                                break;
                            case 8:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", 6);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 7));
                                break;
                            case 9:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                break;
                            case 10:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(3, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                break;
                            case 11:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(3, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(7, 9));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", 7);
                                break;
                            case 12:
                                PlayerPrefs.SetInt("breaksPosition0", 4);
                                PlayerPrefs.SetInt("breaksPosition1", 8);
                                break;
                            default:
                                Debug.Log("error");
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt >= 20)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 2));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 6));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 11));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0) - 5), Mathf.Min(6, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt >= 11)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 3));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 5));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 9));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0) - 4), Mathf.Min(5, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    case 2:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(6, 10));
                        switch (PlayerPrefs.GetInt("tracksCount", 0))
                        {
                            case 6:
                                PlayerPrefs.SetInt("breaksPosition0", 2);
                                PlayerPrefs.SetInt("breaksPosition1", 4);
                                break;
                            case 7:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 4));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", 5);
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 6));
                                break;
                            case 8:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", 6);
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(4, 7));
                                break;
                            case 9:
                                PlayerPrefs.SetInt("breaksPosition0", Random.Range(2, 5));
                                if (PlayerPrefs.GetInt("breaksPosition0", 0) == 4)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(6, 8));
                                else if (PlayerPrefs.GetInt("breaksPosition0", 0) == 3)
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 8));
                                else
                                    PlayerPrefs.SetInt("breaksPosition1", Random.Range(5, 7));
                                break;
                            default:
                                Debug.Log("error");
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt == 10)
            {
                PlayerPrefs.SetInt("breaksCount", 2);
                PlayerPrefs.SetInt("tracksCount", 8);
                PlayerPrefs.SetInt("breaksPosition0", 2);
                PlayerPrefs.SetInt("breaksPosition1", 5);
            }
            else if (currentLevelInt >= 6)
            {
                PlayerPrefs.SetInt("breaksCount", Random.Range(0, 2));
                switch (PlayerPrefs.GetInt("breaksCount", 0))
                {
                    case 0:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(2, 5));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("tracksCount", Random.Range(4, 8));
                        PlayerPrefs.SetInt("breaksPosition0", Random.Range(Mathf.Max(2, PlayerPrefs.GetInt("tracksCount", 0)-4), Mathf.Min(5, PlayerPrefs.GetInt("tracksCount", 0) - 1)));
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevelInt == 5)
            {
                PlayerPrefs.SetInt("breaksCount", 1);
                PlayerPrefs.SetInt("tracksCount", 5);
                PlayerPrefs.SetInt("breaksPosition0", 3);
            }
            else if (currentLevelInt == 4)
            {
                PlayerPrefs.SetInt("breaksCount", 1);
                PlayerPrefs.SetInt("tracksCount", 5);
            }
            else if (currentLevelInt == 3)
            {
                PlayerPrefs.SetInt("breaksCount", 1);
                PlayerPrefs.SetInt("tracksCount", 4);
            }
            else if (currentLevelInt == 2)
            {
                PlayerPrefs.SetInt("breaksCount", 0);
                PlayerPrefs.SetInt("tracksCount", 3);
            }
            else if (currentLevelInt == 1)
            {
                PlayerPrefs.SetInt("breaksCount", 0);
                PlayerPrefs.SetInt("tracksCount", 2);
            }

            PlayerPrefs.SetInt("prevLevelCount", currentLevelInt);
        }

        // load literature
        if (PlayerPrefs.GetInt("hasKey", 0) == 1)
            hasKey = true;
        tracksCount = PlayerPrefs.GetInt("tracksCount", 0);
        breaksCount = PlayerPrefs.GetInt("breaksCount", 0);

        breaksPosition[0] = PlayerPrefs.GetInt("breaksPosition0", 0);
        breaksPosition[1] = PlayerPrefs.GetInt("breaksPosition1", 0);
        breaksPosition[2] = PlayerPrefs.GetInt("breaksPosition2", 0);

        // transforms: spawnFolder, tracksTransform;
        // spawn level
        float spawnPos = 6.45f;
        // spawn startingPlatform
        if (currentLevelInt % 5 == 0)
        {
            // chest wall 
            GameObject tempObj = Instantiate(chestWall, new Vector3(0, 0, 0), Quaternion.identity, spawnFolder);
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localPosition = new Vector3(spawnPos, -3.62f, 1.65f);
        }
        else
        {
            // normal wall
            GameObject tempObj = Instantiate(wallBreak, new Vector3(0, 0, 0), Quaternion.identity, spawnFolder);
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localPosition = new Vector3(spawnPos, -3.62f, 1.65f);
        }

        // spawn tracks and breaks
        spawnPos -= 2.15f;
        for (int i = 0; i < tracksCount; i++)
        {
            // track
            GameObject tempObj = Instantiate(singleTrack, new Vector3(0, 0, 0), Quaternion.identity, tracksTransform);
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localPosition = new Vector3(spawnPos, 0, 0);
            spawnPos -= 2.15f;

            // spawn break walls
            if (breaksPosition[0] == i + 1 || breaksPosition[1] == i + 1 || breaksPosition[2] == i + 1)
            {
                GameObject tempObj2 = Instantiate(wallBreak, new Vector3(0, 0, 0), Quaternion.identity, tracksTransform);
                tempObj2.transform.localEulerAngles = Vector3.zero;
                tempObj2.transform.localPosition = new Vector3(spawnPos, -3.62f, 1.65f);
                spawnPos -= 2.15f;
            }
        }

        // spawn ending platform
        if (currentLevelInt % 5 == 0)
        {
            // key wall
            GameObject tempObj = Instantiate(keyWall, new Vector3(0, 0, 0), Quaternion.identity, spawnFolder);
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localPosition = new Vector3(spawnPos, 0, 2.08f);
        }
        else
        {
            // star wall
            GameObject tempObj = Instantiate(starWall, new Vector3(0, 0, 0), Quaternion.identity, spawnFolder);
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localPosition = new Vector3(spawnPos, 0, 0);
        }
    }

    private void Awake()
    {
        //Application.targetFrameRate = 60;

        levelStarted = false;
        levelFailed = false;
        levelPassed = false;
        fasterLoad = false;

        inLoading = false;

        restartLogic = false;
        startLogic = false;
        passedLogic = false;

        reloadLevel = false;

        restartTap = false;
        restartTapLogic = false;

        cheatOn = false;

        miniGameManager.inMiniGame = false;

        tradcksSpawned = 0;

        if (PlayerPrefs.GetInt("eggIsEnabled", 0) == 0) // is off
        {
            sombrero.SetActive(false);
            cheatOn = false;
        }
        else if (PlayerPrefs.GetInt("eggIsEnabled", 0) == 1) // is on
        {
            sombrero.SetActive(true);
            cheatOn = true;
        }

        currentScore.text = "Level " + PlayerPrefs.GetInt("levelCount", 1);

        // change bgMat to dark
        if (PlayerPrefs.GetInt("levelCount", 1) % 5 == 0 && PlayerPrefs.GetInt("levelCount", 1) > 2)
        {
            switch (PlayerPrefs.GetInt("bgInt", 0) % 6)
            {
                case 0:
                    bgMat.color = new Color(0.011808f, 0.011808f, 0.036f, 1);
                    break;
                case 1:
                    bgMat.color = new Color(0.1322581f, 0.05548497f, 0.135f, 1);
                    break;
                case 2:
                    bgMat.color = new Color(0.142f, 0.142f, 0.06943798f, 1);
                    break;
                case 3:
                    bgMat.color = new Color(0.135f, 0.07748999f, 0.07748999f, 1);
                    break;
                case 4:
                    bgMat.color = new Color(0.06508031f, 0.114f, 0.06304198f, 1);
                    break;
                case 5:
                    bgMat.color = new Color(0.04312f, 0.09799997f, 0.09799997f, 1);
                    break;
                default:
                    bgMat.color = new Color(0.011808f, 0.011808f, 0.036f, 1);
                    break;
            }
            if (PlayerPrefs.GetInt("ChangeBG", 0) == 0)
                PlayerPrefs.SetInt("ChangeBG", 1);
        }
        // change bgMat to new color
        else if (PlayerPrefs.GetInt("levelCount", 1) % 5 == 1 && PlayerPrefs.GetInt("levelCount", 1) > 2 && PlayerPrefs.GetInt("ChangeBG", 0) == 1)
        {
            PlayerPrefs.SetInt("ChangeBG", 0);

            PlayerPrefs.SetInt("bgInt", PlayerPrefs.GetInt("bgInt", 0) + 1);

            switch (PlayerPrefs.GetInt("bgInt", 0) % 6)
            {
                case 0:
                    bgMat.color = new Color(0.4139207f, 0.4291233f, 0.87f, 1);
                    break;
                case 1:
                    bgMat.color = new Color(0.8554249f, 0.4156862f, 0.8705882f, 1);
                    break;
                case 2:
                    bgMat.color = new Color(0.681f, 0.681f, 0.424944f, 1);
                    break;
                case 3:
                    bgMat.color = new Color(0.6235294f, 0.3890823f, 0.3890823f, 1);
                    break;
                case 4:
                    bgMat.color = new Color(0.2938729f, 0.482f, 0.2872719f, 1);
                    break;
                case 5:
                    PlayerPrefs.SetInt("bgInt", -1);
                    bgMat.color = new Color(0.296036f, 0.62f, 0.6088288f, 1);
                    break;
                default:
                    bgMat.color = new Color(0.4139207f, 0.4291233f, 0.87f, 1);
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("levelCount", 1) <= 1)
            bgMat.color = new Color(0.4139207f, 0.4291233f, 0.87f, 1);
    }

    private void Start()
    {
        LevelSpawner();
        StartCoroutine(StartLogic());
        PlayerPrefs.SetInt("GamesSinceLastAdPop", PlayerPrefs.GetInt("GamesSinceLastAdPop", 0)+1);
    }

    IEnumerator FadeOutAudio(AudioSource myAudio)
    {
        float timer = 0, totalTime = 24;
        float startingLevel = myAudio.volume;
        while (timer <= totalTime)
        {
            myAudio.volume = Mathf.Lerp(startingLevel, 0, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    private void Update()
    {
        if (reloadLevel)
        {
            fasterLoad = true;
            inLoading = true;
            StartCoroutine(RestartLevel(whiteSquare));
            reloadLevel = false;
        }

        if (cheatOn && PlayerPrefs.GetInt("eggIsEnabled", 0) == 0) // turn on
        {
            sombrero.SetActive(true);
            PlayerPrefs.SetInt("eggIsEnabled", 1);
        }
        else if (!cheatOn && PlayerPrefs.GetInt("eggIsEnabled", 0) == 1) // turn off
        {
            sombrero.SetActive(false);
            PlayerPrefs.SetInt("eggIsEnabled", 0);
        }

        if (!restartLogic && levelFailed && !levelPassed)
        {
            Transform tempObj = retryTexts[Random.Range(0, retryTexts.Length)].transform;
            SpriteRenderer retryTitle, retryBg;
            retryTitle = tempObj.GetComponent<SpriteRenderer>();
            retryBg = tempObj.GetComponentsInChildren<SpriteRenderer>()[1];

            PlayerPrefs.SetInt("FailedInARow", PlayerPrefs.GetInt("FailedInARow", 0) + 1); //

            StartCoroutine(RetryLiterature(retryTitle, retryBg));
            restartLogic = true;
        }

        if (!startLogic && levelStarted)
        {
            foreach (SpriteRenderer sprites in soundIcons)
            {
                StartCoroutine(FadeImageOut(sprites));
            }

            StartCoroutine(FadeOutAudio(mainMenuMusic));

            StartCoroutine(FadeImageOut(chooTitle));
            StartCoroutine(FadeImageOut(chooBG));
            StartCoroutine(FadeImageOut(swipeTitle));
            StartCoroutine(FadeImageOut(swipeBG));
            startLogic = true;
        }

        if (!passedLogic && levelPassed)
        {
            PlayerPrefs.SetInt("FailedInARow", 0); //

            PlayerPrefs.SetInt("levelCount", PlayerPrefs.GetInt("levelCount", 1) + 1); // increment
            StartCoroutine(RetryLiterature(levelPassedSR, levelPassedBG));
            StartCoroutine(RestartWait());
            passedLogic = true;
        }

        if (restartTap && !restartTapLogic)
        {
            StartCoroutine(RestartWait());
            restartTapLogic = true;
        }
    }

    IEnumerator StartLogic()
    {
        whiteSquare.enabled = true;
        whiteSquare.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeImageOut(whiteSquare));
    }

    IEnumerator RetryLiterature(SpriteRenderer mainText, SpriteRenderer bgText)
    {
        float timer = 0, totalTime = 40;
        Color startingColor1 = mainText.color;
        Color startingColor2 = bgText.color;
        Transform textTransform = mainText.gameObject.transform.parent.transform;

        Vector3 startingScale = textTransform.localScale;


        if (levelFailed)
        {
            yield return new WaitForSeconds(0.2f);
            totalTime = 30;
        }

        while (timer <= totalTime)
        {
            if (timer <= 18)
                textTransform.localScale = Vector3.Lerp(startingScale*0.1f, startingScale * 1.7f, timer / (totalTime-18));

            if (timer < totalTime * 0.75f)
            {
                mainText.color = Color.Lerp(startingColor1, new Color(startingColor1.r, startingColor1.g, startingColor1.b, 1), timer / (totalTime*0.7f));
                bgText.color = Color.Lerp(startingColor2, new Color(startingColor2.r, startingColor2.g, startingColor2.b, 1), timer / (totalTime*0.7f));
            }
            if (timer == 12)
            {
                if (levelFailed)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(FadeImageIn(retryMainText, 45));
                    StartCoroutine(FadeImageIn(retryTextBG, 45));
                }
            }

            yield return new WaitForFixedUpdate();
            timer++;
        }

        timer = 0;
        totalTime = 35;
        if (levelPassed)
            totalTime = 85;
        startingScale = textTransform.localScale;
        while (timer <= totalTime)
        {
            textTransform.localScale = Vector3.Lerp(startingScale, new Vector3(startingScale.x*1.15f, startingScale.y*1.5f, startingScale.z*1.5f), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }

        if (levelFailed)
        {
            yield return new WaitForSeconds(0.5f);
            timer = 0;
            totalTime = 30;
            while (timer <= totalTime)
            {
                mainText.color = Color.Lerp(new Color(startingColor1.r, startingColor1.g, startingColor1.b, 1), startingColor1, timer / (totalTime * 0.7f));
                bgText.color = Color.Lerp(new Color(startingColor2.r, startingColor2.g, startingColor2.b, 1), startingColor2, timer / (totalTime * 0.7f));
                yield return new WaitForFixedUpdate();
                timer++;
            }

        }
    }

    IEnumerator RestartWait()
    {
        if (!restartTap)
            yield return new WaitForSecondsRealtime(2.3f);
        else
            yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(RestartLevel(whiteSquare));
    }

    IEnumerator RestartLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.color = new Color(0, 0, 0, 0);
        myImage.enabled = true;
        bool is5th = false;
        if (PlayerPrefs.GetInt("levelCount", 1) % 5 == 1 && PlayerPrefs.GetInt("levelCount", 1) > 2 && levelPassed && !reloadLevel)
            is5th = true;

        while (timer <= totalTime)
        {
            if (is5th)
                myImage.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), timer / totalTime);
            else
                myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }

        if (fasterLoad)
        {
            fasterLoad = false;
            yield return new WaitForSecondsRealtime(0.5f);
            inLoading = false;
        }

        if (is5th)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    IEnumerator FadeImageOut(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        myImage.enabled = false;
    }

    IEnumerator FadeImageIn(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextIn(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }
}
