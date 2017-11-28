namespace GalconServer.Model
{
    public enum MessageType
    {
        ConnectionResponse = 0,
        StartGame,
        StartGameResponse,
        TickUpdate,
        SendFleet,
        SendFleetResponse,
        EndGame,
        ErrorResponse
    }
}