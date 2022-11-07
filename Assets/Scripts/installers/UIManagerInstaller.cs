using UnityEngine;
using Zenject;

public class UIManagerInstaller : MonoInstaller
{
    [SerializeField] private UIManager uiManager;
    public override void InstallBindings()
    {
        Container.BindInstance(uiManager);
    }
}