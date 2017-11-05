using System;
using System.Collections.Generic;

namespace Galcon.Server.Core
{
    public class Map
    {
        private const double partOfLargePlanets = 0.2;
        private const double partOfMediumPlanets = 0.4;
        private const double startPopulation = 45;
        private const double minDistance = 0.05;
        public List<Planet> Planets;

        public Map(int firstUserID, int secndUserID, int numberOfPlanets)
        {
            Planets = GenerateMap(firstUserID, secndUserID, numberOfPlanets);
        }

        private List<Planet> GenerateMap(int firstUserID, int secndUserID, int numberOfPlanets)
        {
            List<Planet> planetList = new List<Planet>();
            Size size;

            bool isDistanceLongEnough(Planet planet, List<Planet> pList)
            {
                foreach (Planet p in pList)
                {
                    double distance = GetDistanceBetweenPlanets(p, planet);
                    if (distance < minDistance)
                    {
                        return false;
                    }
                }
                return true;
            }

            Planet firstUserPlanet = Planet.GenerateRandomPlanet(1, Size.Huge, firstUserID);
            planetList.Add(firstUserPlanet);
            while (true)
            {
                Planet secndUserPlanet = Planet.GenerateRandomPlanet(2, Size.Huge, secndUserID);
                double d = GetDistanceBetweenPlanets(firstUserPlanet, secndUserPlanet);
                if (d > minDistance)
                {
                    planetList.Add(secndUserPlanet);
                    break;
                }
            }

            int l = (int)(partOfLargePlanets * (double)numberOfPlanets);
            int m = (int)(partOfMediumPlanets * (double)numberOfPlanets);
            for (int i = 3; i <= numberOfPlanets; i++)
            {
                if (l > 0)
                {
                    size = Size.Large;
                    l--;
                }
                else if (m > 0)
                {
                    size = Size.Medium;
                    m--;
                }
                else size = Size.Small;
                Planet planet = Planet.GenerateRandomPlanet(i, size, -1);
                if (isDistanceLongEnough(planet, planetList))
                    planetList.Add(planet);
                else i--;
            }
            return planetList;
        }

        public double GetDistanceBetweenPlanets(Planet firstPlanet, Planet secondPlanet)
        {
            double distance = Math.Pow(firstPlanet.X - secondPlanet.X, 2) + Math.Pow(firstPlanet.Y - secondPlanet.Y, 2);
            return Math.Sqrt(distance);
        }
    }
}
