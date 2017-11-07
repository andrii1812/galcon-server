using System.Collections.Generic;

namespace GalconServer.Model
{
    public class TickUpdate
    {
        public IEnumerable<PlanetUpdate> Planets {get;set;}

        public IEnumerable<Flight> Flights {get;set;}

        public TickUpdate(IEnumerable<PlanetUpdate> planets, IEnumerable<Flight> flights)
        {
            Planets = planets;
            Flights = flights;
        }
    }
}