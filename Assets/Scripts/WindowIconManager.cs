using System;
using Unity.Collections;
using UnityEngine;

public class WindowIconManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private RectTransform[] gridPoints = new RectTransform[6 * 9]; // Список пустых объектов, к которым будет привязка
    private bool[] occupiedGridPoints = new bool[6 * 9];

    void Start()
    {
        int tCounter = 0;
        foreach (RectTransform t in gameObject.GetComponentsInChildren<RectTransform>())
        {
            if (t.gameObject.name == "GridCell")
            {
                gridPoints[tCounter] = t;

                if (t.childCount == 1)
                {
                    occupiedGridPoints[tCounter] = true;
                }
                tCounter++;
            }
        }
    }

    public RectTransform[] GetGridPoints()
    {
        return gridPoints;
    }

    public bool GetOccupiedGridPoints(int i)
    {
        return occupiedGridPoints[i];
    }

    public void SetOccupiedGridPoints(int i, bool value, Transform lastParent, out Transform newParent)
    {
        occupiedGridPoints[Array.IndexOf(gridPoints, lastParent)] = !value;
        occupiedGridPoints[i] = value;
        newParent = gridPoints[i];
    }
}
