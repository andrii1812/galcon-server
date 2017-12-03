using FluentAssertions;
using GalconServer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading;

namespace GalconServer.Tests
{
    [TestClass]
    public class TestForPlanets
    {
        [TestMethod]
        public void TestGenerateRandomPlanet()
        {
            int min = 0, max = Map.MaxPlanetStartPopulation;
            var randomizer = Substitute.For<Random>();
            randomizer.Next(min, max).Returns(50);
            randomizer.NextDouble().Returns(1);

            Planet planet = Planet.GenerateRandomPlanet(1, Size.Medium, 1, randomizer);
            Planet expectedPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            planet.ShouldBeEquivalentTo(expectedPlanet);
        }

        [TestMethod]
        public void TestToPlanetUpdate()
        {
            Planet planet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            PlanetUpdate expectedPlanetUpdate = new PlanetUpdate(1, 50, 1);
            PlanetUpdate planetUpdate = planet.ToPlanetUpdate();
            planetUpdate.ShouldBeEquivalentTo(expectedPlanetUpdate);
        }

        [TestMethod]
        public void TestInteractWithFleetChangePlanetOwner()
        {
            Planet planet = new Planet(1, Size.Medium, 50, 1, 1, -1);
            Flight flight = new Flight(1, 2, 1, 2, 100, 1);
            Planet expectedPlanet = new Planet(1, Size.Medium, 50, 1, 1, 2);
            planet.InteractWithFleet(flight);
            planet.ShouldBeEquivalentTo(expectedPlanet);
        }
    }
}
