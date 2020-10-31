using AutoMapper;
using System;
using TDS_Server.Data.Extensions;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringToTimeSpanConverter : ITypeConverter<string, TimeSpan?>
    {

        public TimeSpan? Convert(string time, TimeSpan? destination, ResolutionContext context)
            => time.ParseTimeSpan();
    }
}
