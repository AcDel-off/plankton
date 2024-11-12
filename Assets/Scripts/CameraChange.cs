using System.Collections;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private CameraRotation cameraFocused;
    [SerializeField] private CameraRotation cameraMovable;

    [SerializeField] private int fieldOfViewFocused = 54;
    [SerializeField] private int fieldOfViewMovable = 70;

    [SerializeField] private KeyCode camChangeKey = KeyCode.None;
    [SerializeField] private float animationTime = 0.5f;
    private bool isFocused = true, isRotating = false, secondTurn = false;

    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(camChangeKey) && !isRotating)
        {
            ChangeCameraState();
        }
    }

    private void ChangeCameraState()
    {
        isRotating = true;

        if (isFocused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            cameraFocused.enabled = false;
            cameraMovable.enabled = true;
            StartCoroutine(ChangeState(fieldOfViewFocused, fieldOfViewMovable));
        }
        else
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);
            Debug.Log(hit.transform);

            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("screen"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    cameraFocused.enabled = true;
                    cameraMovable.enabled = false;
                    StartCoroutine(ChangeState(fieldOfViewMovable, fieldOfViewFocused));
                }
            }
        }

        isRotating = false;
    }

    private IEnumerator ChangeState(int fieldOfViewFrom, int fieldOfViewTo)
    {
        float animTimer = 0f;
        while (animTimer <= animationTime)
        {
            cam.fieldOfView = Mathf.Lerp(fieldOfViewFrom, fieldOfViewTo, animTimer / animationTime);
            animTimer += Time.deltaTime;
            yield return null;
        }
        isFocused = !isFocused;
    }

    public void StartTurnChecking()
    {
        StartCoroutine(TurnChecker());
    }

    private IEnumerator TurnChecker()
    {
        float timer = 0f;
        bool firstLook = false, secondLook = false, textChanged = false, turnToNewText = false, backSecondTime = false;
        while (isFocused)
        {
            timer += Time.deltaTime;
            if(timer >= 7)
            {
                firstLook = true;
                break;
            }
            yield return null;
        }
        while (!isFocused)
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);

            if (hit.transform != null)
            {
                if (!textChanged)
                {
                    if (hit.transform.CompareTag("back"))
                    {
                        GameObject.Find("QuestManager").GetComponent<QuestManager>().ChangeOCText("ты видишь меня?");
                        textChanged = true;
                    }
                }
                else
                {
                    if (!turnToNewText)
                    {
                        if (hit.transform.CompareTag("front") || hit.transform.CompareTag("screen"))
                        {
                            turnToNewText = true;
                            timer = 0f;
                        }
                    }
                    else
                    {
                        timer += Time.deltaTime;
                        if (timer >= 5)
                        {
                            firstLook = true;
                            break;
                        }

                        if (hit.transform.CompareTag("back"))
                        {
                            backSecondTime = true;
                            break;
                        }
                    }
                }
            }
            yield return null;
        }
        if (!firstLook)
        {
            timer = 0f;
            while (isFocused)
            {
                timer += Time.deltaTime;
                if (timer >= 5)
                {
                    secondLook = true;
                    break;
                }
                yield return null;
            }
        }
        if (!secondLook && !firstLook)
        {
            while (!isFocused)
            {
                RaycastHit hit;
                Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);

                if (hit.transform != null)
                {
                    if (!backSecondTime)
                    {
                        if (hit.transform.CompareTag("back"))
                        {
                            backSecondTime = true;
                        }
                    }
                    else
                    {
                        if (hit.transform.CompareTag("front") || hit.transform.CompareTag("screen"))
                        {
                            break;
                        }
                    }
                }
                yield return null;
            }
        }

        GameObject.Find("QuestManager").GetComponent<QuestManager>().ChangeOCText("/OC");
        GameObject.Find("BottomPanel").GetComponent<BottomPanel>().TurnOffRestartingScreen();
        GameObject.Find("QuestManager").GetComponent<QuestManager>().ScreamerSmile();
        Debug.LogWarning("SCREAMER!!!!");
    }

    public void StartSecondTurnChecking()
    {
        if (!secondTurn)
        {
            StartCoroutine(SecondTurnChecker());
            secondTurn = true;
        }
    }

    private IEnumerator SecondTurnChecker()
    {
        bool turnedBack = false, screamerSeen = false;
        while (!screamerSeen)
        {
            if (!isFocused)
            {
                RaycastHit hit;
                Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);

                if (hit.transform != null)
                {
                    if (!turnedBack)
                    {
                        if (hit.transform.CompareTag("back"))
                        {
                            turnedBack = true;
                        }
                    }
                    else
                    {
                        if (hit.transform.CompareTag("front") || hit.transform.CompareTag("screen"))
                        {
                            break;
                        }
                    }
                }
            }
            yield return null;
        }

        GameObject.Find("QuestManager").GetComponent<QuestManager>().ScreamerBackPerson(true);

        while (!screamerSeen)
        {
            if (!isFocused)
            {
                RaycastHit hit;
                Physics.Raycast(cam.transform.position, cam.transform.forward, out hit);

                if (hit.transform != null)
                {
                    if (hit.transform.CompareTag("back"))
                    {
                        screamerSeen = true;
                    }
                }
            }
            yield return null;
        }

        float timer = 0f;
        Debug.LogWarning("SCREAMER!!!!");

        soundManager.ScreamerBackSound(); //запуск звука СКРИМЕРА

        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        GameObject.Find("QuestManager").GetComponent<QuestManager>().ScreamerBackPerson(false);
    }
}
