using System;
using System.Collections.Generic;

namespace GalconServer.App.Events
{
    using Core;

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
