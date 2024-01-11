public class UnloadingPresenter : IStatePresenter {
    private readonly GameFlow _flow;
    private readonly IMenuCommand _continueCommand;

    public UnloadingPresenter(CommandFactory factory, GameFlow flow) {
        _flow = flow;
        _continueCommand = factory.CreateRoute<MenuScreenState>();
    }
    
    public void EnterState(ScreenState state) {
        _flow.Dispose();
        _continueCommand.Execute();
    }
    
    public void ExitState(ScreenState state) {}
}
