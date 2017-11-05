namespace GalconServer.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Newtonsoft.Json;

    public class Map
    {
        private const double _partOfLargePlanets = 0.2;
        private const double _partOfMediumPlanets = 0.4;
        private const double _startPopulation = 45;
        private const double _minDistance = 0.05;
        public List<Planet> Planets;

        public static List<Planet> GenerateMap(int firstUserID, int secndUserID, int numberOfPlanets)
        {
            var planetList = new List<Planet>();

            bool IsDistanceLongEnough(Planet planet, List<Planet> pList)
            {
                return pList.Select(p => GetDistanceBetweenPlanets(p, planet)).All(distance => !(distance < _minDistance));
            }

            Planet firstUserPlanet = Planet.GenerateRandomPlanet(1, Size.Huge, firstUserID);
            firstUserPlanet.Population = _startPopulation;
            planetList.Add(firstUserPlanet);
            Planet secondUserPlanet;
            while (true)
            {
                secondUserPlanet = Planet.GenerateRandomPlanet(2, Size.Huge, secndUserID);
                if (IsDistanceLongEnough(secondUserPlanet, planetList))
                {
                    planetList.Add(secondUserPlanet);
                    break;
                }
            }
            secondUserPlanet.Population = _startPopulation;

            int l = (int)(_partOfLargePlanets * numberOfPlanets);
            int m = (int)(_partOfMediumPlanets * numberOfPlanets);
            for (int i = 3; i <= numberOfPlanets; i++)
            {
                Size size;
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
                else
                    size = Size.Small;
                Planet planet = Planet.GenerateRandomPlanet(i, size, -1);
                if (IsDistanceLongEnough(planet, planetList))
                    planetList.Add(planet);
                else
                    i--;
            }
            return planetList;
        }

        public static double GetDistanceBetweenPlanets(Planet firstPlanet, Planet secondPlanet)
        {
            double distance = Math.Pow(firstPlanet.X - secondPlanet.X, 2) + Math.Pow(firstPlanet.Y - secondPlanet.Y, 2);
            return Math.Sqrt(distance);
        }

        public string SerializeMap()
        {
            return JsonConvert.SerializeObject(Planets);
        }

        public List<Planet> DeserializeMap(string json)
        {
            return JsonConvert.DeserializeObject<List<Planet>>(json);
        }

        public string SerializeMapUpdate()
        {
            var planetsUpdate = Planets.Select(x => x.ToPlanetUpdate());
            return JsonConvert.SerializeObject(planetsUpdate);
        }

        public List<PlanetUpdate> DeserializeMapUpdate(string json)
        {
            return JsonConvert.DeserializeObject<List<PlanetUpdate>>(json);
        }
    }
}
