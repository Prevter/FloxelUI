using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FloxelUI.Common
{
	/// <summary>
	/// Interaction logic for MessageBox.xaml
	/// </summary>
	public partial class MessageBox : Window
	{
		public string Caption = "";
		public string Text;
		public MessageBoxButton Buttons = MessageBoxButton.OK;
		public MessageBoxResult Result = MessageBoxResult.None;

		private void Init()
		{
			Floxel.InitWindow(this);
			InitializeComponent();

			this.Title = Caption;
			messageTextBlock.Text = Text;
		}

		public MessageBox(string message) 
		{
			Text = message;
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
			MessageBox msgBox = new(message);
			msgBox.Caption = caption;
			msgBox.ShowDialog();
			return msgBox.Result;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Control control)
			{
				switch (control.Tag) {
					case "Ok":
						Result = MessageBoxResult.OK;
						break;
				}

				Close();
			}
		}
	}
}
