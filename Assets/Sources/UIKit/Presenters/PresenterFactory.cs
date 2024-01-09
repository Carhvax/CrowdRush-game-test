using Zenject;

public interface IStatePresenter : IStateObserver {
    
}

public class PresenterFactory {
    private readonly DiContainer _container;
    
    public PresenterFactory(DiContainer container) {
        _container = container;
    }

    public IStateObserver CreatePresenter<TPresenter>() where TPresenter : IStatePresenter {
        return _container.Resolve<TPresenter>();
    }
}

public static class PresentersExtensions {

    public static void AddPresenter<TPresenter>(this PresenterFactory factory, IStateMap map) where TPresenter : IStatePresenter{
        map.AddObserver(factory.CreatePresenter<TPresenter>());
    }
    
}
