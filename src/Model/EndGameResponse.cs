namespace GalconServer.Model
{
    public class EndGameResponse
    {
        public int Winner { get; }
        public string Reason { get; }
        public EndGameResponse(int winner, string reason)
        {
            this.Reason = reason;
            this.Winner = winner;
        }
    }
}