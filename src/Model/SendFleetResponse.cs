namespace GalconServer.Model
{
    public class SendFleetResponse
    {
        public int FleetId { get; private set; }
        public int ErrorCode { get; private set; }

        public SendFleetResponse(int fleetId, int errorCode = 0)
        {
            FleetId = fleetId;
            ErrorCode = errorCode;
        }
    }
}