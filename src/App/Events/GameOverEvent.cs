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
        public User Winner { get; private set; }
        public string Reason { get; private set; }

        public GameOverEventArgs(User winner, string reason)
        {
            Winner = winner;
            Reason = reason;
        }
    }
}
