using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace IntoTheBrain.DataModel
{

    public class TaskItem
    {
        //Номер задачи
        public uint NumTask { get; set; }
        
        //Правельный ответ
        public String TrueAnswer { get; set; }
        
        //текст задачи
        public String TextTask { get; set; }

        // заполняет ComboBox возможными ответами 
        public String ComboBoxTask { get; set; }
    }

    public class TaskData
    {
        public String Title { get; set; }

        private List<TaskItem> _Items = new List<TaskItem>();
        public List<TaskItem> Items
        {
            get
            {
                return _Items;
            }
        }
    }

    public class TaskDataSource
    {
        private ObservableCollection<TaskData> _tasks = new ObservableCollection<TaskData>();
        public ObservableCollection<TaskData> Tasks
        {
            get
            {
                return _tasks;
            }
        }
        
        public async Task GetTasks()
        {
            var taskData = new TaskData {Title = "Одна единственная группа"};

            IList<String> tasks = await GetListTasks();
            foreach (var task in tasks)
            {
                taskData.Items.Add(await ParseTextTask(task));
            }

            Tasks.Add(taskData);
        }

        public async Task GetDemoTasks()
        {
            var taskData = new TaskData { Title = "Одна единственная группа" };

            IList<String> tasks = await GetListTasks();
            foreach (var task in tasks.Take(10))
            {
                taskData.Items.Add(await ParseTextTask(task));
            }

            Tasks.Add(taskData);
        }

        /// <summary>
        /// Разбирает строку-задачу из текстового файла на составляющее TaskItem
        /// </summary>
        /// <param name="textTask"></param>
        /// <returns></returns>
        private async Task<TaskItem> ParseTextTask(String textTask)
        {
            var task = new TaskItem();

            #region Отрезаем "Номер задачи"

            //Ищем начиная с начала, несколько символов и точка
            string pattern = @"^\d+\.";
            Match match = Regex.Match(textTask, pattern);
            if (match.Success)            //Если нашел соответствие
                task.NumTask = Convert.ToUInt32(match.Value.TrimEnd('.'));
            else
                throw new Exception("Не найден номер задачи");
            textTask = Regex.Replace(textTask, pattern, String.Empty); //Вырезаем сохраненный кусокa

            #endregion

            #region Отрезаем "Правильный ответ"

            //Шаблон = [any символы]
            pattern = @"\[.*\]";
            match = Regex.Match(textTask, pattern);
            if (match.Success)
                task.TrueAnswer = match.Value.Trim(' ', '[', ']');
            else
                throw new Exception("Не найден ответ");
            textTask = Regex.Replace(textTask, pattern, String.Empty);

            #endregion

            #region Отрезаем "ComboBox задачи"

            //task.CommentTask = String.Empty;
            pattern = @"\{.*\}";
            match = Regex.Match(textTask, pattern);
            if (match.Success)
                task.ComboBoxTask = match.Value.Trim(' ', '{', '}');
            textTask = Regex.Replace(textTask, pattern, String.Empty);

            #endregion

            #region Отрезаем "Тело задачи"

            task.TextTask = textTask.Trim();

            #endregion

            return task;
        }

        /// <summary>
        /// Читает задачи из текстового файла
        /// </summary>
        /// <returns></returns>
        private async Task<IList<string>> GetListTasks()
        {
            //StorageFile sampleFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("dataFile.txt");
            StorageFile sampleFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///DataModel/DataFiles/dataFile.txt"));
            if (sampleFile==null)
            {
                throw new Exception("Файл с задачами не найден");
            }
            IList<String> tasks = await FileIO.ReadLinesAsync(sampleFile, UnicodeEncoding.Utf8);
            return tasks;
        }
    }
    
}
