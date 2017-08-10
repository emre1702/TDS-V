using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

public class SpawnPosData {
#pragma warning disable IDE1006 // Benennungsstile
	public string team { get; set; }

	public string Xpos { get; set; }
	public string Ypos { get; set; }
	public string Zpos { get; set; }
	public string Zrot { get; set; }
#pragma warning restore IDE1006 // Benennungsstile
}

namespace MapCreator {
	public partial class MainWindow : Window {

		private List<SpawnPosData> playerSpawnListItems = new List<SpawnPosData> ();

		private bool CheckPlayerSpawnPositions ( ) {
			if ( double.TryParse ( xpositionplayerspawnbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number ) )
				if ( number < 10000 && number > -10000 )
					if ( double.TryParse ( ypositionplayerspawnbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) )
						if ( number < 10000 && number > -10000 )
							if ( double.TryParse ( zpositionplayerspawnbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) )
								if ( number < 10000 && number > -10000 )
									return true;
			return false;
		}

		private bool CheckPlayerSpawnRotation ( ) {
			if ( double.TryParse ( zrotationplayerspawnbox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number ) )
				if ( number >= -360 && number <= 360 )
					return true;
			return false;
		}

		private void OnPlayerSpawnAddClick ( object sender, RoutedEventArgs e ) {
			if ( teamnumberplayerspawnbox.Text != "" ) {
				if ( CheckPlayerSpawnPositions () ) {
					if ( CheckPlayerSpawnRotation () ) {
						SpawnPosData posdata = new SpawnPosData {
							team = teamnumberplayerspawnbox.Text,
							Xpos = xpositionplayerspawnbox.Text,
							Ypos = ypositionplayerspawnbox.Text,
							Zpos = zpositionplayerspawnbox.Text,
							Zrot = zrotationplayerspawnbox.Text,
						};
						playerSpawnListItems.Add ( posdata );
						playerspawnlist.Items.Refresh ();
					} else
						MessageBox.Show ( "z-rotation is missing or wrong", "error" );
				} else
					MessageBox.Show ( "positions are missing or wrong", "error" );
			} else
				MessageBox.Show ( "team-number is missing", "error" );
		}

		/* private void OnPlayerSpawnDeleteClear ( object sender, RoutedEventArgs e ) {
			teamnumberplayerspawnbox.Text = "";
			xpositionplayerspawnbox.Text = "";
			ypositionplayerspawnbox.Text = "";
			zpositionplayerspawnbox.Text = "";
			zrotationplayerspawnbox.Text = "";

			if ( playerspawnlist.SelectedIndex != -1 ) {
				for ( int i = playerspawnlist.SelectedItems.Count - 1; i >= 0; i-- ) {
					try {
						playerspawnlistitems.Remove ( (PosData) playerspawnlist.SelectedItems[i] );
						playerspawnlist.Items.Refresh ();
					} catch { }
					//playerspawnlist.Items.RemoveAt ( index );
					//break;
				}
			}
		} */

		private void OnPlayerSpawnsSelectionChanged ( object sender, SelectionChangedEventArgs e ) {
			int index = playerspawnlist.SelectedIndex;
			SpawnPosData posdata = (SpawnPosData) playerspawnlist.SelectedItems[index];
			teamnumberplayerspawnbox.Text = posdata.team;
			xpositionplayerspawnbox.Text = posdata.Xpos;
			ypositionplayerspawnbox.Text = posdata.Ypos;
			zpositionplayerspawnbox.Text = posdata.Zpos;
			zrotationplayerspawnbox.Text = posdata.Zrot;
		}

		private bool CheckPlayerSpawnInput ( ) {
			bool correct = true;
			Dictionary<int, int> amountinteams = new Dictionary<int, int> ();
			for ( int i = 0; i < playerspawnlist.Items.Count && correct; i++ ) {
				correct = false;
				SpawnPosData posdata = (SpawnPosData) playerspawnlist.Items[i];
				if ( int.TryParse ( posdata.team, NumberStyles.Any, CultureInfo.InvariantCulture, out int teamnumber ) ) {
					if ( teamnumber >= 1 && teamnumber <= 99 ) {
						if ( !amountinteams.ContainsKey ( teamnumber ) ) {
							amountinteams[teamnumber] = 0;
						}
						amountinteams[teamnumber]++;
						if ( double.TryParse ( posdata.Xpos, NumberStyles.Any, CultureInfo.InvariantCulture, out double number ) ) {
							if ( number >= -10000 && number <= 10000 ) {
								if ( double.TryParse ( posdata.Ypos, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
									if ( number >= -10000 && number <= 10000 ) {
										if ( double.TryParse ( posdata.Zpos, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
											if ( number >= -10000 && number <= 10000 ) {
												if ( double.TryParse ( posdata.Zrot, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
													if ( number >= -360 && number <= 360 ) {
														correct = true;
													} else
														MessageBox.Show ( "Zrot in player-spawns in " + ( i + 1 ) + ". entry is invalid!", "error" );
												} else
													MessageBox.Show ( "Zrot in player-spawns in " + ( i + 1 ) + ". entry is not a number!", "error" );
											} else
												MessageBox.Show ( "Zpos in player-spawns in " + ( i + 1 ) + ". entry is invalid!", "error" );
										} else
											MessageBox.Show ( "Zpos in player-spawns in " + ( i + 1 ) + ". entry is not a number!", "error" );
									} else
										MessageBox.Show ( "Ypos in player-spawns in " + ( i + 1 ) + ". entry is invalid!", "error" );
								} else
									MessageBox.Show ( "Ypos in player-spawns in " + ( i + 1 ) + ". entry is not a number!", "error" );
							} else
								MessageBox.Show ( "Xpos in player-spawns in " + ( i + 1 ) + ". entry is invalid!", "error" );
						} else
							MessageBox.Show ( "Xpos in player-spawns in " + ( i + 1 ) + ". entry is not a number!", "error" );
					} else
						MessageBox.Show ( "Team-number in player-spawns in " + ( i + 1 ) + ". entry is invalid!", "error" );
				} else
					MessageBox.Show ( "Team-number in player-spawns in " + ( i + 1 ) + ". entry is not a number!", "error" );
			}
			if ( amountinteams.Count > 0 ) {
				foreach ( KeyValuePair<int, int> entry in amountinteams ) {
					if ( entry.Value < 8 ) {
						MessageBox.Show ( "Team "+entry.Key+" does only have "+entry.Value+" spawns ( need atleast 8) in player-spawns!", "error" );
						return false;
					}
				}
			} else
				MessageBox.Show ( "Player-spawns are missing!", "error" );
			return correct;
		}
	}
}