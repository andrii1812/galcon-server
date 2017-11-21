namespace GalconServer.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Timers;
    using Core;
    using Events;
    using Microsoft.Extensions.Options;
    using Model;

    public class GameManager : AGameManager
    {
        public GameManager(IOptions<Configuration> config) : base(config)
        {
        }

        public override void StartGame(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
            Timer = new System.Timers.Timer(TickInterval);
            TickId = 0;
            Timer.Elapsed += OnTick;
            Map = new Map {Planets = Map.GenerateMap(Player1.Id, Player2.Id, NumberOfPlanets)};
            Flights = new List<Flight>();
            OnGameStarted(new GameStartedEventArgs(TickId, new List<User> {Player1,Player2}, Map));
            Timer.Start();
        }

        protected override void OnTick(object sender, ElapsedEventArgs e)
        {
            if (IsGameOver(out User winner))
            {
                var looser = Player1.Equals(winner) ? Player2 : Player1;
                OnGameOver(new GameOverEventArgs(TickId, winner, $"{looser.Name} have no planets"));
                return;
            }

            Interlocked.Increment(ref TickId);
            var planetUpdates = new List<PlanetUpdate>();
            List<Flight> tmpFlight = Flights.ToList();
            foreach (var flight in tmpFlight)
            {
                flight.Move();
                if (flight.HasArrived())
                {
                    Flights.Remove(flight);
                    Map.GetPlanetById(flight.ToPlanet)?.InteractWithFleet(flight);
                }
            }
            foreach (var planet in Map.Planets)
            {
                planet.IncreasePopulation();
                planetUpdates.Add(planet.ToPlanetUpdate());
            }
            OnTickUpdated(new TickUpdateEventArgs(TickId, planetUpdates, Flights.ToList()));
        }

        protected override void OnGameOver(GameOverEventArgs e)
        {            
            Timer.Stop();
            Timer.Elapsed -= OnTick;
            Timer = null;
            Player1 = null;
            Player2 = null;
            Map = null;
            base.OnGameOver(e);
        }

        public override void PlayerLeft(User player)
        {
            var winner = Player1.Equals(player) ? Player2 : Player1;
            OnGameOver(new GameOverEventArgs(TickId, winner, $"{player.Name} has left"));
        }

        public override List<SendFleetResponse> SendFleet(int senderId, List<SendFleetCommand> commands)
        {
            var result = new List<SendFleetResponse>();
            foreach (var command in commands)
            {
                var planetFrom = Map.GetPlanetById(command.From);
                if (planetFrom.Owner != senderId)
                {
                    result.Add(new SendFleetResponse(-1, ErrorCodes.DoesntOwnFromPlanet));
                }
                else if (planetFrom.Population <= 1)
                {
                    result.Add(new SendFleetResponse(-1, ErrorCodes.NotEnoughPopulation));
                }
                else
                {
                    var planetTo = Map.GetPlanetById(command.To);
                    int flightId = AddNewFlight(senderId, planetFrom, planetTo);
                    result.Add(new SendFleetResponse(flightId));
                }
            }
            return result;
        }

        private bool IsGameOver(out User winner)
        {
            bool isPlayer1Alive = Map.HasPlanets(Player1.Id);
            if (isPlayer1Alive)
            {
                bool isPlayer2Alive = Map.HasPlanets(Player2.Id);
                if (isPlayer2Alive)
                {
                    winner = null;
                    return false;
                }
                else
                {
                    winner = Player1;
                    return true;
                }
            }
            else //todo can both have no planets?
            {
                winner = Player2;
                return true;
            }
        }
    }
}
