using Cysharp.Threading.Tasks;
using DroneAssembly.DataBase.Models;

public interface INetworkService
{
    void SendData(string playerName, float time);
    UniTask<LeaderboardData> FetchDataAsync();
}