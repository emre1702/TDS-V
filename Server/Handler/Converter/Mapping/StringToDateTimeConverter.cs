using AutoMapper;
using System;
using TDS.Server.Data.Extensions;

namespace TDS.Server.Handler.Converter.Mapping
{
    internal class StringToDateTimeConverter : ITypeConverter<string, DateTime?>
    {

        public DateTime? Convert(string time, DateTime? destination, ResolutionContext context)
            => time.ParseDateTime();
    }
}
