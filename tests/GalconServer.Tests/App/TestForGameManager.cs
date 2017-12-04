namespace GalconServer.Tests.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Core;
    using FluentAssertions;
    using FluentAssertions.Events;
    using GalconServer.App;
    using GalconServer.App.Events;
    using GalconServer.Model;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestForGameManager
    {
       [TestMethod]
        public void StartGame_PlayerOneNull_ArgumentNullExceptionThrown()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);

            Action action = () =>
            {
                gameManager.StartGame(null, new User(2, "User 2"));
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void StartGame_PlayerTwoNull_ArgumentNullExceptionThrown()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);

            Action action = () =>
            {
                gameManager.StartGame(new User(1, "User 1"), null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void StartGame_PlayerAreEqual_ArgumentExceptionThrown()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);

            Action action = () =>
            {
                gameManager.StartGame(new User(1, "User 1"), new User(1, "User 1"));
            };

            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void StartGame_ValidData_GameStartedEventRaised()
        {
            bool isEventRaised = false;
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            //todo fluent assertion EventMonitor doesn't work with .Net Core?
            gameManager.GameStarted += GameManager_GameStarted;

            gameManager.StartGame(new User(1, "User 1"), new User(2, "User 2"));

            void GameManager_GameStarted(GalconServer.App.Events.GameStartedEventArgs e)
            {
                e.Players.Should().HaveCount(2);
                e.Map.Planets.Should().HaveCount(configuration.Value.NumberOfPlanets, "Number of planets are set in configuration");
                isEventRaised = true;
            }
            isEventRaised.ShouldBeEquivalentTo(true, "because game should have started");
        }

        [TestMethod]
        public void PlayerLeft_Null_ArgumentNullExceptionThrown()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            gameManager.StartGame(new User(1, "User 1"), new User(2, "User 2"));

            Action action = () =>
            {
                gameManager.PlayerLeft(null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void PlayerLeft_UnknownPlayer_ArgumentOutOfRangeExceptionThrown()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            gameManager.StartGame(new User(1, "User 1"), new User(2, "User 2"));

            Action action = () =>
            {
                gameManager.PlayerLeft(new User(3, "User 3"));
            };

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void PlayerLeft_Player1_GameOverEventRaisedPlayer2Won()
        {
            bool isEventRaised = false;
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);
            gameManager.GameOver += GameManager_GameOver;
            void GameManager_GameOver(GalconServer.App.Events.GameOverEventArgs e)
            {
                isEventRaised = true;
                e.Winner.Should().Be(player2, "because player 1 has left");
            }

            gameManager.PlayerLeft(player1);

            isEventRaised.ShouldBeEquivalentTo(true, "because game should be over after player has left");
        }

        [TestMethod]
        public void PlayerLeft_Player2_GameOverEventRaisedPlayer1Won()
        {
            bool isEventRaised = false;
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);
            gameManager.GameOver += GameManager_GameOver;
            void GameManager_GameOver(GalconServer.App.Events.GameOverEventArgs e)
            {
                isEventRaised = true;
                e.Winner.Should().Be(player1, "because player 1 has left");
            }

            gameManager.PlayerLeft(player2);

            isEventRaised.ShouldBeEquivalentTo(true, "because game should be over after player has left");
        }

        [TestMethod]
        public void SendFleet_UnknownId_ArgumentOutOfRangeException()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);

            Action action = () =>
            {
                gameManager.SendFleet(-1, new List<SendFleetCommand>());
            };

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void SendFleet_AllValid_SuccessfulResults()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);
            foreach (var planet in gameManager.Map.Planets.Where(x=>x.Owner!= player2.Id))
            {
                planet.Owner = player1.Id;
                planet.Population += 10;//to have enough population for fleet send
            }
            var planets = gameManager.Map.Planets
                .Where(x => x.Owner == player1.Id)
                .Select(x=>x.ID)
                .ToArray();
            var commandList = new List<SendFleetCommand>() {new SendFleetCommand(planets, 2)};

            var responses = gameManager.SendFleet(player1.Id, commandList);

            responses.Should().HaveCount(planets.Length);
            responses.Where(x => x.ErrorCode == ErrorCodes.Success).Should().HaveCount(planets.Length);
        }

        [TestMethod]
        public void SendFleet_AllInvalid_AllDoesntOwnFromPlanet()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);
            var planets = gameManager.Map.Planets
                .Where(x => x.Owner != player1.Id)
                .Select(x => x.ID)
                .ToArray();
            var commandList = new List<SendFleetCommand>() { new SendFleetCommand(planets, 2) };

            var responses = gameManager.SendFleet(player1.Id, commandList);

            responses.Should().HaveCount(planets.Length);
            responses.Where(x => x.ErrorCode == ErrorCodes.DoesntOwnFromPlanet).Should().HaveCount(planets.Length);
        }

        [TestMethod]
        public void SendFleet_EmptyPlanet_NotEnoughPopulation()
        {
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.StartGame(player1, player2);
            var planet = gameManager.Map.Planets.First(x => x.Owner == player1.Id);
            planet.Population = 1;
            var commandList = new List<SendFleetCommand>() { new SendFleetCommand(new[]{planet.ID}, 2) };

            var responses = gameManager.SendFleet(player1.Id, commandList);

            responses.Should().HaveCount(1);
            responses.Where(x => x.ErrorCode == ErrorCodes.NotEnoughPopulation).Should().HaveCount(1);
        }

        [TestMethod]
        public void GameOverEvent_Player1HaveNoPlanets_GameOverEventRaised()
        {
            bool isGameOverEventRaised = false;
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.GameOver += GameManagerOnGameOver;
            void GameManagerOnGameOver(GameOverEventArgs gameOverEventArgs)
            {
                isGameOverEventRaised = true;
                gameOverEventArgs.Winner.ShouldBeEquivalentTo(player2, "because he captured all planets");
            }
            gameManager.StartGame(player1, player2);

            gameManager.Map.Planets.ForEach(x=>x.Owner=player2.Id);//player2 "captures" all planets
            Thread.Sleep(configuration.Value.TickInterval * 3);//waiting for tick update

            isGameOverEventRaised.ShouldBeEquivalentTo(true);
        }
        
        [TestMethod]
        public void GameOverEvent_Player2HaveNoPlanets_GameOverEventRaised()
        {
            bool isGameOverEventRaised = false;
            IOptions<Configuration> configuration = Options.Create(new Configuration());
            var gameManager = new GameManager(configuration);
            var player1 = new User(1, "User 1");
            var player2 = new User(2, "User 2");
            gameManager.GameOver += GameManagerOnGameOver;
            void GameManagerOnGameOver(GameOverEventArgs gameOverEventArgs)
            {
                isGameOverEventRaised = true;
                gameOverEventArgs.Winner.ShouldBeEquivalentTo(player1, "because he captured all planets");
            }
            gameManager.StartGame(player1, player2);

            gameManager.Map.Planets.ForEach(x => x.Owner = player1.Id);//player1 "captures" all planets
            Thread.Sleep(configuration.Value.TickInterval * 3);//waiting for tick update

            isGameOverEventRaised.ShouldBeEquivalentTo(true);
        }
    }
}
