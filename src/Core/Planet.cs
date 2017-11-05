using System;

namespace GalconServer.Core
{
    public class Planet
    {
        public int ID { get; set; }
        public Size Size { get; set; }
        public double Population { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Owner { get; set; }

        public Planet(int id, Size size, double population, double x, double y, int owner)
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
            double population = rnd.Next(0, 100);
            double x = rnd.NextDouble();
            double y = rnd.NextDouble();
            return new Planet(id, size, population, x, y, owner);
        }

        public PlanetUpdate ToPlanetUpdate()
        {
            return new PlanetUpdate(ID, Population, Owner);
        }
    }
}
