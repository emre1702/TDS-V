using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

public class MapLimitPosData {
	public int ID { get; set; }
	public string Xpos { get; set; }
	public string Ypos { get; set; }
}

namespace MapCreator {
	public partial class MainWindow : Window {

		private List<MapLimitPosData> mapLimitListItems = new List<MapLimitPosData> ();
		private static int mapLimitCounterID = 1;

		private bool CheckMapLimitPositions ( ) {
			if ( double.TryParse ( xposmaplimitbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number ) )
				if ( number < 10000 && number > -10000 )
					if ( double.TryParse ( yposmaplimitbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) )
						if ( number < 10000 && number > -10000 )
							return true;
			return false;
		}

		private void OnMapLimitAddClick ( object sender, RoutedEventArgs e ) {
			if ( CheckMapLimitPositions () ) {
				MapLimitPosData posdata = new MapLimitPosData {
					ID = mapLimitCounterID++,
					Xpos = xposmaplimitbox.Text,
					Ypos = yposmaplimitbox.Text
				};
				mapLimitListItems.Add ( posdata );
				maplimitlist.ItemsSource = null;
				maplimitlist.ItemsSource = mapLimitListItems;
			} else
				MessageBox.Show ( "positions are missing or wrong", "error" );
		}

		private void OnMapLimitPreviewKeyDown ( object sender, KeyEventArgs e ) {
			if ( e.Key == Key.Delete ) {
				DataGrid grid = (DataGrid) sender;
				if ( grid.SelectedItems.Count > 0 ) {
					mapLimitCounterID -= grid.SelectedItems.Count;
				}
			}
		}

		private bool CheckMapLimitInput ( ) {
			maplimitlist.Items.SortDescriptions.Clear ();
			maplimitlist.Items.SortDescriptions.Add ( new System.ComponentModel.SortDescription ( "ID", ListSortDirection.Ascending ) );
			foreach ( System.Windows.Controls.DataGridColumn col in maplimitlist.Columns ) {
				col.SortDirection = null;
			}
			maplimitlist.Columns[0].SortDirection = ListSortDirection.Ascending;
			maplimitlist.Items.Refresh ();

			bool correct = true;

			if ( maplimitlist.Items.Count > 0 && maplimitlist.Items.Count < 3 ) {
				MessageBox.Show ( "Map-limit is invalid! It's a 3D game, not 2D.", "error" );
				correct = false;
			} else if ( maplimitlist.Items.Count > 10 ) {
				MessageBox.Show ( "Map-limit is invalid! Too many edges!", "error" );
				correct = false;
			}

			for ( int i = 0; i < maplimitlist.Items.Count && correct; i++ ) {
				correct = false;
				MapLimitPosData posdata = (MapLimitPosData) maplimitlist.Items[i];
				if ( double.TryParse ( posdata.ID.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double number ) ) {
					if ( number == i + 1 ) {
						if ( double.TryParse ( posdata.Xpos, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
							if ( number >= -10000 && number <= 10000 ) {
								if ( double.TryParse ( posdata.Ypos, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
									if ( number >= -10000 && number <= 10000 ) {
										correct = true;
									} else
										MessageBox.Show ( "Ypos in map-limit in " + ( i + 1 ) + ". entry is invalid!", "error" );
								} else
									MessageBox.Show ( "Ypos in map-limit in " + ( i + 1 ) + ". entry is not a number!", "error" );
							} else
								MessageBox.Show ( "Xpos in map-limit in " + ( i + 1 ) + ". entry is invalid!", "error" );
						} else
							MessageBox.Show ( "Xpos in map-limit in " + ( i + 1 ) + ". entry is not a number!", "error" );
					} else
						MessageBox.Show ( "map-limit ID " + ( i + 1 ) + " is missing!", "error" );
				}
			}

			return correct;
		}
	}
}