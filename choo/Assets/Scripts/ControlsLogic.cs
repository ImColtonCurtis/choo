using System.Collections;
using System.Collections.Generic;
using Unity.Services.Mediation.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsLogic : MonoBehaviour
{
    bool touchedDown;

    [SerializeField] GameObject noIcon;

    [SerializeField] Animator soundAnim;

    Vector3 startingTapLoc;
    public static int swipeDirection, currentDirection;
    float swipeRange = 0.01f;

    public static bool playerSwiped;

    int cheatCounter;

    void Awake()
    {
        currentDirection = 1; // 1 is up, 2 is right, 3 is down, 4 is left
        playerSwiped = false;
        touchedDown = false;
        swipeDirection = 0; // 1 is up, 2 is right, 3 is down, 4 is left

        if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
        {
            noIcon.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            noIcon.SetActive(true);
            AudioListener.volume = 0;
        }

        cheatCounter = 0;
    }


    void OnTouchDown(Vector3 point)
    {
        if (!touchedDown && !GameManager.inLoading)
        {
            if (ShowAds.poppedUp)
            {
                if (point.x <= 0)
                    ShowAds.shouldShowRewardedAd = true;
                else
                    ShowAds.dontShow = true;
            }
            else if (ShowAds.skipPoppedUp)
            {
                if (point.x <= 0)
                    ShowAds.shouldShowRewardedAd = true;
                else
                    ShowAds.dontShow = true;
            }
            else
            {
                // cheat: top-right, top-right, top-left, bottom-right
                // top right tap
                if (!GameManager.levelStarted && (cheatCounter == 0 || cheatCounter == 1) && point.x >= 0.03f && point.y >= 8f)
                {
                    cheatCounter++;
                }
                // top left tap
                else if (!GameManager.levelStarted && (cheatCounter == 2) && point.x <= -0.03f && point.y >= 8f)
                {
                    cheatCounter++;
                }
                // bottom right tap
                else if (!GameManager.levelStarted && (cheatCounter == 3) && point.x >= 0.03f && point.y <= 7.92f)
                {
                    cheatCounter = 0;
                    if (!GameManager.cheatOn)
                        GameManager.cheatOn = true;
                    else
                        GameManager.cheatOn = false;
                }
                else if (!GameManager.levelStarted && point.x <= -0.01f && point.y <= 7.92f) // bottom left button clicked
                {
                    if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
                    {
                        PlayerPrefs.SetInt("SoundStatus", 0);
                        noIcon.SetActive(true);
                        AudioListener.volume = 0;
                    }
                    else
                    {
                        PlayerPrefs.SetInt("SoundStatus", 1);
                        noIcon.SetActive(false);
                        AudioListener.volume = 1;
                    }
                    soundAnim.SetTrigger("Blob");
                }
                else
                {
                    if (!GameManager.levelFailed)
                    {
                        touchedDown = true;
                        startingTapLoc = point;
                        if (!GameManager.levelStarted)
                            GameManager.levelStarted = true;
                    }
                    else if (!GameManager.restartTap)
                        GameManager.restartTap = true;

                    if (miniGameManager.levelFailed2 && !miniGameManager.restartTap2)
                        miniGameManager.restartTap2 = true;
                }
            }
        }
    }

    void OnTouchStay(Vector3 point)
    {
        if (touchedDown && ((!GameManager.levelFailed && !GameManager.levelPassed && !miniGameManager.inMiniGame) || (!miniGameManager.levelFailed2 && miniGameManager.inMiniGame)))
        {
            Vector3 Distance = point - startingTapLoc;
            if (Distance.x < -swipeRange)
            {
                touchedDown = false;
                playerSwiped = true;
                swipeDirection = 4;
            }
            else if (Distance.x > swipeRange)
            {
                touchedDown = false;
                playerSwiped = true;
                swipeDirection = 2;
            }
            else if (Distance.y > swipeRange && !miniGameManager.inMiniGame)
            {
                touchedDown = false;
                playerSwiped = true;
                swipeDirection = 1;
            }
            else if (Distance.y < -swipeRange && !miniGameManager.inMiniGame)
            {
                touchedDown = false;
                playerSwiped = true;
                swipeDirection = 3;
            }
        }
    }

    void OnTouchUp()
    {
        if (touchedDown)
        {
            touchedDown = false;        
        }
    }

    void OnTouchExit()
    {
        if (touchedDown)
        {
            touchedDown = false;          
        }
    }
}
