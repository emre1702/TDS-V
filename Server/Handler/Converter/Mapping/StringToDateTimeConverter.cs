using AutoMapper;
using System;
using TDS_Server.Data.Extensions;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringToDateTimeConverter : ITypeConverter<string, DateTime?>
    {

        public DateTime? Convert(string time, DateTime? destination, ResolutionContext context)
            => time.ParseDateTime();
    }
}
