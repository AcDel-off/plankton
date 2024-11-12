using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DownloadingProcess : MonoBehaviour
{
    [SerializeField] private float loadingTime = 1f;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject[] newFiles;
    [SerializeField] private QuestManager questManager;

    public void StartAnimation(float loadingTime)
    {
        this.loadingTime = loadingTime;
        loadingSlider.value = 0f;
        StartCoroutine(DownloadingAnimation(loadingTime));
    }

    private IEnumerator DownloadingAnimation(float fileIndex)
    {
        float timer = 0f;
        while(timer <= loadingTime)
        {
            loadingSlider.value = Mathf.Lerp(0f, 1f, timer / loadingTime);
            timer += Time.deltaTime;
            yield return null;
        }

        switch (fileIndex)
        {
            case 1:
                newFiles[0].SetActive(true);
                questManager.DownloadFile(0);
                break;
            case 3:
                newFiles[2].SetActive(true);
                questManager.DownloadFile(2);
                break;
            case 4:
                newFiles[1].SetActive(true);
                questManager.DownloadFile(1);
                break;
            case 5:
                newFiles[3].SetActive(true);
                questManager.DownloadFile(3);
                break;
        }
        gameObject.GetComponent<WindowUI>().CloseWindow();
    }
}
