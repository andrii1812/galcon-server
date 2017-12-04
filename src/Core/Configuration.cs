namespace GalconServer.Core
{
    public class Configuration
    {
        public string Host {get; set;} = "0.0.0.0";

        public string Port {get;set;} = "5050";

        public int TickInterval { get; set; } = 1000;

        public int NumberOfPlanets { get; set; }  = 20;

        public double FleetSpeed { get; set; } = 0.03;

        public double PartOfLargePlanets { get; set; } = 0.2;

        public double PartOfMediumPlanets { get; set; } = 0.4;

        public int PlayerStartPopulation { get; set; } = 45;

        public int MaxPlanetStartPopulation { get; set; } = 40;

        public double MinDistanceBetweenPlanets { get; set; } = 0.05;

        public double PartOfPopulationToSend { get; set; } = 0.5;

        public double PopulationGrowthCoefficient { get; set; } = 0.25;
    }
}