using System.Collections;

namespace Shooter.Pathfinding
{
    public class PriorityQueue
    {
        private ArrayList nodes = new ArrayList();

        public int Length
        {
            get { return nodes.Count; }
        }

        public bool Contains(object node)
        {
            return nodes.Contains(node);
        }

        public Node GetFirst()
        {
            if (nodes.Count > 0)
            {
                return nodes[0] as Node;
            }

            return null;
        }

        public void Push(Node node)
        {
            nodes.Add(node);
            nodes.Sort();
        }

        public void Remove(Node node)
        {
            nodes.Remove(node);
            nodes.Sort();
        }
    }
}