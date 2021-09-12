using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskSchedule_Cont1.Models;
using TaskSchedule_Cont1.Services;

namespace TaskSchedule_Cont1.ViewModel
{
    class TaskViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private bool isUpdate = false;

        private string isAddTask = "Collapsed";
        public string IsAddTask
        {
            get { return isAddTask; }
            set
            {
                isAddTask = value;
                NotifyPropertyChanged("IsAddTask");
            }
        }

        private string taskName;
        public string TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;
                NotifyPropertyChanged("TaskName");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }
        private DateTime dueDate = DateTime.Now;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                NotifyPropertyChanged("DueDate");
            }
        }

        private List<TaskModel> taskList;
        public List<TaskModel> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                NotifyPropertyChanged("TaskList");

            }
        }

        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                NotifyPropertyChanged("SelectedTask");
            }
        }
        public ICommand cmdAddaTask { get; private set; }
        public ICommand cmdAddTask { get; private set; }
        public ICommand cmdCancelTask { get; private set; }
        public ICommand cmdUpdateItem { get; private set; }
        public ICommand cmdDeleteItem { get; private set; }

        public bool CanAddaTask
        {
            get { return IsAddTask == "Collapsed"; }
        }
        public bool CanExectute_Update
        {
            get { return IsAddTask == "Collapsed"; }
        }
        public bool CanDelete
        {
            get { return SelectedTask != null; }
        }
        public bool CanExectute_AddTask
        {
            get { return !string.IsNullOrEmpty(TaskName); }
        }
        public TaskViewModel()
        {
            cmdCancelTask = new RelayCommand(CancelAddingTask, () => true);
            cmdAddaTask = new RelayCommand(AddaTask, () => CanAddaTask);
            cmdAddTask = new RelayCommand(AddTask, () => CanExectute_AddTask);
            cmdDeleteItem = new RelayCommand(Delete, () => CanDelete);
            cmdUpdateItem = new RelayCommand(Update, () => CanExectute_Update);
            getTask();
        }

        private void AddaTask()
        {
            IsAddTask = "Visible";
            isUpdate = false;
            TaskName = string.Empty;
            Description = string.Empty;
        }
        private async void AddTask()
        {
            if (!isUpdate)
            {
                var r = await App.TaskDatabase.SaveTaskAsync(new TaskModel
                {
                    TaskName = TaskName,
                    Description = Description,
                    DueDate = DueDate
                });
            }
            else
            {

                SelectedTask.TaskName = TaskName;
                SelectedTask.Description = Description;
                SelectedTask.DueDate = DueDate;
                var r = await App.TaskDatabase.UpdateTask(SelectedTask);
                isUpdate = false;
            }
            IsAddTask = "Collapsed";

            getTask();

        }
        public async void getTask()
        {
            TaskList = await App.TaskDatabase.GetTaskAsync();
        }
        private async void Delete()
        {
            await App.TaskDatabase.DeleteTask(SelectedTask);
            getTask();
        }
        private void CancelAddingTask()
        {
            IsAddTask = "Collapsed";
            isUpdate = false;
        }
        private void Update()
        {
            IsAddTask = "Visible";
            isUpdate = true;

            TaskName = selectedTask.TaskName;
            Description = selectedTask.Description;
            DueDate = selectedTask.DueDate;

        }
    }
}
