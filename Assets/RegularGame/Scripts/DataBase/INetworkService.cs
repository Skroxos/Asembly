using Cysharp.Threading.Tasks;
using DroneAssembly.DataBase.Models;

public interface INetworkService
{
    UniTask<bool> SendPlayerDataAsync(string playerName, float time);
    UniTask<(bool isSuccess, PlayerScore[] data)> FetchDataAsync();
}