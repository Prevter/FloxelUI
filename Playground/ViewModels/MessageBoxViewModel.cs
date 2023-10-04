﻿using FloxelLib.MVVM;
using System.Collections.Generic;
using System.Windows;
using MessageBox = FloxelLib.Common.MessageBox;

namespace FloxelPlayground.ViewModels;

public sealed partial class MessageBoxViewModel : BaseViewModel
{
	[UpdateProperty]
	private string _messageBoxTitle = "", _messageBoxMessage = "";

	[UpdateProperty]
	private MessageBoxImage _selectedIcon = MessageBoxImage.None;

	public List<MessageBoxImage> MessageBoxImages { get; } = new()
	{
		MessageBoxImage.None,
		MessageBoxImage.Error,
		MessageBoxImage.Question,
		MessageBoxImage.Exclamation,
		MessageBoxImage.Asterisk
	};

	[RelayCommand]
	private void OpenMessageBox()
	{
		MessageBox.Show(_messageBoxMessage, _messageBoxTitle, _selectedIcon);
	}

	[RelayCommand]
	private void OpenDefaultMessageBox()
	{
		System.Windows.MessageBox.Show(_messageBoxMessage, _messageBoxTitle, MessageBoxButton.OK, _selectedIcon);
	}
}