using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Notebook notebook;

    [Header("Quest Objects")]
    [SerializeField] private GameObject emailWindow, newDatabasePiece, letterWindow, letterUnknownWindow,
        newMessage, graphIcon, downloadHighlight, archiveFile, errorWindow, bottomPanel, graphPhoto;
    [Header("Horror Objects")]
    [SerializeField] private GameObject errorCleaner, ocText, screamerScreenSmile, secretNewMessage, screamerBackPerson, 
        screamerWindow, addWindow, downloadsWindow, sighSoundScreamer, soundManager;
    [SerializeField] private GameObject[] emailWindows = new GameObject[3];

    [SerializeField] private GameObject[] finalStageMovingWindows;

    private bool databaseCopied = false;
    private int lettersCreated = 0;
    private int destinationPerson = 0;
    private int textCopied = 0;
    private string[] letterTexts =
    {
        "Здравствуйте!\r\n\r\nНе могли бы вы подготовить краткий отчёт по текущему статусу проекта \"Selfcare\"? Интересуют ключевые этапы, сроки выполнения и возможные риски. Желательно получить отчёт до конца недели.\r\n\r\nСпасибо!",
        "Добрый день\r\nНужно проверить всю клиентскую документацию на актуальность перед передачей её заказчику. Обратите внимание на соответствие данных текущим стандартам. Подскажите, когда сможете это сделать.",
        "Привет!\r\n\r\nПоступила информация, что в базе данных есть устаревшие данные по некоторым клиентам. Пожалуйста, обнови записи по мере возможности и дай знать, когда всё будет готово.\r\n\r\nБуду ждать!"
    };
    private bool[] letterSent = { false, false, false };
    private bool[] filesDownloaded = { false, false, false };

    private int currentQuestIndex = 0;

    private bool errorClosing = false, errorSending = false, screamerWindowFace = false, closeDownloadsWindow = false, addWindowOpened = false;

    public void StartQuest(int questIndex)
    {
        currentQuestIndex = questIndex;

        switch (currentQuestIndex)
        {
            case 0:
                Quest_1_Started();
                break;
            case 1:
                Quest_2_Started();
                break;
            case 2:
                Quest_3_Started();
                break;
            case 3:
                Quest_4_Started();
                break;
            case 4:
                Quest_5_Started();
                break;
        }
    }

    private void Quest_1_Started()
    {
        if (!emailWindow.GetComponent<WindowUI>().GetClosed())
            OpenEmail();
    }

    private void Quest_2_Started()
    {
        if (!emailWindow.GetComponent<WindowUI>().GetClosed())
            OpenEmail();
    }

    private void Quest_3_Started()
    {
        if (!emailWindow.GetComponent<WindowUI>().GetClosed())
            OpenEmail();
    }

    private void Quest_4_Started()
    {
        if (!emailWindow.GetComponent<WindowUI>().GetClosed())
            OpenEmail();

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                emailWindows[i].SetActive(true);
            }
            else
            {
                emailWindows[i].SetActive(false);
            }
        }

        secretNewMessage.SetActive(true);
    }

    private void Quest_5_Started()
    {
        if (!emailWindow.GetComponent<WindowUI>().GetClosed())
            OpenEmail();
    }

    //Email
    public void OpenEmail()
    {
        switch(currentQuestIndex)
        {
            case 0:
                notebook.CompleteTask(0);
                break;
            case 1:
                notebook.CompleteTask(0);
                break;
            case 2:
                notebook.CompleteTask(1);
                break;
            case 3:
                notebook.CompleteTask(4);
                break;
            case 4:
                notebook.CompleteTask(0);
                break;
        }
    }
    public void EmailSpam()
    {
        if (currentQuestIndex == 1)
        {
            notebook.CompleteTask(1);
        }
    }

    public void DeleteMessage(int spamIndex)
    {
        if (currentQuestIndex == 1)
        {
            if (spamIndex == 1)
            {
                notebook.UpdateTask(0);
            }
        }
    }

    public void RestoreMessage(int spamIndex)
    {
        if (currentQuestIndex == 1)
        {
            if (spamIndex == 1)
            {
                notebook.UpdateTask(3);
            }
        }
    }

    public void OpenAndClearBin(int taskIndex)
    {
        if (currentQuestIndex == 1)
        {
            switch (taskIndex)
            {
                case 0:
                    notebook.CompleteTask(3);
                    break;
                case 1:
                    notebook.CompleteTask(4);
                    break;
            }
        }
    }

    public void OpenNewMessage(int messageIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (messageIndex == 0)
                    notebook.CompleteTask(1);
                break;
            case 4:
                if (messageIndex == 1)
                    notebook.CompleteTask(1);
                break;
        }
    }

    public void SendMessageChecker(int senderIndex)
    {
        switch (currentQuestIndex)
        {
            case 2:
                if (senderIndex == 1)
                    if (RightMessage())
                    {
                        if (lettersCreated == 3 && !errorSending)
                        {
                            errorWindow.GetComponent<WindowUI>().OpenWindow();
                            notebook.UpdateTask(6);
                            letterSent[destinationPerson] = false;
                        }
                        else
                        {
                            letterWindow.SetActive(false);
                            notebook.UpdateTask(1);
                        }
                    }
                    else
                    {
                        errorWindow.GetComponent<WindowUI>().OpenWindow();
                    }
                break;
            case 3:
                if (senderIndex == 1)
                {
                    if (textCopied == 4 && destinationPerson == 0)
                    {
                        letterWindow.SetActive(false);
                        notebook.CompleteTask(10);
                        newMessage.SetActive(true);
                    }
                    else
                    {
                        errorWindow.GetComponent<WindowUI>().OpenWindow();
                    }
                }
                break;
        }
    }

    private bool RightMessage()
    {
        if (destinationPerson == 1 && textCopied == 1 ||
            destinationPerson == 0 && textCopied == 2 ||
            destinationPerson == 2 && textCopied == 3)
        {
            if (letterSent[destinationPerson])
            {
                return false;
            }
            Debug.Log($"im here: letterSent {letterSent[0]} {letterSent[1]} {letterSent[2]}");
            letterSent[destinationPerson] = true;
            Debug.Log($"im there: letterSent {letterSent[0]} {letterSent[1]} {letterSent[2]}");
            return true;
        }
        else return false;
    }

    public void CreateNewLetter()
    {
        switch (currentQuestIndex)
        {
            case 2:
                Debug.Log("lettersCreated " + lettersCreated);
                switch (lettersCreated)
                {
                    case 0:
                        letterWindow.SetActive(true);
                        lettersCreated++;
                        break;
                    case 1:
                        letterWindow.SetActive(true);
                        lettersCreated++;
                        break;
                    case 2:
                        letterWindow.SetActive(true);
                        lettersCreated++;
                        break;
                    default:
                        letterUnknownWindow.SetActive(true);
                        break;
                }

                notebook.CompleteTask(3);
                break;
            case 3:
                Debug.Log("lettersCreated " + lettersCreated);
                switch (lettersCreated)
                {
                    case 3:
                        letterWindow.SetActive(true);
                        lettersCreated++;
                        break;
                    default:
                        letterUnknownWindow.SetActive(true);
                        break;
                }

                notebook.CompleteTask(5);
                break;
            default:
                letterUnknownWindow.SetActive(true);
                break;
        }
    }

    public void ChooseDestinationPerson(int destinationPersonIndex)
    {
        destinationPerson = destinationPersonIndex;

        if (currentQuestIndex == 2)
            notebook.CompleteTask(4);
        if (currentQuestIndex == 3)
        {
            if (destinationPersonIndex == 0)
                notebook.CompleteTask(6);
        }
    }

    //Files
    public void DownloadFile(int fileIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (fileIndex == 0)
                    notebook.CompleteTask(2);
                break;
            case 4:
                if (fileIndex == 1 && !filesDownloaded[0])
                {
                    notebook.UpdateTask(2);
                    filesDownloaded[0] = true;
                }
                else if (fileIndex == 2 && !filesDownloaded[1])
                {
                    notebook.UpdateTask(2);
                    filesDownloaded[1] = true;
                }
                else if (fileIndex == 3 && !filesDownloaded[2])
                {
                    notebook.UpdateTask(2);
                    filesDownloaded[2] = true;
                }
                if (filesDownloaded[0] && filesDownloaded[1] && filesDownloaded[2])
                    downloadHighlight.SetActive(true);
                break;
        }
    }

    public void OpenFile(int fileIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (fileIndex == 0)
                    notebook.CompleteTask(4);
                if (fileIndex == 1)
                    notebook.CompleteTask(7);
                if (fileIndex == 4)
                    notebook.CompleteTask(3);
                break;
            case 2:
                if (fileIndex == 2)
                    notebook.CompleteTask(0);
                break;
            case 3:
                if (fileIndex == 3)
                    notebook.CompleteTask(0);
                break;
            case 4:
                if (fileIndex == 4)
                {
                    if (!addWindowOpened)
                    {
                        addWindow.GetComponent<WindowUI>().OpenWindow();
                        addWindowOpened = true;
                    }
                    notebook.CompleteTask(3);
                }
                break;
        }
    }

    //Data
    public void HighlightGroup(int highlightIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (highlightIndex == 0)
                    notebook.CompleteTask(5);
                break;
            case 2:
                if (highlightIndex == 1)
                    notebook.CompleteTask(5);
                break;
            case 3:
                if (highlightIndex == 2)
                {
                    notebook.CompleteTask(1);
                    notebook.CompleteTask(2);
                }
                else if (highlightIndex == 3)
                    notebook.CompleteTask(7);
                break;
            case 4:
                if (highlightIndex == 4)
                {
                    if (!closeDownloadsWindow)
                    {
                        downloadsWindow.GetComponent<WindowUI>().CloseWindow();
                        FinalBugsStage();
                        closeDownloadsWindow = true;
                    }
                    notebook.CompleteTask(4);
                }
                break;
        }
    }

    public void CopyData(int dataIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (dataIndex == 0)
                {
                    databaseCopied = true;
                    notebook.CompleteTask(6);
                }
                break;
            case 2:
                if (dataIndex == 1 || dataIndex == 2 || dataIndex == 3)
                {
                    textCopied = dataIndex;
                    notebook.CompleteTask(6);
                }
                break;
            case 3:
                if (dataIndex == 4)
                {
                    if (!screamerWindowFace)
                    {
                        screamerWindow.SetActive(true);
                        screamerWindowFace = true;
                    }
                    textCopied = dataIndex;
                    notebook.CompleteTask(8);
                }
                break;
        }
    }

    public void PasteData(int dataIndex)
    {
        switch (currentQuestIndex)
        {
            case 0:
                if (dataIndex == 0)
                {
                    if (databaseCopied)
                    {
                        notebook.CompleteTask(8);
                        newDatabasePiece.SetActive(true);
                    }
                }
                break;
            case 2:
                if (dataIndex == 1)
                {
                    if (textCopied == 2 && !errorClosing)
                    {
                        emailWindow.GetComponent<WindowUI>().CloseWindow();
                        errorWindow.GetComponent<WindowUI>().OpenWindow();
                        errorClosing = true;
                    }
                    if (textCopied > 0)
                    {
                        letterWindow.GetComponentInChildren<TextMeshProUGUI>().text = letterTexts[textCopied - 1];
                        notebook.CompleteTask(7);
                    }
                }
                break;
            case 3:
                if (dataIndex == 1)
                {
                    if (textCopied == 4)
                    {
                        notebook.CompleteTask(9);
                        graphPhoto.SetActive(true);
                    }
                }
                break;
        }
    }

    public void ArchiveData(int archiveIndex)
    {
        if (currentQuestIndex == 4)
        {
            if (archiveIndex == 1)
            {
                soundManager.GetComponent<SoundManager>().HeartSound();
                                            //АНИМАЦИЯ МИГАЮЩЕГО СВЕТА, ЗВУК УЧАЩЁННОГО СЕРДЦЕБИЕНИЯ, ПОСЛЕ СВЕТ ГАСНЕТ ПОЛНОСТЬЮ
                Invoke(nameof(ToFinalStage), 10f); //ИЗМЕНИТЬ ВРЕМЯ НА ВРЕМЯ АНИМАЦИИ СВЕТА
                archiveFile.SetActive(true);
                notebook.CompleteTask(5);
            }
        }
    }

    public void MakeDiagram(int diagramType)
    {
        if (currentQuestIndex == 3)
        {
            if(diagramType == 1)
            {
                graphIcon.SetActive(true);
                notebook.CompleteTask(3);
            }
        }
    }

    //Horror & bugs
    public void CleaningBinError()
    {
        if (currentQuestIndex == 1)
        {
            notebook.UpdateTask(4);
        }
    }

    public void RestartSystem()
    {
        switch (currentQuestIndex)
        {
            case 1:
                errorCleaner.SetActive(false);
                notebook.UpdateTask(5);
                bottomPanel.GetComponent<BottomPanel>().RestartSystem();
                break;
            case 2:
                errorSending = true;
                ChangeOCText("обернись");
                notebook.UpdateTask(7);
                bottomPanel.GetComponent<BottomPanel>().RestartSystem(true);
                GameObject.Find("Main Camera").GetComponent<CameraChange>().StartTurnChecking();
                break;
            default:
                bottomPanel.GetComponent<BottomPanel>().RestartSystem();
                break;
        }
    }

    public void ChangeOCText(string newText) => ocText.GetComponent<TextMeshProUGUI>().text = newText;

    public void ScreamerSmile() => screamerScreenSmile.SetActive(true);
    public void ScreamerBackPerson(bool toState) => screamerBackPerson.SetActive(toState);

    private void FinalBugsStage()
    {
        StartCoroutine(OpenAndFlyingWindows());
    }

    private IEnumerator OpenAndFlyingWindows()
    {
        while (true)
        {
            int randomWindow = Random.Range(0, finalStageMovingWindows.Length);
            finalStageMovingWindows[randomWindow].GetComponent<WindowUI>().OpenWindow();
            if (Random.Range(0, 10) >= 6)
            {
                finalStageMovingWindows[randomWindow].GetComponent<WindowUI>().ErrorMove(Random.Range(2f, 10f));

                if (!sighSoundScreamer.activeSelf)
                    sighSoundScreamer.SetActive(true);
            }
            yield return new WaitForSeconds(Random.Range(1f, 4f));
        }
    }

    private void ToFinalStage()
    {
        SceneManager.LoadScene(1);
    }
}
