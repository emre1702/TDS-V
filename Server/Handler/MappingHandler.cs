using System;
using System.Threading.Tasks;
using AutoMapper;
using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Converter.Mapping;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Server.Handler
{
    public class MappingHandler
    {
        public IMapper Mapper { get; set; }


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
                        str.Equals("true", StringComparison.OrdinalIgnoreCase) 
                        || str == "1" 
                        || str.Equals("yes", StringComparison.OrdinalIgnoreCase));

                cfg.CreateMap<string, DateTime?>().ConvertUsing<StringToDateTimeConverter>();
                cfg.CreateMap<string, TimeSpan?>().ConvertUsing<StringToTimeSpanConverter>();
                cfg.CreateMap<string, Position3D?>().ConvertUsing<StringToPosition3DConverter>();
                cfg.CreateMap<string, Vector3?>().ConvertUsing<StringToVector3Converter>();

                cfg.CreateMap<string, ITDSPlayer?>().ConvertUsing(new StringNameToPlayerConverter(tdsPlayerHandler));
                cfg.CreateMap<string, Task<Players?>>().ConvertUsing(new StringNameToDBPlayerConverter(databasePlayerHelper));
            });
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }

        public Type GetCorrectDestType(Type sourceType)
            => sourceType switch
            {
                Type player when player == typeof(Players) => typeof(Task<Players?>),
                Type dateTime when dateTime == typeof(DateTime) => typeof(DateTime?),
                Type dateTime when dateTime == typeof(TimeSpan) => typeof(TimeSpan?),
                _ => sourceType
            };

    }
}
