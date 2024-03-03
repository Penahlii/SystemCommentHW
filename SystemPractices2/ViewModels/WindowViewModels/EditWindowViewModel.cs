using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using SystemPractices2.Commands;
using SystemPractices2.Models;
using SystemPractices2.Services;

namespace SystemPractices2.ViewModels.WindowViewModels;

public class EditWindowViewModel : NotificationService
{
    // private fields

    private ObservableCollection<Comment> comments;
    private string name;
    private string email;
    private string body;
    private string jsonFilePath = "comments.json";

    // public fields

    public Comment SelectedComment { get; set; }
    public string Name { get => this.name; set { this.name = value; OnPropertyChanged(); } }
    public string Email { get => this.email; set { this.email = value; OnPropertyChanged(); } }
    public string Body { get => this.body; set { this.body = value; OnPropertyChanged(); } }
    public ObservableCollection<Comment> Comments { get => this.comments; set { this.comments = value; OnPropertyChanged(); } }

    public ICommand? EditCommand { get; set; } 

    public EditWindowViewModel(int selectedCommentID)
    {
        UploadComments();

        this.SelectedComment = FindTheComment(selectedCommentID);

        this.Email = SelectedComment.email;
        this.Name = SelectedComment.name;
        this.Body = SelectedComment.body;

        this.EditCommand = new RelayCommand(Edit);
    }

    public async void Edit(object? parameter)
    {
        this.SelectedComment.email = Email;
        this.SelectedComment.name = Name;
        this.SelectedComment.body = Body;

        string jsonData = JsonSerializer.Serialize(Comments);
        File.WriteAllText(jsonFilePath, jsonData);

        var currentWindow = (Window)parameter;
        currentWindow.Close();
    }

    public async void UploadComments()
    {
        string jsonData = File.ReadAllText(jsonFilePath);
        this.Comments = JsonSerializer.Deserialize<ObservableCollection<Comment>>(jsonData);
    }

    public Comment FindTheComment(int id)
    {
        foreach (var com in Comments)
        {
            if (com.id == id)
                return com;
        }
        return null;
    }
}
