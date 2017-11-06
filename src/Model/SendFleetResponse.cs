namespace GalconServer.Model
{
    public class SendFleetResponse
    {
        public int FleetId { get; private set; }
        public ErrorCodes ErrorCode { get; private set; }

        public SendFleetResponse(int fleetId, ErrorCodes errorCode = 0)
        {
            FleetId = fleetId;
            ErrorCode = errorCode;
        }
    }
}