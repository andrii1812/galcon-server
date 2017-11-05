using System;

namespace Galcon.Server.Core
{
    public class User
    {
        private static int IdSource = 0;

        public int Id {get;set;}
        public string Name {get; set;}
        public int ID { get; private set; }


        public User(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as User;

            if(other == null) 
            {
                return false;
            }

<<<<<<< HEAD
            return other.Id == Id;
=======
            return other.Name == Name && other.ID == ID;
>>>>>>> ff49eae4106edb123ae2fcc50d52a9769b0f7d1a
        }

        public override int GetHashCode()
        {
<<<<<<< HEAD
            return Id.GetHashCode();
        }

        internal static int CreateId()
        {
            return IdSource++;
=======
            return Name.GetHashCode() ^ ID;
>>>>>>> ff49eae4106edb123ae2fcc50d52a9769b0f7d1a
        }
    }
}