using System;
using UnityEngine;

namespace Shooter.Pathfinding
{
    #pragma warning disable // Disable unnecessary warnings.
    public class Node : IComparable
    {
        public float NodeTotalCost { get; set; }
        public float EstimatedCost { get; set; }
        public bool IsObstacle { get; set; }
        public Node Parent { get; set; }
        public Vector3 Position { get; set; }

        public Node()
        {
            NodeTotalCost = 0;
            EstimatedCost = 1;
            IsObstacle = false;
            Parent = null;
        }

        public Node(Vector3 position)
        {
            NodeTotalCost = 0;
            EstimatedCost = 1;
            IsObstacle = false;
            Parent = null;
            Position = position;
        }

        public void MarkAsObstacle()
        {
            IsObstacle = true;
        }

        public int CompareTo(object obj)
        {
            Node node = obj as Node;
            if (EstimatedCost < node.EstimatedCost)
            {
                return -1;
            }

            if (EstimatedCost > node.EstimatedCost)
            {
                return 1;
            }

            return 0;
        }
    }
}