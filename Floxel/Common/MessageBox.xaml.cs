using Material.Icons;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FloxelLib.Common
{
	/// <summary>
	/// Interaction logic for MessageBox.xaml
	/// </summary>
	public partial class MessageBox : FloxelLib.Controls.Window
	{
		public string Caption = "";
		public string Text;
		public MessageBoxButton Buttons = MessageBoxButton.OK;
		public MessageBoxResult Result = MessageBoxResult.None;
		public MessageBoxImage Icon = MessageBoxImage.None;

		protected override void OnSourceInitialized(EventArgs e)
		{
			this.RemoveIcon();
		}

		private void Init()
		{
			InitializeComponent();

			this.Title = Caption;
			messageTextBlock.Text = Text;

			switch (Icon)
			{
				case MessageBoxImage.None:
					messageIcon.Visibility = Visibility.Collapsed;
					break;
				case MessageBoxImage.Error:
					messageIcon.Kind = MaterialIconKind.CloseCircle;
					break;
				case MessageBoxImage.Question:
					messageIcon.Kind = MaterialIconKind.HelpCircle;
					break;
				case MessageBoxImage.Exclamation:
					messageIcon.Kind = MaterialIconKind.Alert;
					break;
				case MessageBoxImage.Asterisk:
					messageIcon.Kind = MaterialIconKind.AlertCircle;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public MessageBox(string message)
		{
			Text = message;
			Init();
		}

		public MessageBox(string message, string caption)
		{
			Text = message;
			Caption = caption;
			Init();
		}

		public MessageBox(string message, string caption, MessageBoxImage icon)
		{
			Text = message;
			Caption = caption;
			Icon = icon;
			Init();
		}

		public static MessageBoxResult Show(string message)
		{
			MessageBox msgBox = new(message);
			msgBox.ShowDialog();
			return msgBox.Result;
		}

		public static MessageBoxResult Show(string message, string caption)
		{
			MessageBox msgBox = new(message, caption);
			msgBox.ShowDialog();
			return msgBox.Result;
		}

		public static MessageBoxResult Show(string message, string caption, MessageBoxImage icon)
		{
			MessageBox msgBox = new(message, caption, icon);
			msgBox.ShowDialog();
			return msgBox.Result;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (sender is not Control control) return;
			Result = control.Tag switch
			{
				"Ok" => MessageBoxResult.OK,
				_ => Result
			};
			Close();
		}
	}
}
