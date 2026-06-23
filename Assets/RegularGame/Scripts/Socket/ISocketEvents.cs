using System;

namespace DroneAssembly.Socket
{
    public interface ISocketEvents
    {
        event Action OnPartSnapped;
        event Action OnPartExited;
        event Action OnValidPartEntered;
    }
}