namespace GalconServer.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Timers;
    using Core;
    using Events;
    using Model;

    public class GameManager : AGameManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickInterval"> interval between ticks in milliseconds </param>
        public GameManager(double tickInterval) : base(tickInterval)
        {
            _tickInterval = tickInterval;
        }

        public override void StartGame(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
            _timer = new System.Timers.Timer(_tickInterval) {Enabled = true};
            _tickId = 0;
            _timer.Elapsed += OnTick;
            Map = new Map(Player1.Id, Player2.Id, _numberOfPlanets);
            OnGameStarted(new GameStartedEventArgs(new List<User> {Player1,Player2}, Map));
            _timer.Start();
        }

        protected override void OnTick(object sender, ElapsedEventArgs e)
        {
            Interlocked.Increment(ref _tickId);
            //todo increment all values
        }

        public override void PlayerLeft(User player)
        {
            var winner = Player1.Equals(player) ? Player2 : Player1;
            OnGameOver(new GameOverEventArgs(winner, $"{player.Name} has left"));
        }

        public override List<SendFleetResponse> SendFleet(List<SendFleetCommand> commands)
        {
            throw new NotImplementedException();
        }
    }
}
