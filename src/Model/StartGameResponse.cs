using System.Collections.Generic;
using GalconServer.Core;

namespace GalconServer.Model
{
    public class StartGameResponse
    {
        public List<Planet> Map {get;set;}

        public List<User> Players {get;set;}

        public StartGameResponse(List<Planet> map, List<User> players)
        {
            Map = map;
            Players = players;
        }
    }
}