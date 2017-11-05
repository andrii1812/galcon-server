using System;

namespace Galcon.Server.Core
{
    public class User
    {
        private static int IdSource = 0;

        public int Id {get;set;}
        public string Name {get; set;}

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