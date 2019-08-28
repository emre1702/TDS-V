using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Enum;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Blips
    {
        private static List<List<Blip>> _teamSpawnBlips = new List<List<Blip>>();
        private static List<Blip> _mapLimitBlips = new List<Blip>();
        private static List<Blip> _bombPlantPlaceBlips = new List<Blip>();
        private static Blip _mapCenterBlip;

        public static void AddedPosition(object[] args)
        {
            EMapCreatorPositionType type = (EMapCreatorPositionType)Convert.ToInt32(args[0]);
            float x = Convert.ToSingle(args[1]);
            float y = Convert.ToSingle(args[2]);
            float z = Convert.ToSingle(args[3]);
            string name = "";

            switch (type)
            {
                case EMapCreatorPositionType.TeamSpawn:
                    int teamNumber = Convert.ToInt32(args[4]);
                    while (_teamSpawnBlips.Count <= teamNumber)
                        _teamSpawnBlips.Add(new List<Blip>());
                    var teamSpawnBlip = new Blip(Constants.TeamSpawnBlipSprite, new Vector3(x, y, z), name: name, dimension: Player.LocalPlayer.Dimension);
                    _teamSpawnBlips[teamNumber].Add(teamSpawnBlip);
                    break;

                case EMapCreatorPositionType.MapLimit:
                    var mapLimitBlip = new Blip(Constants.MapLimitBlipSprite, new Vector3(x, y, z), name: name, dimension: Player.LocalPlayer.Dimension);
                    _mapLimitBlips.Add(mapLimitBlip);
                    break;

                case EMapCreatorPositionType.BombPlantPlace:
                    var bombPlantPlaceBlip = new Blip(Constants.BombPlantPlaceBlipSprite, new Vector3(x, y, z), name: name, dimension: Player.LocalPlayer.Dimension);
                    _bombPlantPlaceBlips.Add(bombPlantPlaceBlip);
                    break;

                case EMapCreatorPositionType.MapCenter:
                    _mapCenterBlip?.Destroy();
                    _mapCenterBlip = new Blip(Constants.MapCenterBlipSprite, new Vector3(x, y, z), name: name, dimension: Player.LocalPlayer.Dimension);
                    break;

            }
        }

        public static void RemovedPosition(EMapCreatorPositionType type, int index, int teamNumber)
        {
            switch (type)
            {
                case EMapCreatorPositionType.TeamSpawn:
                    _teamSpawnBlips[teamNumber][index].Destroy();
                    _teamSpawnBlips[teamNumber].RemoveAt(index);
                    if (_teamSpawnBlips[teamNumber].Count == 0)
                        _teamSpawnBlips.RemoveAt(teamNumber);
                    break;

                case EMapCreatorPositionType.MapLimit:
                    _mapLimitBlips[index].Destroy();
                    _mapLimitBlips.RemoveAt(index);
                    break;

                case EMapCreatorPositionType.BombPlantPlace:
                    _bombPlantPlaceBlips[index].Destroy();
                    _bombPlantPlaceBlips.RemoveAt(index);
                    break;
            }
        }

        public static void Reset()
        {
            foreach (var teamList in _teamSpawnBlips)
                foreach (var blip in teamList)
                    blip.Destroy();
            _teamSpawnBlips.Clear();

            foreach (var blip in _mapLimitBlips)
                blip.Destroy();
            _mapLimitBlips.Clear();

            foreach (var blip in _bombPlantPlaceBlips)
                blip.Destroy();
            _bombPlantPlaceBlips.Clear();

            _mapCenterBlip?.Destroy();
            _mapCenterBlip = null;
        }
    }
}
