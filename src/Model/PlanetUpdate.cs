namespace GalconServer.Model
{
    public class PlanetUpdate
    {
        public int ID { get; set; }
        public int Population { get; set; }
        public int Owner { get; set; }

        public PlanetUpdate(int id, int population, int owner)
        {
            ID = id;
            Population = population;
            Owner = owner;
        }
    }
}
