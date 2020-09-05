﻿using AutoMapper;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Converter.Mapping
{
    internal class StringNameToPlayerConverter : ITypeConverter<string, ITDSPlayer?>
    {
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public StringNameToPlayerConverter(ITDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public ITDSPlayer? Convert(string name, ITDSPlayer? destination, ResolutionContext _)
        {
            if (name.Length >= 2 && name[0] == '@' && name[^1] == ':')
                name = name[1..^1];

            return _tdsPlayerHandler.FindTDSPlayer(name);
        }
    }
}
