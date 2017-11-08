namespace GalconServer.Model
{
    using System;
    using System.Threading;

    public class Planet
    {
        private double _population;

        public int ID { get; set; }
        public Size Size { get; set; }

        public int Population
        {
            get { return (int) _population; }
            set { _population = value; }
        }

        public double X { get; set; }
        public double Y { get; set; }
        public int Owner { get; set; }

        public Planet(int id, Size size, int population, double x, double y, int owner)
        {
            ID = id;
            Size = size;
            Population = population;
            X = x;
            Y = y;
            Owner = owner;
        }

        public static Planet GenerateRandomPlanet(int id, Size size, int owner)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int population = rnd.Next(0, Map.MaxPlanetStartPopulation);
            double x = rnd.NextDouble();
            double y = rnd.NextDouble();
            return new Planet(id, size, population, x, y, owner);
        }

        public PlanetUpdate ToPlanetUpdate()
        {
            return new PlanetUpdate(ID, Population, Owner);
        }

        public void IncreasePopulation()
        {
            if (Owner == -1)//doesn't increase population if  this planet doesn't have owner
            {
                return;
            }
            double addend = (int)Size * Map.PopulationGrowthCoefficient;
            Interlocked.Exchange(ref _population, _population + addend);
        }

        public void InteractWithFleet(Flight flight)
        {
            if (flight.OwnerID == Owner)
            {
                Interlocked.Exchange(ref _population, _population + flight.Population);
            }
            else
            {
                int resultPopulation = Population - flight.Population;
                if (resultPopulation >= 0)
                {
                    Interlocked.Exchange(ref _population, resultPopulation);
                }
                else
                {
                    Population = Math.Abs(resultPopulation);
                    Owner = flight.OwnerID;
                }
            }
        }

        public int SendFleet()
        {
            int toSend = (int)(Population * Map.PartOfPopulationToSend);
            Interlocked.Exchange(ref _population, _population - toSend);
            return toSend;
        }
    }
}
