public class FlowPresenter : IStatePresenter {
    
    private readonly GameFlow _flow;
    private readonly IMenuCommand _completeCommand;

    public FlowPresenter(CommandFactory factory, GameFlow flow) {
        _flow = flow;

        _completeCommand = factory.CreateRoute<CompleteScreenState>();
    }
    
    public void EnterState(ScreenState state) => _flow.Complete += OnGameCompleted;

    private void OnGameCompleted() => _completeCommand.Execute();

    public void ExitState(ScreenState state) => _flow.Complete -= OnGameCompleted;
}
