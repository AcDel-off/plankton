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
            ambientObject.SetActive(false);                    //бшйкчвхрэ юсдхн, лхцючыхи ябер
        }

        if (timer >= screamerTime)
        {
            if (windowScreamer) 
            {
                ambientObject.SetActive(true);                               //бйкчвхрэ юсдхн напюрмнА ябер бняярюмнбхрэ
                backgroundChange.SetActive(true);
            }

            gameObject.SetActive(false);
        }
        timer += Time.deltaTime;
    }
}
