using System.Collections.Generic;
using DroneAssembly.Socket;
using DroneAssembly.Validator;
using NUnit.Framework;
using UnityEngine;

public class SocketStepValidationTest
{
    private SocketStepValidationSO _socketStepValidationSO;
    private SocketIDSO _propellerID;
    private SocketIDSO _bellID;
    private SocketIDSO _mountingPlateID;

    [SetUp]
    public void Setup()
    {
        _socketStepValidationSO = ScriptableObject.CreateInstance<SocketStepValidationSO>();
        _propellerID = ScriptableObject.CreateInstance<SocketIDSO>();
        _propellerID.name = "Propeller";
        _bellID = ScriptableObject.CreateInstance<SocketIDSO>();
        _bellID.name = "Bell";
        _mountingPlateID = ScriptableObject.CreateInstance<SocketIDSO>();
        _mountingPlateID.name = "Mounting Plate";
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_socketStepValidationSO);
        Object.DestroyImmediate(_propellerID);
        Object.DestroyImmediate(_bellID);
        Object.DestroyImmediate(_mountingPlateID);
    }

    [Test]
    public void IsSocketAllowed_WhenNoSocketsSet_ReturnsTrue()
    {
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_propellerID));
    }

    [Test]
    public void IsSocketAllowed_WhenSocketIsInAllowedList_ReturnsTrue()
    {
        _socketStepValidationSO.UpdateAllowedSockets(new List<SocketIDSO> { _propellerID, _bellID });
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_propellerID));
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_bellID));
    }

    [Test]
    public void IsSocketAllowed_WhenSocketIsNotInAllowedList_ReturnsFalse()
    {
        _socketStepValidationSO.UpdateAllowedSockets(new List<SocketIDSO> { _propellerID, _bellID });
        Assert.IsFalse(_socketStepValidationSO.IsSocketAllowed(_mountingPlateID));
    }

    [Test]
    public void UpdateAllowedSockets_ClearsPreviousSockets()
    {
        _socketStepValidationSO.UpdateAllowedSockets(new List<SocketIDSO> { _propellerID, _bellID });
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_propellerID));
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_bellID));

        _socketStepValidationSO.UpdateAllowedSockets(new List<SocketIDSO> { _mountingPlateID });
        Assert.IsFalse(_socketStepValidationSO.IsSocketAllowed(_propellerID));
        Assert.IsFalse(_socketStepValidationSO.IsSocketAllowed(_bellID));
        Assert.IsTrue(_socketStepValidationSO.IsSocketAllowed(_mountingPlateID));
    }
    
}
