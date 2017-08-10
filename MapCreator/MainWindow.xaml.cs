using System.Windows;

namespace MapCreator {
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window {

		public MainWindow ( ) {
			InitializeComponent ();
			playerspawnlist.ItemsSource = playerSpawnListItems;
			maplimitlist.ItemsSource = mapLimitListItems;
		}
	}
}
