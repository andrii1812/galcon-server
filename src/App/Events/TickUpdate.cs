using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.App.Events
{
    using Core;
    using Galcon.Server.Core;

    public delegate void TickUpdateEventHandler(TickUpdateEventArgs e);

    public class TickUpdateEventArgs : EventArgs
    {

        public TickUpdateEventArgs()
        {
        }
    }
}
