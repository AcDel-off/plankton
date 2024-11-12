using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class SnapToGrid : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition; //�������� ������� ������� ����� ���������������
    private WindowIconManager iconManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        iconManager = GameObject.Find("GridContainer").GetComponent<WindowIconManager>();
    }

    //����� ��� �������������� �������
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out mousePos);
        rectTransform.localPosition = mousePos; //��������� ������� ������� �� �������� ����
    }

    // ����� ��� �������� � ���������� ������� ������� ����� ����������
    public void OnEndDrag(PointerEventData eventData)
    {
        Transform closestPoint = FindClosestGridPoint(out int freeIndex);
        if (closestPoint != null)
        {
            // ����������� ������ � ������� ���������� ������� �������
            rectTransform.position = closestPoint.position;
            iconManager.SetOccupiedGridPoints(freeIndex, true, gameObject.transform.parent, out Transform newParent);
            gameObject.transform.SetParent(newParent);
        }
        else
        {
            // ���� ��� ������ �������� (��� ������), ���������� ������ � �������� ���������
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    // ����� ��� ���������� ���������� ������� ������� �����
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
