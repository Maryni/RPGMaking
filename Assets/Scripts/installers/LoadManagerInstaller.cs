using UnityEngine;
using Zenject;

public class LoadManagerInstaller : MonoInstaller
{
    [SerializeField] private LoadManager loadManager;
    public override void InstallBindings()
    {
        Container.BindInstance(loadManager);
    }
}