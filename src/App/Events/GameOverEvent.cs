using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.App.Events
{
    using GalconServer.Core;

    public delegate void GameOverEventHandler(GameOverEventArgs e);

    public class GameOverEventArgs : EventArgs
    {
        public int TickID { get; private set; }
        public User Winner { get; private set; }
        public string Reason { get; private set; }

        public GameOverEventArgs(int tickID, User winner, string reason)
        {
            Winner = winner;
            Reason = reason;
            TickID = tickID;
        }
    }
}
