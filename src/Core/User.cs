namespace Galcon.Server.Core
{
    public class User
    {
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

            return other.Name == Name && other.ID == ID;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ ID;
        }
    }
}