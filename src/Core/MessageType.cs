namespace Galcon.Server.Core
{
    public enum MessageType
    {
        ConnectionResponse = 0,
        StartGame,
        StartGameResponse,
        TickUpdate,
        SendFleet,
        SendFleetResponse,
        EndGame
    }
}