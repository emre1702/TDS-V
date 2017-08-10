using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace MapCreator {
	public partial class MainWindow : Window {

		private void CheckMapButtonClick ( object sender, RoutedEventArgs e ) {
			if ( CheckPlayerSpawnInput () ) {
				if ( CheckMapLimitInput() ) {
					if ( CheckMapInfoInput () ) {
						tabmapinfo.IsEnabled = false;
						tabmaplimit.IsEnabled = false;
						tabplayerspawns.IsEnabled = false;
						checkmapbutton.IsEnabled = false;
						createmapbutton.IsEnabled = true;
						MessageBox.Show ( "You can now create the map!" );
					}
				}
			}
		}

		private void CreateMapButtonClick ( object sender, RoutedEventArgs e ) {
			string filename = mapname.Text.Replace ( ' ', '_' ) + ".xml";
			if ( File.Exists ( filename ) ) {
				string oldfilesname = filename + ".old";
				while ( File.Exists ( oldfilesname ) ) {
					oldfilesname += ".old";
				}
				File.Move ( filename, oldfilesname );
			}
			XmlTextWriter writer = new XmlTextWriter ( filename, Encoding.UTF8 );
			writer.WriteStartDocument ();
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 4;
			writer.WriteStartElement ( "TDSMap" );

			// map //
			writer.WriteComment ( "map-info" );
			writer.WriteStartElement ( "map" );
			writer.WriteAttributeString ( "name", mapname.Text );
			writer.WriteAttributeString ( "type", "normal" );
			writer.WriteAttributeString ( "minplayers", minplayers.Text );
			writer.WriteAttributeString ( "maxplayers", maxplayers.Text );
			writer.WriteEndElement ();

			// description //
			writer.WriteComment ( "description" );
			if ( englishdescription.Text != "" ) {
				writer.WriteStartElement ( "english" );
				writer.WriteString ( englishdescription.Text );
				writer.WriteEndElement ();
			}
			if ( germandescription.Text != "" ) {
				writer.WriteStartElement ( "german" );
				writer.WriteString ( germandescription.Text );
				writer.WriteEndElement ();
			}

			// map-limit //
			writer.WriteComment ( "map-limit" );
			for ( int i = 0; i < maplimitlist.Items.Count; i++ ) {
				writer.WriteStartElement ( "limit" );
				MapLimitPosData posdata = (MapLimitPosData) maplimitlist.Items[i];
				writer.WriteAttributeString ( "x", posdata.Xpos );
				writer.WriteAttributeString ( "y", posdata.Ypos );
				writer.WriteEndElement ();
			}

			// player-spawns //
			writer.WriteComment ( "player-spawns" );
			for ( int i = 0; i < playerspawnlist.Items.Count; i++ ) {
				SpawnPosData posdata = (SpawnPosData) playerspawnlist.Items[i];
				writer.WriteStartElement ( "team"+ posdata.team );
				writer.WriteAttributeString ( "x", posdata.Xpos );
				writer.WriteAttributeString ( "y", posdata.Ypos );
				writer.WriteAttributeString ( "z", posdata.Zpos );
				writer.WriteAttributeString ( "rot", posdata.Zrot );
				writer.WriteEndElement ();
			}

			writer.WriteEndElement ();
			writer.WriteEndDocument ();
			writer.Close ();

			MessageBox.Show ( "Map was successfully created! Restart the programm if you want to create a new map." );
		}
	}
}