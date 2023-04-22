using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator charAnim, cameraShake;
    [SerializeField] Transform posTransform, rotTransform, defaultTransform, actorTranform, tracks;

    float movementSpeed = 0.08f, lastSpawnLocation, spawnZ;

    bool inAction, hasKey;
    GameObject key;
    Transform starObj;

    [SerializeField] SoundManagerLogic mySoundManager;

    [SerializeField] AudioSource[] char_land_sounds, char_jump_sounds, char_ground_sounds, ground_impact_sounds;
    [SerializeField] AudioSource char_chest, char_key, char_lost, char_star;

    private void Start()
    {
        hasKey = false;
        inAction = false;
        lastSpawnLocation = actorTranform.position.z;
        spawnZ = 70;
    }

    private void Update()
    {
        // spawn level
        if (actorTranform.position.z >= lastSpawnLocation + 18)
        {
            spawnZ += 18;
            lastSpawnLocation = actorTranform.position.z;
        }

        if (ControlsLogic.playerSwiped && !inAction && ((!GameManager.levelFailed && !GameManager.levelPassed && !miniGameManager.inMiniGame) || (!miniGameManager.levelFailed2 && miniGameManager.inMiniGame)))
        {
            charAnim.Play("jump", 0, 0.114f);
            char_jump_sounds[Random.Range(0, char_jump_sounds.Length)].Play(); // jump sound

            inAction = true;
            // preform turn
            switch (ControlsLogic.currentDirection)  // 1 is up, 2 is right, 3 is down, 4 is left
            {
                case 1:
                    switch (ControlsLogic.swipeDirection)
                    {
                        case 1:
                            StartCoroutine(MoveForward());
                            break;
                        case 2:
                            StartCoroutine(MoveRight());
                            StartCoroutine(RotateRight());
                            break;
                        case 3:
                            StartCoroutine(MoveBackward());
                            StartCoroutine(Rotate180());
                            break;
                        case 4:
                            StartCoroutine(MoveLeft());
                            StartCoroutine(RotateLeft());
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (ControlsLogic.swipeDirection)
                    {
                        case 1:
                            StartCoroutine(MoveForward());
                            StartCoroutine(RotateLeft());
                            break;
                        case 2:
                            StartCoroutine(MoveRight());
                            break;
                        case 3:
                            StartCoroutine(MoveBackward());
                            StartCoroutine(RotateRight());
                            break;
                        case 4:
                            StartCoroutine(MoveLeft());
                            StartCoroutine(Rotate180());
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    switch (ControlsLogic.swipeDirection)
                    {
                        case 1:
                            StartCoroutine(MoveForward());
                            StartCoroutine(Rotate180());
                            break;
                        case 2:
                            StartCoroutine(MoveRight());
                            StartCoroutine(RotateLeft());
                            break;
                        case 3:
                            StartCoroutine(MoveBackward());
                            break;
                        case 4:
                            StartCoroutine(MoveLeft());
                            StartCoroutine(RotateRight());
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (ControlsLogic.swipeDirection)
                    {
                        case 1:
                            StartCoroutine(MoveForward());
                            StartCoroutine(RotateRight());
                            break;
                        case 2:
                            StartCoroutine(MoveRight());
                            StartCoroutine(Rotate180());
                            break;
                        case 3:
                            StartCoroutine(MoveBackward());
                            StartCoroutine(RotateLeft());
                            break;
                        case 4:
                            StartCoroutine(MoveLeft());
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            ControlsLogic.currentDirection = ControlsLogic.swipeDirection;
            ControlsLogic.playerSwiped = false;
        }
        else if (ControlsLogic.playerSwiped && inAction)
            ControlsLogic.playerSwiped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Train" && ((!GameManager.levelFailed && !miniGameManager.inMiniGame) || (!miniGameManager.levelFailed2 && miniGameManager.inMiniGame)))
        {
            cameraShake.SetTrigger("shake");

            char_land_sounds[Random.Range(0, char_land_sounds.Length)].Play(); // landing sound
            ground_impact_sounds[Random.Range(0, ground_impact_sounds.Length)].Play(); // landing sound
            
            charAnim.SetTrigger("grounded");
            if (actorTranform.parent != collision.gameObject.transform)
                actorTranform.SetParent(collision.gameObject.transform);
        }
        if (collision.gameObject.tag == "Lost" && ((!GameManager.levelFailed && !GameManager.levelPassed && !miniGameManager.inMiniGame) || (!miniGameManager.levelFailed2 && miniGameManager.inMiniGame)))
        {
            char_ground_sounds[Random.Range(0, char_ground_sounds.Length)].Play(); // ground smack sound
            ground_impact_sounds[Random.Range(0, ground_impact_sounds.Length)].Play(); // landing sound

            char_lost.Play(); // landing sound

            mySoundManager.Play("lost_jingle"); // losing jingle
            cameraShake.SetTrigger("shake");
            if (!miniGameManager.inMiniGame)
                GameManager.levelFailed = true;
            else
                miniGameManager.levelFailed2 = true;
            actorTranform.SetParent(defaultTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasKey)
        {
            // Gathering Key logic
            if (other.tag == "Key_Wood" && !GameManager.levelFailed)
            {

                char_key.Play(); // key sound
                mySoundManager.Play("key_get"); // key jingle
                hasKey = true;
                key = other.gameObject;
                key.transform.parent.GetComponent<KeyTracker>().target = transform.parent.parent.transform;
                key.transform.parent.GetComponent<KeyTracker>().keyTriggered = true;
            }
            else if (other.tag == "Key_Bronze" && !GameManager.levelFailed)
            {
                char_key.Play(); // key sound
                mySoundManager.Play("key_get"); // key jingle
                hasKey = true;
                key = other.gameObject;
                key.transform.parent.GetComponent<KeyTracker>().target = transform.parent.parent.transform;
                key.transform.parent.GetComponent<KeyTracker>().keyTriggered = true;
            }
            else if (other.tag == "Key_Silver" && !GameManager.levelFailed)
            {
                char_key.Play(); // key sound
                mySoundManager.Play("key_get"); // key jingle
                hasKey = true;
                key = other.gameObject;
                key.transform.parent.GetComponent<KeyTracker>().target = transform.parent.parent.transform;
                key.transform.parent.GetComponent<KeyTracker>().keyTriggered = true;
            }
            else if (other.tag == "Key_Gold" && !GameManager.levelFailed)
            {
                char_key.Play(); // key sound
                mySoundManager.Play("key_get"); // key jingle
                hasKey = true;
                key = other.gameObject;
                key.transform.parent.GetComponent<KeyTracker>().target = transform.parent.parent.transform;
                key.transform.parent.GetComponent<KeyTracker>().keyTriggered = true;
            }
        }
        if (other.tag == "Chest" && hasKey && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            char_chest.Play(); // chest sound

            mySoundManager.Play("chest_open"); // chest jingle
            GameManager.levelPassed = true;
            StartCoroutine(OpenChest(other.gameObject));
        }

        if (other.tag == "Star" && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            char_star.Play(); // star sound

            mySoundManager.Play("star_get"); // star jingle
            GameManager.levelPassed = true;
            starObj = other.transform;
            StartCoroutine(StarLogic());
        }
    }

    IEnumerator StarLogic()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 startingSize = starObj.localScale;
        Vector3 startingPos = starObj.position;
        float timer = 0, totalTimer = 20;
        while (timer <= totalTimer)
        {
            starObj.localScale = Vector3.Lerp(startingSize, Vector3.zero, timer / totalTimer); // shrink
            starObj.position = Vector3.Lerp(startingPos, transform.position, timer / totalTimer); // move inside
            yield return new WaitForFixedUpdate();
            timer++;
        }
        yield return new WaitForSeconds(0.1f);
        GameManager.levelPassed = true;
    }

    IEnumerator OpenChest(GameObject other)
    {
        key.transform.parent.GetComponent<KeyTracker>().chestTriggered = true;
        yield return new WaitForSeconds(0.85f);
        GameObject chestObj = other;
        chestObj.GetComponent<ChestLogic>().chestAnim.SetTrigger("Open");
        chestObj.GetComponent<ChestLogic>().particlesObj.SetActive(true);
        GameManager.levelPassed = true;
    }

    IEnumerator TurnActionOff()
    {
        yield return new WaitForSeconds(0.06f);
        inAction = false;
    }

    IEnumerator RotateLeft()
    {
        float endingAngle = rotTransform.transform.localEulerAngles.y-90;
        float amountRotated = 0, rotationSpeed = 2.55f*2;
        while (amountRotated < 90)
        {
            rotTransform.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
            amountRotated += rotationSpeed;
            yield return new WaitForFixedUpdate();
        }
        rotTransform.transform.localEulerAngles = new Vector3(rotTransform.transform.localEulerAngles.x, endingAngle,
            rotTransform.transform.localEulerAngles.z);
    }

    IEnumerator RotateRight()
    {
        float endingAngle = rotTransform.transform.localEulerAngles.y + 90;
        float amountRotated = 0, rotationSpeed = 2.55f*2;
        while (amountRotated < 90)
        {
            rotTransform.transform.Rotate(new Vector3(0, rotationSpeed, 0));
            amountRotated += rotationSpeed;
            yield return new WaitForFixedUpdate();
        }
        rotTransform.transform.localEulerAngles = new Vector3(rotTransform.transform.localEulerAngles.x, endingAngle,
            rotTransform.transform.localEulerAngles.z);
    }

    IEnumerator Rotate180()
    {
        float endingAngle = rotTransform.transform.localEulerAngles.y - 180;
        float amountRotated = 0, rotationSpeed = 5.1f*2;
        float direction = 1;
        if (Random.Range(0, 2) == 0)
            direction = -1;
        while (amountRotated < 180)
        {
            rotTransform.transform.Rotate(new Vector3(0, rotationSpeed*direction, 0));
            amountRotated += rotationSpeed;
            yield return new WaitForFixedUpdate();
        }
        rotTransform.transform.localEulerAngles = new Vector3(rotTransform.transform.localEulerAngles.x, endingAngle,
            rotTransform.transform.localEulerAngles.z);
        yield return new WaitForFixedUpdate();
    }

    // moves by 2.15f
    IEnumerator MoveLeft()
    {
        float endingPosition = posTransform.transform.position.x - 2.15f;
        float amountMoved = 0;
        yield return new WaitForSeconds(0.15f);
        while (amountMoved < 2.15f)
        {
            posTransform.transform.Translate(new Vector3(-movementSpeed, 0, 0));
            amountMoved += movementSpeed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TurnActionOff());
    }

    IEnumerator MoveRight()
    {
        float endingPosition = posTransform.transform.position.x + 2.15f;
        float amountMoved = 0;
        yield return new WaitForSeconds(0.15f);
        while (amountMoved < 2.15f)
        {
            posTransform.transform.Translate(new Vector3(movementSpeed, 0, 0));
            amountMoved += movementSpeed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TurnActionOff());
    }

    IEnumerator MoveForward()
    {
        float endingPosition = posTransform.transform.position.z + 2.15f;
        float amountMoved = 0;
        yield return new WaitForSeconds(0.15f);
        while (amountMoved < 2.15f)
        {
            posTransform.transform.Translate(new Vector3(0, 0, movementSpeed));
            amountMoved += movementSpeed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TurnActionOff());
    }

    IEnumerator MoveBackward()
    {
        float endingPosition = posTransform.transform.position.z - 2.15f;
        float amountMoved = 0;
        yield return new WaitForSeconds(0.15f);
        while (amountMoved < 2.15f)
        {
            posTransform.transform.Translate(new Vector3(0, 0, -movementSpeed));
            amountMoved += movementSpeed;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TurnActionOff());
    }
}
