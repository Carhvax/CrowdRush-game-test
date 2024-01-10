public interface IAgentEventsHandler {
    void ApplyDamage(MapAgent mapAgent, int damage);

    MapAgent NearestTarget(MapAgent agent);
}
