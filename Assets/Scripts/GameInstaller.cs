using Interfaces;
using MCTS;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField] private GameBoardManager _gameBoardManager;
    [SerializeField] private GameBoardCellsContainer _gameBoardCellsContainer;

    public override void InstallBindings()
    {
        Container.Bind<IGameState>().To<GameState>().AsSingle();
        Container.Bind<IGameBoardManager>().FromInstance(_gameBoardManager);
        Container.Bind<IGameBoardCellsContainer>().FromInstance(_gameBoardCellsContainer);
        Container.Bind<MctsAlgorithm>().AsTransient()
                 .WithArguments(2000, true); // maxIterations, useParallel
    }
}
