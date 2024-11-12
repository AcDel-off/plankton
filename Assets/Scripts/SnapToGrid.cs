using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class SnapToGrid : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition; //исходная позиция объекта перед перетаскиванием
    private WindowIconManager iconManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        iconManager = GameObject.Find("GridContainer").GetComponent<WindowIconManager>();
    }

    //метод для перетаскивания объекта
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out mousePos);
        rectTransform.localPosition = mousePos; //обновляем позицию объекта по движению мыши
    }

    // Метод для привязки к ближайшему пустому объекту после отпускания
    public void OnEndDrag(PointerEventData eventData)
    {
        Transform closestPoint = FindClosestGridPoint(out int freeIndex);
        if (closestPoint != null)
        {
            // Привязываем объект к позиции ближайшего пустого объекта
            rectTransform.position = closestPoint.position;
            iconManager.SetOccupiedGridPoints(freeIndex, true, gameObject.transform.parent, out Transform newParent);
            gameObject.transform.SetParent(newParent);
        }
        else
        {
            // Если нет пустых объектов (или ошибка), возвращаем объект в исходное положение
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    // Метод для нахождения ближайшего пустого объекта сетки
    private Transform FindClosestGridPoint(out int freeIndex)
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        int pointCounter = 0;
        freeIndex = -1;
        foreach (Transform point in iconManager.GetGridPoints())
        {
            float distance = Vector2.Distance(rectTransform.position, point.position);
            if (distance < closestDistance && !iconManager.GetOccupiedGridPoints(pointCounter))
            {
                closestDistance = distance;
                closestPoint = point;
                freeIndex = pointCounter;
            }
            pointCounter++;
        }

        return closestPoint;
    }
}
