using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.App
{
    using System.Timers;
    using Core;
    using Galcon.Server.Core;
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
            _timer = new Timer(_tickInterval);
            Map = new Map(Player1.ID, Player2.ID, _numberOfPlanets);
        }

        public override void PlayerLeft(User player)
        {
            throw new NotImplementedException();
        }

        public override List<SendFleetResponse> SendFleet(List<SendFleetCommand> commands)
        {
            throw new NotImplementedException();
        }
    }
}
