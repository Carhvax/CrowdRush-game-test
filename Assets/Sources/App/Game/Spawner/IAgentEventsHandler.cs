public interface IAgentEventsHandler {
    void InitHandler();
    void ApplyDamage(MapAgent mapAgent, int damage);

    MapAgent NearestTarget(MapAgent agent);

    void Tick();
    void DisposeHandler();
}
