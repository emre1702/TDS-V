using AutoMapper;
using GTANetworkAPI;
using System;
using System.Globalization;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Server.Handler.Converter.Mapping
{
    internal class StringToVector3Converter : ITypeConverter<string, Vector3?>
    {
        public Vector3? Convert(string str, Vector3? destination, ResolutionContext context)
        {
            try
            {
                var positions = str.Split('|');
                return new Vector3(
                    ToFloat(positions[0]),
                    ToFloat(positions[1]),
                    ToFloat(positions[2]));
            }
            catch
            {
                return null;
            }
        }

        private float ToFloat(string str)
            => float.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
    }
}
