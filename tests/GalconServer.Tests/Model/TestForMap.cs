namespace GalconServer.Tests.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using GalconServer.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestGetPlanetByIdPlanetFound()
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
        public void TestGetPlanetByIdPlanetNotFound()
        {
            Planet firstPlanet = new Planet(1, Size.Medium, 50, 1, 1, 1);
            Planet secondPlanet = new Planet(2, Size.Medium, 50, 1, 5, 1);
            Map map = new Map();
            List<Planet> planets = new List<Planet>() { firstPlanet, secondPlanet };
            map.Planets = planets;
            Planet planet = map.GetPlanetById(0);
            planet.ShouldBeEquivalentTo(null);
        }

        [TestMethod]
        public void TestGetPlanetByIdEmptyList()
        {
            Map map = new Map();
            List<Planet> planets = new List<Planet>();
            map.Planets = planets;
            Planet planet = map.GetPlanetById(1);
            planet.ShouldBeEquivalentTo(null);
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

        [TestMethod]
        public void TestGenerateMapNumberOfPlanets()
        {
            List<Planet> planets = Map.GenerateMap(1, 2, 100);
            Assert.AreEqual(planets.Count, 100);
        }

        [TestMethod]
        public void TestGenerateMapNumberOfHugePlanets()
        {
            List<Planet> planets = Map.GenerateMap(1, 2, 100);
            Assert.AreEqual(planets.Where(x => x.Size == Size.Huge).ToList().Count, 2);
        }
    }
}
