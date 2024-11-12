using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notebook : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleText;  // ����� ��� �������� �������� ������
    [SerializeField] private List<TextMeshProUGUI> taskTexts; // ������ ��������� �������� ��� �������
    [SerializeField] private List<Toggle> taskToggles;        // ������ Toggle ��� �������

    [SerializeField] private QuestManager questManager;

    private int currentQuestIndex = 0; // ������� ����� ������
    private List<string> currentTasks; // ������ ������� ������� �������

    private Dictionary<int, QuestData> questDatabase = new Dictionary<int, QuestData>
    {
        { 0, new QuestData("������� �1", new List<string> { "������� �����", //OpenEmail
                                                            "������� ����� ���������", //OpenNewMessage 0
                                                            "������� ����", //DownloadFile 0
                                                            "������� ����� ��������", //OpenFile 4
                                                            "������� ����", //OpenFile 0
                                                            "�������� ����, ����� �� ��", //HighlightGroup 0
                                                            "����������� ������ ����", //CopyData 0
                                                            "������� ������ ���� �� ������� �����", //OpenFile 1
                                                            "�������� ������������� ���������� � ����� ���������" }) }, //PasteData 0
        { 1, new QuestData("������� �2", new List<string> { "������� �����", //OpenEmail
                                                            "����� � ����� �����", //EmailSpam
                                                            "������������� ������ ��������� �� �������, ������� �������� (0/5)", //DeleteMessage 1
                                                            "��������� �������", //OpenAndClearBin 0
                                                            "�������� �������" }) }, //OpenAndClearBin 1
        { 2, new QuestData("������� �3", new List<string> { "������� ����� � ���������� ������� �����", //OpenFile 2
                                                            "������� �����", //OpenEmail
                                                            "�� ����������� �������� ������ ������ ���������� � ����� �������� (0/3)",
                                                            "a.\t������� ����� ������", //CreateNewLetter
                                                            "b.\t������� �������� �� ������", //ChooseDestinationPerson (������ 3 �������)
                                                            "c.\t�������� �����, ����� �� ����", //HighlightGroup 1
                                                            "d.\t����������� ����� �� ����� ", //CopyData 1, 2, 3
                                                            "e.\t�������� ����� � ������", //PasteData 1
                                                            "f.\t��������� ������"}) }, //SendMessageChecker 1
        { 3, new QuestData("������� �4", new List<string> { "������� ���� � ��������� �� ������� �����", //OpenFile 3
                                                            "����� ������� \"������� �� ��������� �� �������\"", 
                                                            "�������� �������, ����� �� ��", //HighlightGroup 2
                                                            "������� ������", //MakeDiagram 1
                                                            "������� �����", //OpenEmail
                                                            "������� ����� ������", //CreateNewLetter
                                                            "������� �������� \"andrue\" �� ������", //ChooseDestinationPerson (1-�� �������)
                                                            "�������� ������, ����� �� ����", //HighlightGroup 3
                                                            "����������� ������", //CopyData 4
                                                            "�������� ������ � ������", //PasteData 1
                                                            "��������� ������"}) }, //SendMessageChecker 1
        { 4, new QuestData("������� �5", new List<string> { "������� �����", //OpenEmail
                                                            "������� ����� ���������", //OpenNewMessage 1
                                                            "������� ����� �� ����������� (0/3)", //DownloadFile 1, 2, 3
                                                            "������� ����� ��������", //OpenFile 4
                                                            "�������� ��� �����, ����� �� ���", //HighlightGroup 4
                                                            "������������ ����� "}) } //ArchiveData 1
    };

    private int deletedSpamMessagesCounter = 0;
    private int sendMessagesCounter = 0;
    private int downloadFilesCounter = 0;

    private void Start()
    {
        LoadQuest(currentQuestIndex);
    }

    // �������� ������ �� ������
    private void LoadQuest(int questIndex)
    {
        if (!questDatabase.ContainsKey(questIndex))
        {
            Debug.LogWarning("����� �� ������ � ���� ������.");
            return;
        }

        QuestData questData = questDatabase[questIndex];
        questTitleText.text = questData.questTitle;
        currentTasks = questData.tasks;

        // ��������� ��������� ������� �������
        for (int i = 0; i < taskTexts.Count; i++)
        {
            if (i < currentTasks.Count)
            {
                taskTexts[i].text = currentTasks[i];
                taskTexts[i].fontStyle = FontStyles.Normal; // ���������� ����� ������
                taskToggles[i].isOn = false; // ���������� ��������� Toggle

                taskTexts[i].gameObject.SetActive(true);
                taskToggles[i].gameObject.SetActive(true); // ���������� Toggle � �����
            }
            else
            {
                taskTexts[i].gameObject.SetActive(false); // ��������� ������ ��������
                taskToggles[i].gameObject.SetActive(false);
            }
        }

        questManager.StartQuest(questIndex);
    }

    // �������� ���������� �������
    public void CompleteTask(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= currentTasks.Count)
        {
            Debug.LogWarning("�������� ������ �������.");
            return;
        }

        // ��������� ��������� �������
        taskTexts[taskIndex].fontStyle = FontStyles.Strikethrough; // ����������� �����
        taskToggles[taskIndex].isOn = true; // �������� �������� Toggle

        // ���������, ��������� �� ��� ������� ������
        if (AllTasksCompleted())
        {
            CompleteQuest();
        }
    }

    public void UpdateTask(int taskIndex)
    {
        switch(taskIndex)
        {
            case 0:
                taskTexts[2].text = $"������������� ������ ��������� �� �������, ������� �������� ({++deletedSpamMessagesCounter}/5)";
                if (deletedSpamMessagesCounter == 5)
                {
                    CompleteTask(2);
                }
                break;
            case 1:
                taskTexts[2].text = $"�� ����������� �������� ������ ������ ���������� � ����� �������� ({++sendMessagesCounter}/3)";
                for (int i = 3; i <= 8; i++)
                {
                    taskTexts[i].fontStyle = FontStyles.Normal;
                    taskToggles[i].isOn = false;
                }
                if (sendMessagesCounter == 3)
                {
                    CompleteQuest();
                }
                break;
            case 2:
                taskTexts[2].text = $"������� ����� �� ����������� ({++downloadFilesCounter}/3)";
                if (downloadFilesCounter == 3)
                {
                    CompleteTask(2);
                }
                break;

            case 3:
                taskTexts[2].text = $"������������� ������ ��������� �� �������, ������� �������� ({--deletedSpamMessagesCounter}/5)";
                if (deletedSpamMessagesCounter != 5)
                {
                    taskTexts[2].fontStyle = FontStyles.Normal;
                    taskToggles[2].isOn = false;
                }
                break;

            case 4:
                taskTexts[4].text = "������������� ������������ �������";
                break;
            case 5:
                taskTexts[4].text = "�������� �������";
                break;

            case 6:
                taskTexts[8].text = "f.\t������������� ������������ �������";
                break;
            case 7:
                taskTexts[8].text = "f.\t��������� ������";
                break;
        }
    }

    // �������� ���������� ���� ������� ������
    private bool AllTasksCompleted()
    {
        foreach (var toggle in taskToggles)
        {
            if (toggle.gameObject.activeSelf && !toggle.isOn) // ��������� ������ �������� Toggle
            {
                return false;
            }
        }
        return true;
    }

    // ���������� �������� ������ � ������� � ����������
    private void CompleteQuest()
    {
        currentQuestIndex++;
        if (questDatabase.ContainsKey(currentQuestIndex))
        {
            LoadQuest(currentQuestIndex); // ��������� ��������� �����
        }
        else
        {
            Debug.Log("��� ������ ���������!");
        }
    }

    public void TestComplete() => CompleteQuest();                                                          //�������
}

// ����� ��� �������� ������ � ������
[System.Serializable]
public class QuestData
{
    public string questTitle;
    public List<string> tasks;

    public QuestData(string title, List<string> taskList)
    {
        questTitle = title;
        tasks = taskList;
    }
}
