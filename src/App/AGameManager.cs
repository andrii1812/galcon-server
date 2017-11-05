namespace GalconServer.App
{
    using System.Collections.Generic;
    using System.Timers;
    using Core;
    using Events;
    using Model;

    public abstract class AGameManager
    {
        protected Timer _timer;
        protected double _tickInterval;
        protected int _numberOfPlanets = 20; //todo  add to constructor or read from config
        protected int _tickId = 0;

        public User Player1 { get; protected set; }
        public User Player2 { get; protected set; }

        public Map Map { get; protected set; }
        public List<Flight> Flights { get; protected set; }

        public event GameStartedEEventHandler GameStarted;
        public event TickUpdateEventHandler TickUpdated;
        public event GameOverEventHandler GameOver;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickInterval"> interval between ticks in milliseconds </param>
        protected AGameManager(double tickInterval)
        {
            _tickInterval = tickInterval;
        }

        public abstract void StartGame(User player1, User player2);
        public abstract void PlayerLeft(User player);
        public abstract List<SendFleetResponse> SendFleet(int senderId, List<SendFleetCommand> commands);

        protected abstract void OnTick(object sender, ElapsedEventArgs e);

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