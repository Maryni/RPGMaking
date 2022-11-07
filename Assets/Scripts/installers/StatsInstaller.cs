using UnityEngine;
using Zenject;

public class StatsInstaller : MonoInstaller
{
    [SerializeField] private Stats stats;
    public override void InstallBindings()
    {
        Container.BindInstance(stats);
    }
}