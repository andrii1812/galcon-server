using System;

namespace GalconServer.App.Events
{
    using System.Collections.Generic;
    using Model;

    public delegate void TickUpdateEventHandler(TickUpdateEventArgs e);

    public class TickUpdateEventArgs : EventArgs
    {
        public int TickID { get; private set; }
        public List<Flight> Flights { get; private set; }
        public List<PlanetUpdate> PlanetUpdate { get; private set; }

        public TickUpdateEventArgs(int tickID, List<PlanetUpdate> planetUpdate, List<Flight> flights)
        {
            PlanetUpdate = planetUpdate;
            Flights = flights;
            TickID = tickID;
        }
    }
}
