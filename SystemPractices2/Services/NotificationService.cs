﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SystemPractices2.Services;

public class NotificationService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}