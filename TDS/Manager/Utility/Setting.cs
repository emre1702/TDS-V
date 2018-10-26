using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Entity;

namespace TDS.Manager.Utility
{
    class Setting
    {
        public static string GamemodeName { get => settings.GamemodeName; }
        public static string MapsPath { get => settings.MapsPath; }
        public static string NewMapsPath { get => settings.NewMapsPath; }
        

        private static Settings settings;

        public static async Task Load(TDSNewContext dbcontext)
        {
            settings = await dbcontext.Settings.AsNoTracking().SingleAsync();
        }
    }
}
