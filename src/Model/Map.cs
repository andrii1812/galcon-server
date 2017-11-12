namespace GalconServer.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class Map
    {
        public static double PartOfLargePlanets { get; private set; } = 0.2;
        public static double PartOfMediumPlanets { get; private set; } = 0.4;
        public static int PlayerStartPopulation { get; private set; } = 45;
        public static int MaxPlanetStartPopulation { get; private set; } = 40;
        public static double MinDistanceBetweenPlanets { get; private set; } = 0.05;
        public static double PartOfPopulationToSend { get; private set; } = 0.5;
        public static double PopulationGrowthCoefficient { get; private set; } = 0.25;
        
        public List<Planet> Planets;

        public static void Initialize(IOptions<Configuration> config)
        {
            PartOfLargePlanets = config.Value.PartOfLargePlanets;
            PartOfMediumPlanets = config.Value.PartOfMediumPlanets;
            PlayerStartPopulation = config.Value.PlayerStartPopulation;
            MinDistanceBetweenPlanets = config.Value.MinDistanceBetweenPlanets;
            PartOfPopulationToSend = config.Value.PartOfPopulationToSend;
            PopulationGrowthCoefficient = config.Value.PopulationGrowthCoefficient;
            MaxPlanetStartPopulation = config.Value.MaxPlanetStartPopulation;
        }

        public static List<Planet> GenerateMap(int firstUserID, int secndUserID, int numberOfPlanets)
        {
            var planetList = new List<Planet>();

            bool IsDistanceLongEnough(Planet planet, List<Planet> pList)
            {
                return pList.Select(p => GetDistanceBetweenPlanets(p, planet)).All(distance => !(distance < MinDistanceBetweenPlanets));
            }

            Planet firstUserPlanet = Planet.GenerateRandomPlanet(1, Size.Huge, firstUserID);
            firstUserPlanet.Population = PlayerStartPopulation;
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
            secondUserPlanet.Population = PlayerStartPopulation;

            int l = (int)(PartOfLargePlanets * numberOfPlanets);
            int m = (int)(PartOfMediumPlanets * numberOfPlanets);
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
                {
                    if (size == Size.Large)
                        l++;
                    else if (size == Size.Medium)
                        m++;
                    i--;
                }
            }
            return planetList;
        }

        public static double GetDistanceBetweenPlanets(Planet firstPlanet, Planet secondPlanet)
        {
            double distance = Math.Pow(firstPlanet.X - secondPlanet.X, 2) + Math.Pow(firstPlanet.Y - secondPlanet.Y, 2);
            return Math.Sqrt(distance);
        }

        public Planet GetPlanetById(int id)
        {
            return Planets.FirstOrDefault(x => x.ID == id);
        }

        public bool HasPlanets(int ownerId)
        {
            return Planets.Any(x => x.Owner == ownerId);
        }

        public IEnumerable<PlanetUpdate> MapUpdate()
        {
            var planetsUpdate = Planets.Select(x => x.ToPlanetUpdate());
            return planetsUpdate;
        }

        public List<PlanetUpdate> DeserializeMapUpdate(string json)
        {
            return JsonConvert.DeserializeObject<List<PlanetUpdate>>(json);
        }
    }
}
