using System;
using System.Linq;
using System.Threading.Tasks;

namespace MCTS
{
    public class MctsAlgorithm
    {
        private Node _root;
        private int _maxIterations;
        private bool _useParallel;
        private Random _random = new Random();

        public MctsAlgorithm(GameState initialState, int maxIterations, bool useParallel = true)
        {
            _root = new Node(initialState);
            _maxIterations = maxIterations;
            _useParallel = useParallel;
        }

        // Основной метод для поиска лучшего хода
        public Move FindBestMove()
        {
            if (_useParallel)
            {
                Parallel.For(0, _maxIterations, i => RunIteration());
            }
            else
            {
                for (int i = 0; i < _maxIterations; i++) RunIteration();
            }

            // Выбор наиболее посещаемого ребенка
            var bestChild = _root.Children.OrderByDescending(c => c.Visits).First();
            return bestChild.State.GetLastMove();
        }

        // Одна итерация MCTS (Selection, Expansion, Simulation, Backpropagation)
        private void RunIteration()
        {
            Node node = _root;
            GameState state = node.State.Clone();

            // 1. Selection: спускаемся по дереву, пока не найдем лист
            while (node.Children.Any() && !IsTerminal(state))
            {
                node = node.SelectChild();
                state = node.State.Clone();
            }

            // 2. Expansion: если узел не терминальный, добавляем детей
            if (!IsTerminal(state))
            {
                var legalMoves = state.GetLegalMoves();
                foreach (var move in legalMoves)
                {
                    var newState = state.Clone();
                    newState.MakeMove(move);
                    node.Children.Add(new Node(newState, node));
                }
                // Выбираем случайного ребенка для симуляции
                node = node.Children[_random.Next(node.Children.Count)];
                state = node.State.Clone();
            }

            // 3. Simulation: симулируем игру до конца
            double result = Simulate(state);

            // 4. Backpropagation: обновляем статистику
            while (node != null)
            {
                node.Update(result);
                node = node.Parent;
            }
        }

        // Проверка, является ли состояние терминальным (игра завершена)
        private bool IsTerminal(GameState state)
        {
            return state.GlobalWinner.HasValue || state.GetLegalMoves().Count == 0;
        }

        // Симуляция случайной игры до конца
        private double Simulate(GameState state)
        {
            while (!IsTerminal(state))
            {
                var legalMoves = state.GetLegalMoves();
                var randomMove = legalMoves[_random.Next(legalMoves.Count)];
                state.MakeMove(randomMove);
            }
            return CalculateResult(state);
        }

        // Вычисление результата симуляции
        private double CalculateResult(GameState state)
        {
            if (state.GlobalWinner.HasValue)
            {
                // Если есть победитель, возвращаем 1 (победа) или -1 (поражение)
                return state.GlobalWinner == Player.X ? 1 : -1;
            }
            else
            {
                // Если игра завершилась ничьей, возвращаем 0
                return 0;
            }
        }
    }
}

