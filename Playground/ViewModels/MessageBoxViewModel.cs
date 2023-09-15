using System.Collections.Generic;
using System.Windows;
using FloxelLib.MVVM;
using MessageBox = FloxelLib.Common.MessageBox;

namespace FloxelPlayground.ViewModels;

public sealed partial class MessageBoxViewModel : BaseViewModel
{
    private string _messageBoxTitle = "", _messageBoxMessage = "";
    private MessageBoxImage _messageBoxImage = MessageBoxImage.None;
    private MessageBoxButton _messageBoxButton = MessageBoxButton.OK;
    
    public List<MessageBoxImage> MessageBoxImages { get; } = new()
    {
        MessageBoxImage.None,
        MessageBoxImage.Error,
        MessageBoxImage.Question,
        MessageBoxImage.Exclamation,
        MessageBoxImage.Asterisk
    };
    
    public string MessageBoxTitle
    {
        get => _messageBoxTitle;
        set => SetField(ref _messageBoxTitle, value);
    }
    
    public string MessageBoxMessage
    {
        get => _messageBoxMessage;
        set => SetField(ref _messageBoxMessage, value);
    }
    
    public MessageBoxImage MessageBoxImage
    {
        get => _messageBoxImage;
        set => SetField(ref _messageBoxImage, value);
    }
    
    [RelayCommand]
    private void OpenMessageBox()
    {
        MessageBox.Show(_messageBoxMessage, _messageBoxTitle, _messageBoxImage);
    }
    
    [RelayCommand]
    private void OpenDefaultMessageBox()
    {
        System.Windows.MessageBox.Show(_messageBoxMessage, _messageBoxTitle, MessageBoxButton.OK, _messageBoxImage);
    }
}