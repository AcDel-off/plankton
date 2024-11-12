using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject planeColor;
    [SerializeField] private GameObject textColor;
    public void StartCutScene()
    {
        StartCoroutine(FinalCutScene());
    }

    private IEnumerator FinalCutScene()
    {
        float timer = 0;
        while (timer < 3)
        {
            timer += Time.deltaTime;

            Image image = planeColor.GetComponent<Image>();
            Color color = image.color;
            color.a = Mathf.Lerp(0, 1, timer / 3);
            image.color = color;

            yield return null;
        }
        yield return new WaitForSeconds(2);
        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;

            TextMeshProUGUI image = textColor.GetComponent<TextMeshProUGUI>();
            Color color = image.color;
            color.a = Mathf.Lerp(0, 1, timer / 3);
            image.color = color;

            yield return null;
        }
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
