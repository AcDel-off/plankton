using UnityEngine;

public class Screamer : MonoBehaviour
{
    [SerializeField] private float screamerTime = 0.3f;
    private float timer = 0f;
    [Header("Window Screamer")]
    [SerializeField] private bool windowScreamer = false;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject backgroundChange;

    [SerializeField] private GameObject ambientObject;

    void Update()
    {
        if (windowScreamer)
        {
            ambientObject.SetActive(false);                    //��������� �����, �������� ����
        }

        if (timer >= screamerTime)
        {
            if (windowScreamer) 
            {
                ambientObject.SetActive(true);                               //�������� ����� �������� ���� ������������
                backgroundChange.SetActive(true);
            }

            gameObject.SetActive(false);
        }
        timer += Time.deltaTime;
    }
}
