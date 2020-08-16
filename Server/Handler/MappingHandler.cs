using AltV.Net.Data;
using AutoMapper;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Converter.Mapping;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler
{
    public class MappingHandler
    {
        #region Public Constructors

        public MappingHandler(ITDSPlayerHandler tdsPlayerHandler, DatabasePlayerHelper databasePlayerHelper)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing(a => a);
                cfg.CreateMap<string, char>().ConvertUsing(a => a[0]);
                cfg.CreateMap<string, int>().ConvertUsing(str => Convert.ToInt32(str));
                cfg.CreateMap<string, float>().ConvertUsing(str => Convert.ToSingle(str));
                cfg.CreateMap<string, double>().ConvertUsing(str => Convert.ToDouble(str));
                cfg.CreateMap<string, bool>().ConvertUsing(str =>
                        str.Equals("true", StringComparison.CurrentCultureIgnoreCase) || str == "1" || str.Equals("yes", StringComparison.CurrentCultureIgnoreCase));

                cfg.CreateMap<string, DateTime?>().ConvertUsing<StringToDateTimeConverter>();
                cfg.CreateMap<string, TimeSpan?>().ConvertUsing<StringToTimeSpanConverter>();
                cfg.CreateMap<string, Position?>().ConvertUsing<StringToPositionConverter>();

                cfg.CreateMap<string, ITDSPlayer?>().ConvertUsing(new StringNameToPlayerConverter(tdsPlayerHandler));
                cfg.CreateMap<string, Task<Players?>>().ConvertUsing(new StringNameToDBPlayerConverter(databasePlayerHelper));
            });
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }

        #endregion Public Constructors

        #region Public Properties

        public IMapper Mapper { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Type GetCorrectDestType(Type sourceType)
            => sourceType switch
            {
                Type player when player == typeof(Players) => typeof(Task<Players?>),
                Type dateTime when dateTime == typeof(DateTime) => typeof(DateTime?),
                Type dateTime when dateTime == typeof(TimeSpan) => typeof(TimeSpan?),
                _ => sourceType
            };

        #endregion Public Methods
    }
}
