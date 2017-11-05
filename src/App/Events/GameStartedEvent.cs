using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.App.Events
{
    using Core;
    using Galcon.Server.Core;

    public delegate void GameStartedEEventHandler(GameStartedEventArgs e);

    public class GameStartedEventArgs : EventArgs
    {
        public Map Map { get; private set; }
        public List<User> Players { get; private set; }

        public GameStartedEventArgs(List<User> players, Map map)
        {
            Players = players;
            Map = map;
        }
    }
}
