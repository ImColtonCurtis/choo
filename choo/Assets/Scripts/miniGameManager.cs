using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class miniGameManager : MonoBehaviour
{
    public static bool levelFailed2, restartTap2, cheatOn2, spawnKnife, inMiniGame;

    [SerializeField]
    Transform levelObjectsFolder;

    [SerializeField] Transform spawnFolder, tracksTransform;

    [SerializeField] Transform[] retryTexts;

    [SerializeField] SpriteRenderer whiteSquare, retryMainText, retryTextBG;

    bool restartLogic, startLogic, passedLogic, restartTapLogic;

    [SerializeField] TextMeshPro currentScore;

    [SerializeField] AudioSource miniGameMusic;

    [SerializeField] GameObject sombrero, knifeObj;

    public static int miniGameScore;

    // SOUNDS: PlayerController.cs

    private void Awake()
    {
        currentScore.text = "highscore " + PlayerPrefs.GetInt("miniGameHighScore", 0);

        spawnKnife = false;
        inMiniGame = true;
        levelFailed2 = false;
        restartTap2 = false;

        if (PlayerPrefs.GetInt("eggIsEnabled", 0) == 0) // is off
        {
            sombrero.SetActive(false);
            cheatOn2 = false;
        }
        else if (PlayerPrefs.GetInt("eggIsEnabled", 0) == 1) // is on
        {
            sombrero.SetActive(true);
            cheatOn2 = true;
        }
        miniGameScore = 0;
    }

    private void Start()
    {
        StartCoroutine(FadeHighScoreOut(currentScore));
        whiteSquare.enabled = true;
        whiteSquare.color = Color.white;
        StartCoroutine(FadeImageOut(whiteSquare));
    }

    private void Update()
    {
        // speed up music
        miniGameMusic.pitch = 1 + (0.25f * (Mathf.Clamp((float)miniGameManager.miniGameScore, 0, 100f) / 100f));

        if (GameManager.cheatOn && PlayerPrefs.GetInt("eggIsEnabled", 0) == 0) // turn on
        {
            sombrero.SetActive(true);
            PlayerPrefs.SetInt("eggIsEnabled", 1);
        }
        else if (!GameManager.cheatOn && PlayerPrefs.GetInt("eggIsEnabled", 0) == 1) // turn off
        {
            sombrero.SetActive(false);
            PlayerPrefs.SetInt("eggIsEnabled", 0);
        }

        if (!restartLogic && levelFailed2)
        {
            Transform tempObj = retryTexts[Random.Range(0, retryTexts.Length)].transform;
            SpriteRenderer retryTitle, retryBg;
            retryTitle = tempObj.GetComponent<SpriteRenderer>();
            retryBg = tempObj.GetComponentsInChildren<SpriteRenderer>()[1];

            StartCoroutine(RetryLiterature(retryTitle, retryBg));

            StartCoroutine(FadeOutAudio(miniGameMusic));

            StartCoroutine(RestartWait());
            restartLogic = true;
        }

        if (restartTap2 && !restartTapLogic)
        {
            StartCoroutine(RestartWait());
            restartTapLogic = true;
        }

        if (spawnKnife)
        {
            Instantiate(knifeObj);
            spawnKnife = false;
        }

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

    IEnumerator ScoreAdder()
    {
        while (!levelFailed2)
        {
            yield return new WaitForSecondsRealtime(1);
            miniGameScore++;
            currentScore.text = miniGameScore + "";
        }
        if (miniGameScore > PlayerPrefs.GetInt("miniGameHighScore", 0))
            PlayerPrefs.SetInt("miniGameHighScore", miniGameScore);
    }

    IEnumerator FadeHighScoreOut(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;

        yield return new WaitForSecondsRealtime(2f);

        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        Instantiate(knifeObj);
        yield return new WaitForSecondsRealtime(0.5f);
        myTtext.text = "0";
        timer = 0;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        StartCoroutine(ScoreAdder());
    }

    IEnumerator RetryLiterature(SpriteRenderer mainText, SpriteRenderer bgText)
    {
        float timer = 0, totalTime = 40;
        Color startingColor1 = mainText.color;
        Color startingColor2 = bgText.color;
        Transform textTransform = mainText.gameObject.transform.parent.transform;

        Vector3 startingScale = textTransform.localScale;


        if (levelFailed2)
        {
            yield return new WaitForSeconds(0.2f);
            totalTime = 30;
        }

        while (timer <= totalTime)
        {
            if (timer <= 18)
                textTransform.localScale = Vector3.Lerp(startingScale * 0.1f, startingScale * 1.7f, timer / (totalTime - 18));

            if (timer < totalTime * 0.75f)
            {
                mainText.color = Color.Lerp(startingColor1, new Color(startingColor1.r, startingColor1.g, startingColor1.b, 1), timer / (totalTime * 0.7f));
                bgText.color = Color.Lerp(startingColor2, new Color(startingColor2.r, startingColor2.g, startingColor2.b, 1), timer / (totalTime * 0.7f));
            }
            if (timer == 12)
            {
                if (levelFailed2)
                {
                    yield return new WaitForSeconds(0.2f);
                    //StartCoroutine(FadeImageIn(retryMainText, 45));
                    //StartCoroutine(FadeImageIn(retryTextBG, 45));
                }
            }

            yield return new WaitForFixedUpdate();
            timer++;
        }

        timer = 0;
        totalTime = 35;
        startingScale = textTransform.localScale;
        while (timer <= totalTime)
        {
            textTransform.localScale = Vector3.Lerp(startingScale, new Vector3(startingScale.x * 1.15f, startingScale.y * 1.5f, startingScale.z * 1.5f), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }

        if (levelFailed2)
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
        if (!restartTap2)
            yield return new WaitForSecondsRealtime(2.3f);
        else
            yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(RestartLevel(whiteSquare));
    }

    IEnumerator RestartLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }

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
