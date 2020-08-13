using AltV.Net.Data;
using AutoMapper;
using System.Globalization;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringToPositionConverter : ITypeConverter<string, Position?>
    {
        #region Public Methods

        public Position? Convert(string str, Position? destination, ResolutionContext context)
        {
            try
            {
                var positions = str.Split('|');
                return new Position(
                    ToFloat(positions[0]),
                    ToFloat(positions[1]),
                    ToFloat(positions[2]));
            }
            catch
            {
                return null;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private float ToFloat(string str)
            => float.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);

        #endregion Private Methods
    }
}
