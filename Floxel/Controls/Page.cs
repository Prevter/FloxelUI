namespace FloxelLib.Controls;

public class Page : System.Windows.Controls.Page
{
	public Page()
	{
		Style = System.Windows.Application.Current.Resources["PageStyle"] as System.Windows.Style;
	}
}