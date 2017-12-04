namespace GalconServer.Tests.Model
{
    using FluentAssertions;
    using GalconServer.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestForFlight
    {
        [TestMethod]
        public void Move_2Moves_ChangedPosition()
        {
            const int numberOfMoves = 2;
            var flight = new Flight(1, 1, 1, 2, 10, 5);
            int startPosition = flight.CurrentPosition;
            for (int i = 0; i < numberOfMoves; i++)
            {
                flight.Move();
            }
            int endPosotion = flight.CurrentPosition;
            (endPosotion - startPosition).ShouldBeEquivalentTo(numberOfMoves,
                $"because ship has moved {numberOfMoves} times");
        }

        [TestMethod]
        public void HasArrived_NotArrived_False()
        {
            var flight = new Flight(1, 1, 1, 2, 10, 5);
            for (int i = 0; i < flight.TotalPath - 1; i++)
            {
                flight.Move();
            }
            flight.HasArrived().ShouldBeEquivalentTo(false);
        }

        [TestMethod]
        public void HasArrived_Arrived_True()
        {
            var flight = new Flight(1, 1, 1, 2, 10, 5);
            for (int i = 0; i < flight.TotalPath; i++)
            {
                flight.Move();
            }
            flight.HasArrived().ShouldBeEquivalentTo(true);
        }
    }
}
