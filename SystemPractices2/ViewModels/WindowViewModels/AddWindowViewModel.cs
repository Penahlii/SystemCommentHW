using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using SystemPractices2.Commands;
using SystemPractices2.Models;
using SystemPractices2.Services;

namespace SystemPractices2.ViewModels.WindowViewModels;

public class AddWindowViewModel : NotificationService
{
    private ObservableCollection<Comment> comments;
    private string name;
    private string email;
    private string body;

    private string jsonFilePath = "comments.json";
    public string Name { get => this.name; set { this.name = value; OnPropertyChanged(); } }
    public string Email { get => this.email; set { this.email = value; OnPropertyChanged(); } }
    public string Body { get => this.body; set { this.body = value; OnPropertyChanged(); } }
    public ObservableCollection<Comment> Comments { get => this.comments; set { this.comments = value; OnPropertyChanged(); } }
    public Comment newComment;

    public ICommand? AddCommand { get; set; }

    public AddWindowViewModel()
    {
        UploadComments();

        AddCommand = new RelayCommand(Add);


    }

    public async void UploadComments()
    {
        string jsonData = File.ReadAllText(jsonFilePath);
        this.Comments = JsonSerializer.Deserialize<ObservableCollection<Comment>>(jsonData);
    }

    public void Add(object? parameter)
    {
        newComment = CreateNewComment();
        this.Comments.Add(newComment);
        SaveData(jsonFilePath);
        var currentWindow = (Window)parameter;
        currentWindow.Close();
    }

    public Comment CreateNewComment()
    {
        newComment = new();
        var id = this.Comments.Last().id;
        var postId = this.Comments.Last().postId;
        newComment.postId = postId;
        newComment.id = id + 1;
        newComment.email = Email;
        newComment.body = Body;
        newComment.name = Name;
        return newComment;
    }

    public void SaveData(string Path)
    {
        string jsonData = JsonSerializer.Serialize(Comments);
        File.WriteAllText(Path, jsonData);
    }
}
