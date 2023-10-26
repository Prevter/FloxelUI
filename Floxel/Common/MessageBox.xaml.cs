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
		public MessageBoxImage Image = MessageBoxImage.None;
		private bool canClose = false;

		protected override void OnSourceInitialized(EventArgs e)
		{
			this.RemoveIcon();
		}

		private void Init()
		{
			InitializeComponent();

			this.Title = Caption;
			messageTextBlock.Text = Text;

			switch (Image)
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

			switch (Buttons)
			{
                case MessageBoxButton.OK:
                    okButton.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.OKCancel:
                    okButton.Visibility = Visibility.Visible;
                    cancelButton.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    yesButton.Visibility = Visibility.Visible;
                    noButton.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    yesButton.Visibility = Visibility.Visible;
                    noButton.Visibility = Visibility.Visible;
                    cancelButton.Visibility = Visibility.Visible;
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
			Image = icon;
			Init();
		}

		public MessageBox(string message, string caption, MessageBoxButton buttons)
		{
            Text = message;
            Caption = caption;
            Buttons = buttons;
            Init();
        }

		public MessageBox(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon)
		{
            Text = message;
            Caption = caption;
            Buttons = buttons;
            Image = icon;
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

        public static MessageBoxResult Show(string message, string caption, MessageBoxButton buttons)
        {
            MessageBox msgBox = new(message, caption, buttons);
            msgBox.ShowDialog();
            return msgBox.Result;
        }

		public static MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon)
		{
            MessageBox msgBox = new(message, caption, buttons, icon);
            msgBox.ShowDialog();
            return msgBox.Result;
        }

        public static MessageBoxResult Show(string message, string caption, System.Windows.MessageBoxImage icon)
        {
			return Show(message, caption, (MessageBoxImage)icon);
        }

        public static MessageBoxResult Show(string message, string caption, System.Windows.MessageBoxButton buttons)
        {
			return Show(message, caption, (MessageBoxButton)buttons);
        }

        public static MessageBoxResult Show(string message, string caption, System.Windows.MessageBoxButton buttons, System.Windows.MessageBoxImage icon)
        {
			return Show(message, caption, (MessageBoxButton)buttons, (MessageBoxImage)icon);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (sender is not Control control) return;
			Result = control.Tag switch
			{
				"Ok" => MessageBoxResult.OK,
				"Cancel" => MessageBoxResult.Cancel,
				"Yes" => MessageBoxResult.Yes,
				"No" => MessageBoxResult.No,
				_ => Result
			};
			canClose = true;
			Close();
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (canClose) return;

            switch (Buttons)
            {
                case MessageBoxButton.OK:
                    Result = MessageBoxResult.OK;
                    canClose = true;
                    Close();
                    break;
                case MessageBoxButton.OKCancel:
                    Result = MessageBoxResult.Cancel;
					canClose = true;
                    Close();
                    break;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
					// do not allow to close
					e.Cancel = true;
                    break;
            }
        }
	}

	public enum MessageBoxImage
	{
        None = 0,
        Error = 16,
        Question = 32,
        Exclamation = 48,
        Asterisk = 64
    }

	public enum MessageBoxButton
	{
        OK = 0,
        OKCancel = 1,
        YesNo = 4,
        YesNoCancel = 3
    }

	public enum MessageBoxResult
	{
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }
}
