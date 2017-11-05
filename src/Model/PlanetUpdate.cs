namespace GalconServer.Model
{
    public class PlanetUpdate
    {
        public int ID { get; set; }
        public double Population { get; set; }
        public int Owner { get; set; }

        public PlanetUpdate(int id, double population, int owner)
        {
            ID = id;
            Population = population;
            Owner = owner;
        }
    }
}
