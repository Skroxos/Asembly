using DroneAssembly.DataBase;
using DroneAssembly.ScoresUI;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LeaderboardAPI>(Lifetime.Singleton).As<INetworkService>();
        builder.RegisterComponentInHierarchy<FinalUIManager>();
    }
}