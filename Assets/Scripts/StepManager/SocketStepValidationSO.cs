using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Step Validation/Socket Step Validation")]
public class SocketStepValidationSO : ScriptableObject
{
    private HashSet<SocketIDSO> _currentlyAllowedSockets = new HashSet<SocketIDSO>();
    
    public void UpdateAllowedSockets(List<SocketIDSO> allowedSockets)
    {
        _currentlyAllowedSockets.Clear();
        foreach (var socket in allowedSockets)
        {
            _currentlyAllowedSockets.Add(socket);
        }
    }
    
    public bool IsSocketAllowed(SocketIDSO socketID)
    {
        if (_currentlyAllowedSockets.Count == 0)
        {
            return true; // can return true or false depends on design - if true then all sockets are allowed , if false then no sockets are allowed
        }
        
        return _currentlyAllowedSockets.Contains(socketID);
    }
    
}