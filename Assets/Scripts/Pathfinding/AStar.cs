using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Pathfinding
{
    public static class AStar
    {
        public static PriorityQueue OpenList { get; set; }
        public static HashSet<Node> ClosedList { get; set; }
        private static Node currentPathNode;

        public static ArrayList FindPath(Node start, Node goal)
        {
            OpenList = new PriorityQueue();
            start.NodeTotalCost = 0;
            start.EstimatedCost = HeuristicEstimateCost(start, goal);
            OpenList.Push(start);

            ClosedList = new HashSet<Node>();

            while (OpenList.Length != 0)
            {
                currentPathNode = OpenList.GetFirst();
                if (currentPathNode.Position == goal.Position)
                {
                    return CalculatePath(currentPathNode);
                }

                ArrayList neighbours = new ArrayList();
                GridManager.GetInstance().GetNeighbours(currentPathNode, neighbours);
                for (int i = 0; i < neighbours.Count; i++)
                {
                    Node neighbourNode = neighbours[i] as Node;
                    if (!ClosedList.Contains(neighbourNode))
                    {
                        float cost = HeuristicEstimateCost(currentPathNode, neighbourNode);
                        float currentNodeTotalCost = currentPathNode.NodeTotalCost + cost;

                        float neighbourNodeEstimatedCost = HeuristicEstimateCost(neighbourNode, goal);
                        neighbourNode.EstimatedCost = currentNodeTotalCost + neighbourNodeEstimatedCost;
                        neighbourNode.NodeTotalCost = currentNodeTotalCost;
                        neighbourNode.Parent = currentPathNode;

                        if (!OpenList.Contains(neighbourNode))
                        {
                            OpenList.Push(neighbourNode);
                        }
                    }
                }

                ClosedList.Add(currentPathNode);
                OpenList.Remove(currentPathNode);
            }

            if (currentPathNode.Position != goal.Position)
            {
                Debug.LogError("Goal not found, returning potentially empty list...");
                return CalculatePath(currentPathNode);
            }

            return CalculatePath(currentPathNode);
        }

        private static ArrayList CalculatePath(Node node)
        {
            ArrayList pathList = new ArrayList();
            while (node != null)
            {
                pathList.Add(node);
                node = node.Parent;
            }

            pathList.Reverse();
            return pathList;
        }

        private static float HeuristicEstimateCost(Node currentNode, Node goalNode)
        {
            //return (currentNode.Position - goalNode.Position).magnitude;
            float distanceX = Mathf.Abs(currentNode.Position.x - goalNode.Position.x);
            float distanceY = Mathf.Abs(currentNode.Position.z - goalNode.Position.z);
            return 1 * (distanceX * distanceY);
        }
    }
}