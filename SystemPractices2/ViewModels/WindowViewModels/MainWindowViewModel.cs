#nullable disable

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using SystemPractices2.Commands;
using SystemPractices2.Models;
using SystemPractices2.Services;
using SystemPractices2.Views.Windows;

namespace SystemPractices2.ViewModels.WindowViewModels;

public class MainWindowViewModel : NotificationService
{
    private object lockObject = new();
    private Comment selectedcomment;
    public Comment SelectedComment { get => this.selectedcomment; set { this.selectedcomment = value; OnPropertyChanged(); } }
    public string jsonFilePath = "comments.json";

    private string url;
    public string URL { get => this.url; set { this.url = value; OnPropertyChanged(); } }

    private ObservableCollection<Comment> comments;
    public ObservableCollection<Comment> Comments { get => this.comments; set { this.comments = value; OnPropertyChanged(); } }

    public ICommand? UploadCommand { get; set; }
    public ICommand? RemoveCommand { get; set; }
    public ICommand? AddWindowCommand { get; set; }
    public ICommand? EditWindowCommand { get; set; }

    public MainWindowViewModel() 
    {
        comments = new();
        this.URL = "https://jsonplaceholder.typicode.com/comments";


        UploadCommand = new RelayCommand(UpLoadFunction);
        RemoveCommand = new RelayCommand(Remove);
        AddWindowCommand = new RelayCommand(SwitchAddWindow);
        EditWindowCommand = new RelayCommand(SwitchEditWindow);
    }

    public async void UpLoadFunction(object? parameter)
    {
        this.Comments = new();
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            this.Comments = JsonSerializer.Deserialize<ObservableCollection<Comment>>(jsonData);
        }
        else
        {
            var result = await Deserializer1();
            this.Comments = result;

            var data = await GetAllDataFromJsonApi();
            File.WriteAllText(jsonFilePath, data.ToString());
        }
    }


    public async void Remove(object? parameter)
    {
        if (SelectedComment != null)
        {
            foreach (var c in Comments)
            {
                if (c.id == SelectedComment.id && c.postId == SelectedComment.postId)
                {
                    await Task.Delay(2500);
                    lock (lockObject)
                    {
                        //await Task.Delay(2500);
                        Comments.Remove(c);
                        string jsonData = JsonSerializer.Serialize(Comments);
                        File.WriteAllText(jsonFilePath, jsonData);
                        break;
                    }
                }
            }
        }
    }

    public void SwitchAddWindow(object? parameter)
    {
        SwitchingWindow<AddWindow>(parameter);
    }

    public void SwitchEditWindow(object? parameter)
    {
        if (SelectedComment != null)
        {
            var nextWindow = new EditWindow();
            var currentWindow = (Window)parameter;
            var dataContextOfNextWindow = new EditWindowViewModel(SelectedComment.id);
            nextWindow.DataContext = dataContextOfNextWindow;
            nextWindow.Show();
        }
    }

    private void SwitchingWindow<T>(object? parameter) where T : Window, new()
    {
        var nextWindow = new T();
        var currentWindow = (Window)parameter;
        nextWindow.Show();
    }

    public async Task<string> GetAllDataFromJsonApi()
    {
        HttpClient client = new HttpClient();
        var data = await client.GetStringAsync(URL);
        await Task.Delay(2500);
        return data;
    }


    public async Task<ObservableCollection<Comment>> Deserializer1()
    {
        var Photos = await GetAllDataFromJsonApi();
        var result = JsonSerializer.Deserialize<ObservableCollection<Comment>>(Photos);
        return result;
    }
}
