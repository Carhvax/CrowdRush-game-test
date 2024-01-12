public interface IAgentEventsHandler {
    void InitHandler(IStatsProvider stats);
    bool ApplyDamage(MapAgent mapAgent, int damage);

    MapAgent NearestTarget(MapAgent agent);

    void Tick();
    void DisposeHandler();
}
