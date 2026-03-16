using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.Socket;
using UnityEngine;

namespace DroneAssembly.Radios
{
    [CreateAssetMenu(menuName = "Event/SocketSnap")]
    public class EventRadio : GeneralRadio<SocketIDSO>
    {
    }
}