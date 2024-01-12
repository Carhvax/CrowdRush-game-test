using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller {

    [SerializeField] private AppEntry _entryPoint;
    [SerializeField] private GameObject _handlers;
    [SerializeField] private DataAbility[] _abilities;
    
    private Type[] _domainTypes;
    
    public override void InstallBindings() {
        _domainTypes = AppDomain
            .CurrentDomain
            .GetAssembliesTypes();

        BindSources();

        BindStates();

        BindEntry();
    }
    
    private void BindEntry() {
        BindInstanceAsSingle(_entryPoint);
    }

    private void BindStates() {
        BindAsSingle<GameFlow>();
        
        BindAsSingle<AppStates>();
        BindAsSingle<StateChangeHandler>();
        BindAsSingle<CommandFactory>();
        BindPresenters();
    }

    private void BindPresenters() {
        OnType<IStatePresenter>()
            .Each(BindAsSingle);
        
        BindAsSingle<PresenterFactory>();
    }
    
    private void BindSources() {
        OnType<IAppModel>().Each(BindAsSingle);
        
        BindInstanceAsSingle(_abilities.Cast<IAbility>().ToArray());
        BindInstanceAsSingle(_handlers.GetComponents<IAgentEventsHandler>().ToArray());
        BindInstanceAsSingle(FindObjectsOfType<ScreenState>(true));
        
        Container
            .Bind<IIOService>()
            .To<EasySaveIOService>()
            .AsSingle();
    }
    
    private void BindAsSingle<T>() => Container
        .BindInterfacesAndSelfTo<T>()
        .AsSingle();
    
    private void BindAsSingle(Type type) => Container
        .BindInterfacesAndSelfTo(type)
        .AsSingle();
    
    private void BindInstanceAsSingle<T>(T instance) => Container
        .BindInterfacesAndSelfTo<T>()
        .FromInstance(instance)
        .AsSingle();
    
    private Type[] OnType<TType>() {
        var interfaceType = typeof(TType);

        return _domainTypes
            .Where(type => !type.IsAbstract && type.IsClass && interfaceType.IsAssignableFrom(type))
            .ToArray();
    }
}
