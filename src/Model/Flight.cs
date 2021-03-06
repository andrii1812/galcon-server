﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.Model
{
    using System.Threading;
    using Newtonsoft.Json;

    public class Flight
    {
        private int _currentPosition;
        public int ID { get; private set; }
        [JsonProperty("owner")]
        public int OwnerID { get; private set; }
        [JsonProperty("from")]
        public int FromPlanet { get; private set; }
        [JsonProperty("to")]
        public int ToPlanet { get; private set; }
        public int Population { get; private set; }
        public int TotalPath { get; private set; }
        public int CurrentPosition
        {
            get => _currentPosition;
            private set => _currentPosition = value;
        }

        public Flight(int id, int ownerID, int fromPlanet, int toPlanet, int population, int totalPath)
        {
            ID = id;
            OwnerID = ownerID;
            FromPlanet = fromPlanet;
            ToPlanet = toPlanet;
            Population = population;
            TotalPath = totalPath;
            CurrentPosition = 0;
        }

        public void Move()
        {
            Interlocked.Increment(ref _currentPosition);
        }

        public bool HasArrived()
        {
            return CurrentPosition >= TotalPath;
        }
    }
}
