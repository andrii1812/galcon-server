using System;
using System.Collections.Generic;

namespace GalconServer.App.Events
{
    using Core;
    using Model;

    public delegate void GameStartedEEventHandler(GameStartedEventArgs e);

    public class GameStartedEventArgs : EventArgs
    {
        public int TickID { get; private set; }
        public Map Map { get; private set; }
        public List<User> Players { get; private set; }

        public GameStartedEventArgs(int tickID, List<User> players, Map map)
        {
            Players = players;
            Map = map;
            TickID = tickID;
        }
    }
}
