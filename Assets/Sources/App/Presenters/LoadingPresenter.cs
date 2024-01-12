public class LoadingPresenter : IStatePresenter {
    private readonly GameFlow _flow;
    private readonly IMenuCommand _continueCommand;

    public LoadingPresenter(CommandFactory factory, GameFlow flow) {
        _flow = flow;
        _continueCommand = factory.CreateRoute<GameScreenState>();
    }
    
    public void EnterState(ScreenState state) {
        _flow.Create();
        _continueCommand.Execute();
    }
    
    public void ExitState(ScreenState state) {}
}

public class RestartPresenter : IStatePresenter {
    private readonly GameFlow _flow;
    private readonly IMenuCommand _continueCommand;

    public RestartPresenter(CommandFactory factory, GameFlow flow) {
        _flow = flow;
        _continueCommand = factory.CreateRoute<GameScreenState>();
    }
    
    public void EnterState(ScreenState state) {
        _flow.Dispose();
        _flow.Create();
        _continueCommand.Execute();
    }
    
    public void ExitState(ScreenState state) {}
}