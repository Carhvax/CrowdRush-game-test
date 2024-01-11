public class PausePresenter : IStatePresenter {
    private readonly GameFlow _flow;

    public PausePresenter(GameFlow flow) => _flow = flow;

    public void EnterState(ScreenState state) => _flow.Pause(true);

    public void ExitState(ScreenState state) => _flow.Pause(false);
}