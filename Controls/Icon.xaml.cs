using Material.Icons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FloxelUI.Controls
{
	/// <summary>
	/// Interaction logic for Icon.xaml
	/// </summary>
	public partial class Icon : Control
	{
		public Icon()
		{
			InitializeComponent();
		}

		static Icon()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
		}

		public static readonly DependencyProperty KindProperty
			= DependencyProperty.Register(nameof(Kind), typeof(MaterialIconKind), typeof(Icon),
				new PropertyMetadata(default(MaterialIconKind), KindPropertyChangedCallback));

		private static void KindPropertyChangedCallback(DependencyObject dependencyObject,
														DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
			=> ((Icon)dependencyObject).UpdateData();

		/// <summary>
		/// Gets or sets the icon to display.
		/// </summary>
		public MaterialIconKind Kind
		{
			get => (MaterialIconKind)GetValue(KindProperty);
			set => SetValue(KindProperty, value);
		}

		private static readonly DependencyPropertyKey DataPropertyKey
			= DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(Icon), new PropertyMetadata(""));

		public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

		/// <summary>
		/// Gets the icon path data for the current <see cref="Kind"/>.
		/// </summary>
		[TypeConverter(typeof(GeometryConverter))]
		public string? Data
		{
			get => (string?)GetValue(DataProperty);
			private set => SetValue(DataPropertyKey, value);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UpdateData();
		}

		private void UpdateData()
		{
			Data = MaterialIconDataProvider.GetData(Kind);
		}
	}
}
