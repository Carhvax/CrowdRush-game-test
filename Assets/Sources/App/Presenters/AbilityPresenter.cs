public class AbilityPresenter : IStatePresenter {

    private readonly GameFlow _flow;
    private readonly IAbility[] _abilities;
    private readonly IMenuCommand _confirmCommand;

    public AbilityPresenter(CommandFactory factory, GameFlow flow, IAbility[] abilities) {
        _flow = flow;
        _abilities = abilities;
        _confirmCommand = factory.CreateRoute<GameScreenState>();
    }
    
    public void EnterState(ScreenState state) {
        _flow.Pause(true);
        
        if (state.OnResolvePresenterView<AbilitiesPresenterView>(out var view)) {
            view.RequestAbilities((slots) => {
                var abilities = _abilities.GetRandom(slots.Length).ToQueue();

                slots.Each(s => {
                    var ability = abilities.Dequeue();
                    ability.ApplyPanel(s);
                    s.AddListener(() => {
                        _flow.ApplyAbility(ability);
                        _confirmCommand.Execute();
                    });
                });
            });
        }    
    }
    
    public void ExitState(ScreenState state) {
        _flow.Pause(false);
    }
}
