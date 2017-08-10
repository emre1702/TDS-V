using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapCreator {
	public partial class MainWindow : Window {

		private void PositionValidationTextBox ( object sender, TextCompositionEventArgs e ) {
			TextBox textbox = (TextBox) sender;
			bool disallowed = e.Text != "." && e.Text != "-" && !double.TryParse ( e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number );
			if ( e.Text != "." && e.Text != "-" ) {
				if ( double.TryParse ( textbox.Text + e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
					if ( number <= -10000 || number >= 10000 )
						disallowed = true;
				} else
					disallowed = true;
			} else if ( e.Text == "." && ( textbox.Text.IndexOf ( "." ) != -1 || textbox.SelectionStart == 0 ) )
				disallowed = true;
			else if ( e.Text == "-" && ( textbox.SelectionStart != 0 || ( textbox.Text != "" && textbox.Text[0] == '-' ) ) )
				disallowed = true;
			e.Handled = disallowed;
		}

		private void PlayerValidationTextBox ( object sender, TextCompositionEventArgs e ) {
			TextBox textbox = (TextBox) sender;
			bool disallowed = !double.TryParse ( e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number );
			if ( double.TryParse ( textbox.Text + e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
				if ( number < 0 || number > 1000 )
					disallowed = true;
			} else
				disallowed = true;
			e.Handled = disallowed;
		}

		private void RotationValidationTextBox ( object sender, TextCompositionEventArgs e ) {
			TextBox textbox = (TextBox) sender;
			bool disallowed = e.Text != "." && !double.TryParse ( e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number );
			if ( e.Text != "." ) {
				if ( double.TryParse ( textbox.Text + e.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out number ) ) {
					if ( number <= -360 || number >= 360 )
						disallowed = true;

				} else
					disallowed = true;
			} else if ( textbox.Text.IndexOf ( "." ) != -1 )
				disallowed = true;
			e.Handled = disallowed;
		}

		private void TeamNumberValidationTextBox ( object sender, TextCompositionEventArgs e ) {
			e.Handled = !( int.TryParse ( e.Text, out int number ) );
		}
	}
}