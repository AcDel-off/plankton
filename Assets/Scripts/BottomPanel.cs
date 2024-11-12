using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BottomPanel : MonoBehaviour
{
    [SerializeField] int buttonsCount;
    [SerializeField] private List<GameObject> buttons; // ������ ������� ��������� ������ �� ������
    [SerializeField] private List<GameObject> activeWindows = new List<GameObject>(); // ������ �������� ����, ����������� � �������

    [SerializeField] private GameObject startMenu, restartWindow, restartLoading;
    [SerializeField] private float restartingDuration = 5;
    [SerializeField] private float restartingLoadingSpeed = 50;

    [SerializeField] private GameObject notebook, guide;

    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        for (int i = 1; i <= buttonsCount; i++)
        {
            buttons.Add(GameObject.Find($"Application{i}"));
            buttons[i - 1].SetActive(false);
        }

        OpenWindow(notebook);
        OpenWindow(guide);
    }

    // ����� ��� ���������� ������ ����
    public void OpenWindow(GameObject newWindow)
    {
        if (activeWindows.Count < buttons.Count) // ���������, ���� �� ��������� ������
        {
            // ���������� ��������� ������ � ��������� �� ����
            int nextIndex = activeWindows.Count;
            buttons[nextIndex].SetActive(true); // ���������� ������
            activeWindows.Add(newWindow); // ��������� ���� � ������ �������� ����

            // ����������� ����� RestoreWindow() ���� � ������
            Button button = buttons[nextIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => RestoreWindow(newWindow));

            buttons[nextIndex].GetComponentInChildren<Text>().text = newWindow.name;           //�������� ��������� ������
        }
        else
        {
            Debug.LogWarning("��� ��������� ������ �� ������!");
        }
    }

    // ����� ��� �������� ����
    public void CloseWindow(GameObject closingWindow)
    {
        int index = activeWindows.IndexOf(closingWindow);

        if (index != -1) // ���������, ���������� �� ���� � ������
        {
            activeWindows.RemoveAt(index); // ������� ���� �� ������

            // ������� ���������� ���� � ������ �����
            for (int i = index; i < activeWindows.Count; i++)
            {
                Button button = buttons[i].GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                GameObject window = activeWindows[i];
                button.onClick.AddListener(() => RestoreWindow(window));

                buttons[i].GetComponentInChildren<Text>().text = activeWindows[i].name;           //�������� ��������� ������
            }

            // ������������ ��������� ������
            buttons[activeWindows.Count].SetActive(false);
        }
    }

    // ����� ��� ������ RestoreWindow() � ������� WindowUI ��� ���������������� ����
    private void RestoreWindow(GameObject window)
    {
        WindowUI windowUI = window.GetComponent<WindowUI>();
        if (windowUI != null)
        {
            soundManager.WindowOpenSound();

            windowUI.RestoreWindow();
        }
        else
        {
            Debug.LogWarning("��������� WindowUI �� ������ �� �������: " + window.name);
        }
    }

    public void OpenStartMenu()
    {
        if (startMenu.activeSelf)
            startMenu.SetActive(false);
        else
            startMenu.SetActive(true);                                                            //mb add animation
    }

    public void RestartSystem(bool longRestart = false)
    {
        restartWindow.SetActive(true);
        startMenu.SetActive(false);
        foreach (GameObject window in activeWindows)
        {
            window.SetActive(false);
        }
        activeWindows.Clear();
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        if (longRestart)
            StartCoroutine(RestartingSystem(60f));
        else
            StartCoroutine(RestartingSystem(restartingDuration));
    }

    private IEnumerator RestartingSystem(float restartTime)
    {
        float timer = 0;
        while (timer <= restartTime)
        {
            timer += Time.deltaTime;

            restartLoading.transform.Rotate(Vector3.forward * restartingLoadingSpeed * Time.deltaTime);

            yield return null;
        }
        soundManager.SystemStartSound();
        TurnOffRestartingScreen();
    }

    public void TurnOffRestartingScreen() => restartWindow.SetActive(false);

    public int GetWindowsCount() => activeWindows.Count;

    public void ErrorSound() => soundManager.ErrorSound();
}
