using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform windowTransform; // Окно, которое будем перетаскивать
    [SerializeField] private Canvas canvas;                 // Canvas, в котором находится окно

    [SerializeField] private TextMeshProUGUI windowName;

    private Vector2 offset; // Смещение при перетаскивании
    [SerializeField] private bool closed = true;
    [SerializeField] private bool isError = false;

    private string[] errorMessages = { "об этом ли ты мечтал?", "тебе это всё по душе?", 
        "ты правда хочешь этим заниматься?", "это ли твоё призвание?", "а ради чего?",
        "а они говорили тебе, что ты ничего не добьёшься", "ты ничтожество", "забавно", "я вижу тебя", "я – это ты", "ничтожество" };
    
    private bool errorMoved = false;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas не назначен! Пожалуйста, назначьте Canvas в инспекторе.");
        }
    }

    // Метод для начала перетаскивания окна
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(windowTransform, eventData.position, eventData.pressEventCamera, out offset);
        windowTransform.SetSiblingIndex(-1);
    }

    // Метод для обработки перетаскивания окна
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(windowTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // Устанавливаем новое положение с учетом смещения
        Vector2 newPosition = localPoint - offset;

        // Ограничиваем положение окна в пределах Canvas
        newPosition = ClampToCanvas(newPosition);

        windowTransform.localPosition = newPosition;
    }

    // Метод для ограничения положения окна в пределах Canvas
    private Vector2 ClampToCanvas(Vector2 position)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Размеры окна и Canvas
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float windowWidth = windowTransform.rect.width;
        float windowHeight = windowTransform.rect.height;

        // Ограничиваем позицию по горизонтали и вертикали, позволяя выходить за границу Canvas на половину размера объекта
        float clampedX = Mathf.Clamp(position.x, -canvasWidth / 2 + windowWidth / 12, canvasWidth / 2 - windowWidth / 12);
        float clampedY = Mathf.Clamp(position.y, -canvasHeight / 2 + windowHeight / 12, canvasHeight / 2 - windowHeight / 12);

        return new Vector2(clampedX, clampedY);
    }

    // Метод для кнопки "Закрыть" (деактивирует окно)
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
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(1, 100) > 10 ? "Ошибка" : errorMessages[Random.Range(0, errorMessages.Length)];
        }
        windowTransform.gameObject.SetActive(true);
        windowTransform.SetSiblingIndex(-1);
        //windowTransform.SetSiblingIndex(windowTransform.parent.childCount - 2);
        windowName.text = gameObject.name;
        closed = false;
    }

    // Метод для кнопки "Свернуть" (уменьшает высоту окна)
    public void MinimizeWindow() => windowTransform.gameObject.SetActive(false);

    // Метод для кнопки "Развернуть" (возвращает оригинальный размер окна)
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
