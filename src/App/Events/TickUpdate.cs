using System;

namespace GalconServer.App.Events
{
    public delegate void TickUpdateEventHandler(TickUpdateEventArgs e);

    public class TickUpdateEventArgs : EventArgs
    {

        public TickUpdateEventArgs()
        {
        }
    }
}
