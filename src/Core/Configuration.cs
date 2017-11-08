namespace GalconServer.Core
{
    public class Configuration
    {
        public string Host {get; set;}

        public string Port {get;set;}

        public int TickInterval { get; set; }

        public int NumberOfPlanets { get; set; }

        /// <summary>
        /// todo
        /// </summary>
        public double FleetSpeed { get; set; }

        public double PartOfLargePlanets { get; set; }

        public double PartOfMediumPlanets { get; set; }

        public int PlayerStartPopulation { get; set; }

        public int MaxPlanetStartPopulation { get; set; }

        public double MinDistanceBetweenPlanets { get; set; }

        public double PartOfPopulationToSend { get; set; }

        public double PopulationGrowthCoefficient { get; set; }
    }
}