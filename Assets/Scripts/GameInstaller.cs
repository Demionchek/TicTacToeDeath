using MCTS;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Биндим GameState как синглтон
        Container.Bind<GameState>().AsSingle();

        // Биндим MCTS с параметрами
        Container.Bind<MctsAlgorithm>().AsTransient()
                 .WithArguments(2000, true); // maxIterations, useParallel

        // Биндим менеджер доски
        Container.Bind<GameBoardManager>().AsSingle();
    }
}
