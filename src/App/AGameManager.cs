namespace GalconServer.App
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Timers;
    using Core;
    using Events;
    using Microsoft.Extensions.Options;
    using Model;

    public abstract class AGameManager
    {
        protected System.Timers.Timer Timer;
        protected double TickInterval;
        protected int NumberOfPlanets = 20;
        protected double FleetSpeed = 0.03;
        protected int TickId = 0;
        protected int LastFlightId = 0;

        public User Player1 { get; protected set; }
        public User Player2 { get; protected set; }

        public Map Map { get; protected set; }
        public List<Flight> Flights { get; protected set; }
        public int TickID => TickId;

        public event GameStartedEEventHandler GameStarted;
        public event TickUpdateEventHandler TickUpdated;
        public event GameOverEventHandler GameOver;

        protected AGameManager(IOptions<Configuration> config) : this(config.Value.TickInterval)
        {
            NumberOfPlanets = config.Value.NumberOfPlanets;
            FleetSpeed = config.Value.FleetSpeed;
            Map.Initialize(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickInterval"> interval between ticks in milliseconds </param>
        protected AGameManager(double tickInterval)
        {
            TickInterval = tickInterval;
        }

        public abstract void StartGame(User player1, User player2);
        public abstract void PlayerLeft(User player);
        public abstract List<SendFleetResponse> SendFleet(int senderId, List<SendFleetCommand> commands);
        public abstract bool IsGameOver(out User winner);

        protected abstract void OnTick(object sender, ElapsedEventArgs e);

        protected virtual int AddNewFlight(int sender, Planet from, Planet to)
        {
            Interlocked.Increment(ref LastFlightId);
            int flightId = LastFlightId;
            int distanceInTicks = (int) (Map.GetDistanceBetweenPlanets(from, to) / FleetSpeed);
            var flight = new Flight(flightId, sender, from.ID, to.ID, from.SendFleet(), distanceInTicks);
            Flights.Add(flight);
            return flightId;
        }

        protected virtual void OnGameStarted(GameStartedEventArgs e)
        {
            GameStarted?.Invoke(e);
        }

        protected virtual void OnTickUpdated(TickUpdateEventArgs e)
        {
            TickUpdated?.Invoke(e);
        }

        protected virtual void OnGameOver(GameOverEventArgs e)
        {
            GameOver?.Invoke(e);
        }
    }
}