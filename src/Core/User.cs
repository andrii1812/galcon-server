namespace Galcon.Server.Core
{
    public class User
    {
        public string Name {get; set;}

        public override bool Equals(object obj)
        {
            var other = obj as User;

            if(other == null) 
            {
                return false;
            }

            return other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}