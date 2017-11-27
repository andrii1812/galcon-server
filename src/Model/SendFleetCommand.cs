namespace GalconServer.Model
{
    public class SendFleetCommand
    {
        public  int[] From { get; private set; }
	    public int To { get; private set; }

        public SendFleetCommand(int[] @from, int to)
        {
            To = to;
            From = @from;
        }
    }
}