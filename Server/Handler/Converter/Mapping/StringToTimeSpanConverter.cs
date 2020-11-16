using AutoMapper;
using System;
using TDS.Server.Data.Extensions;

namespace TDS.Server.Handler.Converter.Mapping
{
    internal class StringToTimeSpanConverter : ITypeConverter<string, TimeSpan?>
    {

        public TimeSpan? Convert(string time, TimeSpan? destination, ResolutionContext context)
            => time.ParseTimeSpan();
    }
}
