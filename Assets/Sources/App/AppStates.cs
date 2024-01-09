using ScreenStates;

public class AppStates : StateMachine<ScreenState> {

    public AppStates(ScreenState[] states) {
        states.Each(s => {
            s.Init();
            AddState(s);
        });
    }

}
