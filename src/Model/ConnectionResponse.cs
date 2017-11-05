using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalconServer.Model
{
    public class ConnectionResponse
    {

        public int PlayerId { get; private set; }

        public ConnectionResponse(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
