public class BootStatePresenter : IStatePresenter {
    private readonly IMenuCommand _menuCommand;

    public BootStatePresenter(CommandFactory factory) {
        _menuCommand = factory.CreateRoute<MenuScreenState>();
    }
    
    public void EnterState(ScreenState state) {
        Delay.Execute(2f, _menuCommand.Execute);
    }
    
    public void ExitState(ScreenState state) {}
}
