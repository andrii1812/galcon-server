using System;

namespace GalconServer.App.Events
{
    using System.Collections.Generic;
    using Core;
    using Model;

    public delegate void TickUpdateEventHandler(TickUpdateEventArgs e);

    public class TickUpdateEventArgs : EventArgs
    {
        public List<Flight> Flights { get; private set; }
        public Map Map { get; private set; }

        public TickUpdateEventArgs(Map map, List<Flight> flights)
        {
            Map = map;
            Flights = flights;
        }
    }
}
