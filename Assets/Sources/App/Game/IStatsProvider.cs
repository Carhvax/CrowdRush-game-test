public interface IStatsProvider {
    IObservableValue<float> PlayerHealth { get; }
    IObservableValue<float> ConsoleHealth { get; }
    IObservableValue<int> MobsCount { get; }
    IObservableValue<int> Level { get; }
}
