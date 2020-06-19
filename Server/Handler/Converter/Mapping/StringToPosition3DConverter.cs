using AutoMapper;
using System;
using System.Globalization;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringToPosition3DConverter : ITypeConverter<string, Position3D?>
    {
        #region Public Methods

        public Position3D? Convert(string str, Position3D? destination, ResolutionContext context)
        {
            try
            {
                var positions = str.Split('|');
                return new Position3D(
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
