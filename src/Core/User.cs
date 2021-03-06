using System;
using Newtonsoft.Json;

namespace GalconServer.Core
{
    public class User
    {
        private static int IdSource = 0;

        [JsonProperty("playerId")]
        public int Id {get;set;}
        public string Name {get; set;}

        [JsonIgnore]
        public bool IsReady {get;set;} = false;

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as User;

            if(other == null) 
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        internal static int CreateId()
        {
            return IdSource++;
        }
    }
}