using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notebook : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleText;  // Текст для названия текущего квеста
    [SerializeField] private List<TextMeshProUGUI> taskTexts; // Список текстовых объектов для заданий
    [SerializeField] private List<Toggle> taskToggles;        // Список Toggle для заданий

    [SerializeField] private QuestManager questManager;

    private int currentQuestIndex = 0; // Текущий номер квеста
    private List<string> currentTasks; // Список текстов текущих заданий

    private Dictionary<int, QuestData> questDatabase = new Dictionary<int, QuestData>
    {
        { 0, new QuestData("Задание №1", new List<string> { "Открыть почту", //OpenEmail
                                                            "Открыть новое сообщение", //OpenNewMessage 0
                                                            "Скачать файл", //DownloadFile 0
                                                            "Открыть папку загрузок", //OpenFile 4
                                                            "Открыть файл", //OpenFile 0
                                                            "Выделить базу, нажав на неё", //HighlightGroup 0
                                                            "Скопировать данные базы", //CopyData 0
                                                            "Открыть старую базу на рабочем столе", //OpenFile 1
                                                            "Вставить скопированную информацию в конец документа" }) }, //PasteData 0
        { 1, new QuestData("Задание №2", new List<string> { "Открыть почту", //OpenEmail
                                                            "Зайти в отдел спама", //EmailSpam
                                                            "Отсортировать важные сообщения от рекламы, удалить ненужное (0/5)", //DeleteMessage 1
                                                            "Проверить корзину", //OpenAndClearBin 0
                                                            "Очистить корзину" }) }, //OpenAndClearBin 1
        { 2, new QuestData("Задание №3", new List<string> { "Открыть папку с текстовыми файлами писем", //OpenFile 2
                                                            "Открыть почту", //OpenEmail
                                                            "По отдельности прислать каждое письмо указанному в файле человеку (0/3)",
                                                            "a.\tСоздать новое письмо", //CreateNewLetter
                                                            "b.\tВыбрать адресата из списка", //ChooseDestinationPerson (список 3 человек)
                                                            "c.\tВыделить текст, нажав на него", //HighlightGroup 1
                                                            "d.\tСкопировать текст из файла ", //CopyData 1, 2, 3
                                                            "e.\tВставить текст в письмо", //PasteData 1
                                                            "f.\tОтправить письмо"}) }, //SendMessageChecker 1
        { 3, new QuestData("Задание №4", new List<string> { "Открыть файл с таблицами на рабочем столе", //OpenFile 3
                                                            "Найти таблицу \"Расходы на маркетинг по каналам\"", 
                                                            "Выделить таблицу, нажав на неё", //HighlightGroup 2
                                                            "Создать график", //MakeDiagram 1
                                                            "Открыть почту", //OpenEmail
                                                            "Создать новое письмо", //CreateNewLetter
                                                            "Выбрать адресата \"andrue\" из списка", //ChooseDestinationPerson (1-ин человек)
                                                            "Выделить график, нажав на него", //HighlightGroup 3
                                                            "Скопировать график", //CopyData 4
                                                            "Вставить график в письмо", //PasteData 1
                                                            "Отправить письмо"}) }, //SendMessageChecker 1
        { 4, new QuestData("Задание №5", new List<string> { "Открыть почту", //OpenEmail
                                                            "Открыть новое сообщение", //OpenNewMessage 1
                                                            "Скачать файлы по отдельности (0/3)", //DownloadFile 1, 2, 3
                                                            "Открыть папку загрузок", //OpenFile 4
                                                            "Выделить все файлы, нажав на них", //HighlightGroup 4
                                                            "Архивировать файлы "}) } //ArchiveData 1
    };

    private int deletedSpamMessagesCounter = 0;
    private int sendMessagesCounter = 0;
    private int downloadFilesCounter = 0;

    private void Start()
    {
        LoadQuest(currentQuestIndex);
    }

    // Загрузка квеста по номеру
    private void LoadQuest(int questIndex)
    {
        if (!questDatabase.ContainsKey(questIndex))
        {
            Debug.LogWarning("Квест не найден в базе данных.");
            return;
        }

        QuestData questData = questDatabase[questIndex];
        questTitleText.text = questData.questTitle;
        currentTasks = questData.tasks;

        // Обновляем текстовые объекты заданий
        for (int i = 0; i < taskTexts.Count; i++)
        {
            if (i < currentTasks.Count)
            {
                taskTexts[i].text = currentTasks[i];
                taskTexts[i].fontStyle = FontStyles.Normal; // Сбрасываем стиль текста
                taskToggles[i].isOn = false; // Сбрасываем состояние Toggle

                taskTexts[i].gameObject.SetActive(true);
                taskToggles[i].gameObject.SetActive(true); // Активируем Toggle и текст
            }
            else
            {
                taskTexts[i].gameObject.SetActive(false); // Отключаем лишние элементы
                taskToggles[i].gameObject.SetActive(false);
            }
        }

        questManager.StartQuest(questIndex);
    }

    // Проверка выполнения задания
    public void CompleteTask(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= currentTasks.Count)
        {
            Debug.LogWarning("Неверный индекс задания.");
            return;
        }

        // Обновляем состояние задания
        taskTexts[taskIndex].fontStyle = FontStyles.Strikethrough; // Вычёркиваем текст
        taskToggles[taskIndex].isOn = true; // Отмечаем галочкой Toggle

        // Проверяем, выполнены ли все задания квеста
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
                taskTexts[2].text = $"Отсортировать важные сообщения от рекламы, удалить ненужное ({++deletedSpamMessagesCounter}/5)";
                if (deletedSpamMessagesCounter == 5)
                {
                    CompleteTask(2);
                }
                break;
            case 1:
                taskTexts[2].text = $"По отдельности прислать каждое письмо указанному в файле человеку ({++sendMessagesCounter}/3)";
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
                taskTexts[2].text = $"Скачать файлы по отдельности ({++downloadFilesCounter}/3)";
                if (downloadFilesCounter == 3)
                {
                    CompleteTask(2);
                }
                break;

            case 3:
                taskTexts[2].text = $"Отсортировать важные сообщения от рекламы, удалить ненужное ({--deletedSpamMessagesCounter}/5)";
                if (deletedSpamMessagesCounter != 5)
                {
                    taskTexts[2].fontStyle = FontStyles.Normal;
                    taskToggles[2].isOn = false;
                }
                break;

            case 4:
                taskTexts[4].text = "Перезагрузить операционную систему";
                break;
            case 5:
                taskTexts[4].text = "Очистить корзину";
                break;

            case 6:
                taskTexts[8].text = "f.\tПерезагрузить операционную систему";
                break;
            case 7:
                taskTexts[8].text = "f.\tОтправить письмо";
                break;
        }
    }

    // Проверка выполнения всех заданий квеста
    private bool AllTasksCompleted()
    {
        foreach (var toggle in taskToggles)
        {
            if (toggle.gameObject.activeSelf && !toggle.isOn) // Проверяем только активные Toggle
            {
                return false;
            }
        }
        return true;
    }

    // Завершение текущего квеста и переход к следующему
    private void CompleteQuest()
    {
        currentQuestIndex++;
        if (questDatabase.ContainsKey(currentQuestIndex))
        {
            LoadQuest(currentQuestIndex); // Загружаем следующий квест
        }
        else
        {
            Debug.Log("Все квесты завершены!");
        }
    }

    public void TestComplete() => CompleteQuest();                                                          //УДАЛИТЬ
}

// Класс для хранения данных о квесте
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
