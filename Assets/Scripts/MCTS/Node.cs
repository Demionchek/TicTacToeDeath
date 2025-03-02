using System;
using System.Collections.Generic;

namespace MCTS
{
    public class Node
    {
        public GameState State { get; }
        public Node Parent { get; }
        public List<Node> Children { get; } = new List<Node>();
        public int Visits { get; private set; } = 0;
        public double Wins { get; private set; } = 0;

        public Node(GameState state, Node parent = null)
        {
            State = state;
            Parent = parent;
        }

        // Выбор ребенка с максимальным UCB1
        public Node SelectChild()
        {
            Node bestChild = null;
            double bestValue = double.MinValue;
            foreach (var child in Children)
            {
                double ucb = child.Wins / child.Visits + Math.Sqrt(2 * Math.Log(Visits) / child.Visits);
                if (ucb > bestValue)
                {
                    bestValue = ucb;
                    bestChild = child;
                }
            }
            return bestChild;
        }

        // Обновление статистики
        public void Update(double result)
        {
            Visits++;
            Wins += result;
        }
    }
}