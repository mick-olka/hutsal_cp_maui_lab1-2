using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text.Json;
using System.Numerics;
using Lab2;

namespace Lab_2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //Lab2.DatabaseService database = new();
        private string _currentDateTime;
        private string _currentDeviceInfo;
        private string _dbStatus = "init";
        private ToDoItem _dbItem = new();
        private ToDoItem _currentToDo = new();
        
        public ToDoItem ToDoData
        {
            get => _currentToDo;
            set
            {
                _currentToDo = value;
                OnPropertyChanged();
            }
        }
        public ToDoItem DbItem
        {
            get => _dbItem;
            set
            {
                _dbItem = value;
                OnPropertyChanged();
            }
        }

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }
        public string DBStatus
        {
            get => _dbStatus;
            set
            {
                _dbStatus = value;
                OnPropertyChanged();
            }
        }
        public ICommand UpdateTimeCommand { get; }
        public ICommand FetchDataCommand { get; }
        public ICommand GetDataCommand { get; }
        public ICommand SaveDataCommand { get; }

        public string CurrentDeviceinfo
        {
            get => new StringBuilder()
            .AppendLine($"Device Model: {DeviceInfo.Model}")
            .AppendLine($"Device Manufacturer: {DeviceInfo.Manufacturer}")
            .AppendLine($"Device Platform: {DeviceInfo.Platform}")
            .AppendLine($"Device OS Version: {DeviceInfo.VersionString}").ToString();
            set
            {
                _currentDeviceInfo = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            UpdateTimeCommand = new Command(UpdateTime);
            CurrentDateTime = DateTime.Now.ToString("F");
            FetchDataCommand = new Command(FetchDataF);
            SaveDataCommand = new Command(SaveDataF);
            GetDataCommand = new Command(GetDataF);
        }
        private void UpdateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("F");
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FetchDataF ()
        {
            ToDoData = new ToDoItem
            {
                id = 1,
                title = "Loading..."
            };
            FetchDataFromApiAsync();
        }
        private void SaveDataF ()
        {
            DbItem = new ToDoItem
            {
                id = 1,
                title = "Saved"
            };
            SaveDataToDB();
        }        
        private void GetDataF ()
        {
            DbItem = new ToDoItem
            {
                id = 1,
                title = "Loading..."
            };
            GetDataFromDB();
        }

        public ToDoItem GetInitialToDo()
        {
            var todo = new ToDoItem
            {
                id = 1,
                title = "To Do",
                completed = false,
                userId = 1,
            };
            return todo;
        }

        private async Task FetchDataFromApiAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var toDoItem = JsonSerializer.Deserialize<ToDoItem>(json);
                ToDoData = toDoItem;
            }
        }

        private async Task SaveDataToDB ()
        {
            await DatabaseService.Init();
            await DatabaseService.SaveItemAsync(ToDoData);
            DBStatus = "saved";
        }

        private async Task GetDataFromDB ()
        {
            await DatabaseService.Init();
            var tdi = await DatabaseService.GetItemAsync(ToDoData.id);
            System.Diagnostics.Debug.WriteLine("===== Object from Database =====");
            System.Diagnostics.Debug.WriteLine("id: " + tdi.id);
            System.Diagnostics.Debug.WriteLine("title: " + tdi.title);
            System.Diagnostics.Debug.WriteLine("completed: " + tdi.completed);
            DBStatus = "received";
            DbItem = tdi;
        }
    }
}
