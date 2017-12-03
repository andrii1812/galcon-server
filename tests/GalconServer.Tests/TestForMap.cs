using FluentAssertions;
using GalconServer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace GalconServer.Tests
{
    [TestClass]
    public class TestForMap
    {
        [TestMethod]
        public void TestGetDistanceBetweenPlanets()
        {
            Planet firstPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            Planet secondPlanet = new Planet(2, Size.Medium, 50, 1, 5, 1);
            double distance = Map.GetDistanceBetweenPlanets(firstPlanet, secondPlanet);
            Assert.AreEqual(distance, 4);
        }

        [TestMethod]
        public void TestGetPlanetById()
        {
            Planet firstPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            Planet secondPlanet = new Planet(2, Size.Medium, 50, 1, 5, 1);
            Map map = new Map();
            List<Planet> planets = new List<Planet>() { firstPlanet, secondPlanet };
            map.Planets = planets;
            Planet planet = map.GetPlanetById(1);
            planet.ShouldBeEquivalentTo(firstPlanet);
        }

        [TestMethod]
        public void TestHasPlanetsHasPlanets()
        {
            Planet firstPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            Map map = new Map();
            List<Planet> planets = new List<Planet>() { firstPlanet };
            map.Planets = planets;
            Assert.IsTrue(map.HasPlanets(1));
        }

        [TestMethod]
        public void TestHasPlanetsHasntPlanets()
        {
            Planet firstPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            Map map = new Map();
            List<Planet> planets = new List<Planet>() { firstPlanet };
            map.Planets = planets;
            Assert.IsFalse(map.HasPlanets(2));
        }
    }
}
