using System.Windows;

namespace MapCreator {
	public partial class MainWindow : Window {

		private bool CheckMapInfoInput ( ) {
			if ( mapname.Text.Length >= 5 ) {
				if ( minplayers.Text != "" && int.TryParse ( minplayers.Text, out int number ) ) {
					if ( maxplayers.Text != "" && int.TryParse ( minplayers.Text, out number ) ) {
						return true;
					} else {
						MessageBox.Show ( "Max. players is invalid!", "error" );
						return false;
					}
				} else {
					MessageBox.Show ( "Min. players is invalid!", "error" );
					return false;
				}
			} else {
				MessageBox.Show ( "Map-name is too short (atleast 5 chars)", "error" );
				return false;
			}
		}
	}
}