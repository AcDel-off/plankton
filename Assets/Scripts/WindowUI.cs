using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform windowTransform; // ����, ������� ����� �������������
    [SerializeField] private Canvas canvas;                 // Canvas, � ������� ��������� ����

    [SerializeField] private TextMeshProUGUI windowName;

    private Vector2 offset; // �������� ��� ��������������
    [SerializeField] private bool closed = true;
    [SerializeField] private bool isError = false;

    private string[] errorMessages = { "�� ���� �� �� ������?", "���� ��� �� �� ����?", 
        "�� ������ ������ ���� ����������?", "��� �� ��� ���������?", "� ���� ����?",
        "� ��� �������� ����, ��� �� ������ �� ���������", "�� �����������", "�������", "� ���� ����", "� � ��� ��", "�����������" };
    
    private bool errorMoved = false;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas �� ��������! ����������, ��������� Canvas � ����������.");
        }
    }

    // ����� ��� ������ �������������� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(windowTransform, eventData.position, eventData.pressEventCamera, out offset);
        windowTransform.SetSiblingIndex(-1);
    }

    // ����� ��� ��������� �������������� ����
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(windowTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // ������������� ����� ��������� � ������ ��������
        Vector2 newPosition = localPoint - offset;

        // ������������ ��������� ���� � �������� Canvas
        newPosition = ClampToCanvas(newPosition);

        windowTransform.localPosition = newPosition;
    }

    // ����� ��� ����������� ��������� ���� � �������� Canvas
    private Vector2 ClampToCanvas(Vector2 position)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // ������� ���� � Canvas
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float windowWidth = windowTransform.rect.width;
        float windowHeight = windowTransform.rect.height;

        // ������������ ������� �� ����������� � ���������, �������� �������� �� ������� Canvas �� �������� ������� �������
        float clampedX = Mathf.Clamp(position.x, -canvasWidth / 2 + windowWidth / 12, canvasWidth / 2 - windowWidth / 12);
        float clampedY = Mathf.Clamp(position.y, -canvasHeight / 2 + windowHeight / 12, canvasHeight / 2 - windowHeight / 12);

        return new Vector2(clampedX, clampedY);
    }

    // ����� ��� ������ "�������" (������������ ����)
    public void CloseWindow()
    {
        GameObject.Find("BottomPanel").GetComponent<BottomPanel>().CloseWindow(gameObject);
        windowTransform.gameObject.SetActive(false);
        closed = true;
    }

    public void OpenWindow()
    {
        if (closed)
        {
            GameObject.Find("BottomPanel").GetComponent<BottomPanel>().OpenWindow(gameObject);
        }
        if (isError)
        {
            GameObject.Find("BottomPanel").GetComponent<BottomPanel>().ErrorSound();
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(1, 100) > 10 ? "������" : errorMessages[Random.Range(0, errorMessages.Length)];
        }
        windowTransform.gameObject.SetActive(true);
        windowTransform.SetSiblingIndex(-1);
        //windowTransform.SetSiblingIndex(windowTransform.parent.childCount - 2);
        windowName.text = gameObject.name;
        closed = false;
    }

    // ����� ��� ������ "��������" (��������� ������ ����)
    public void MinimizeWindow() => windowTransform.gameObject.SetActive(false);

    // ����� ��� ������ "����������" (���������� ������������ ������ ����)
    public void RestoreWindow()
    {
        if (!gameObject.activeSelf)
        {
            windowTransform.gameObject.SetActive(true);
            windowTransform.SetSiblingIndex(-1);
            //windowTransform.SetSiblingIndex(windowTransform.parent.childCount - 2);
        }
        else
        {
            if (GameObject.Find("BottomPanel").GetComponent<BottomPanel>().GetWindowsCount() == 1)
            {
                MinimizeWindow();
            }
            else
            {
                if (windowTransform.GetSiblingIndex() != windowTransform.parent.childCount - 1) // - 2)
                {
                    windowTransform.SetSiblingIndex(-1);
                    //windowTransform.SetSiblingIndex(windowTransform.parent.childCount - 2);
                }
                else
                {
                    MinimizeWindow();
                }
            }
        }
    }

    public void ErrorMove(float animationTime)
    {
        if (!errorMoved)
            StartCoroutine(ErrorAnimation(animationTime));
        errorMoved = true;
    }

    private IEnumerator ErrorAnimation(float animationTime)
    {
        float timer = 0f;
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float speed = 50f;
        while (timer <= animationTime)
        {
            Vector2 newPosition = (Vector2)windowTransform.localPosition + randomDirection * speed * Time.deltaTime;
            newPosition = ClampToCanvas(newPosition);

            windowTransform.localPosition = newPosition;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public bool GetClosed() => closed;
}
