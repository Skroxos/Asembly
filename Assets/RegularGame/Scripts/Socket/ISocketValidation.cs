namespace DroneAssembly.Socket
{
    public interface ISocketValidation
    {
        bool IsOccupied();
        bool IsPartValid(BaseAssemblyPart part);
    }
}