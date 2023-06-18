using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameBoardGenerator gameBoardGenerator;

    public override void InstallBindings()
    {
        BindGameBoardGenerator();
    }

    private void BindGameBoardGenerator()
    {
        Container.Bind<GameBoardGenerator>().FromInstance(gameBoardGenerator);
    }
}