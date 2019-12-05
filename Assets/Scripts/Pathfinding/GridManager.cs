using Shooter.Utility;
using System.Collections;
using UnityEngine;

namespace Shooter.Pathfinding
{
    public class GridManager : GenericSingleton<GridManager>
    {
        [SerializeField]
        private int rows = 67;
        [SerializeField]
        private int columns = 67;
        [SerializeField]
        private float gridCellSize = 2;
        [SerializeField]
        private bool showGrid = true;
        [SerializeField]
        private bool showObstacleBlocks = true;

        private readonly int nodeColumnDimension = 0;
        private readonly int nodeRowDimension = 1;

        private Vector3 origin = new Vector3();
        public Vector3 GetOrigin
        {
            get { return origin; }
        }

        private GameObject[] obstacleList;

        // Reminder for two dimensional node array:
        // Column right to left.
        // Row bottom to top.
        private Node[,] nodes;
        public Node[,] GetNodes()
        {
            return nodes;
        }
        public void SetNodes(Node node, int column, int row)
        {
            nodes[column, row] = node;
        }

        protected override void Awake()
        {
            base.Awake();
            origin = transform.position;
            obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
            CalculateObstacles();
        }

        private void CalculateObstacles()
        {

            nodes = new Node[columns, rows];
            int index = 0;
            for (int column = 0; column < nodes.GetLength(nodeColumnDimension); column++)
            {
                for (int row = 0; row < nodes.GetLength(nodeRowDimension); row++)
                {
                    Vector3 cellPos = GetGridCellCenter(index);
                    Node node = new Node(cellPos);
                    nodes[column, row] = node;
                    index++;
                }
            }

            MarkObstacles();
        }

        private void MarkObstacles()
        {
            if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach (GameObject obstacle in obstacleList)
                {
                    int indexCell = GetGridIndex(obstacle.transform.position);
                    if (indexCell > 0)
                    {
                        Debug.Log(obstacle);
                        int column = GetColumn(indexCell);
                        int row = GetRow(indexCell);
                        nodes[column, row].MarkAsObstacle();
                    }
                }
            }
        }

        public Vector3 GetGridCellCenter(int index)
        {
            Vector3 cellPosition = GetGridCellPosition(index);
            cellPosition.x += gridCellSize / 2.0f;
            cellPosition.z += gridCellSize / 2.0f;
            return cellPosition;
        }

        public Vector3 GetGridCellPosition(int index)
        {
            int row = GetRow(index);
            int column = GetColumn(index);
            float xPosInGrid = column * gridCellSize;
            float zPosInGrid = row * gridCellSize;
            return origin + new Vector3(xPosInGrid, 0, zPosInGrid);
        }

        public int GetGridIndex(Vector3 position)
        {
            if (!IsInBounds(position))
            {
                return -1;
            }

            position -= origin;
            int column = (int)(position.x / gridCellSize);
            int row = (int)(position.z / gridCellSize);
            return row * columns + column;
        }

        public bool IsInBounds(Vector3 position)
        {
            float width = columns * gridCellSize;
            float height = rows * gridCellSize;
            return position.x >= origin.x
                && position.x <= origin.x + width
                && position.x <= origin.z + height
                && position.z >= origin.z;
        }

        public int GetColumn(int index)
        {
            return index % columns;
        }

        public int GetRow(int index)
        {
            return index / columns;
        }

        public void GetNeighbours(Node node, ArrayList neighbours)
        {
            Vector3 neighbourPos = node.Position;
            int neighbourIndex = GetGridIndex(neighbourPos);

            int currentRow = GetRow(neighbourIndex);
            int currentColumn = GetColumn(neighbourIndex);

            //Bottom
            int leftNodeRow = currentRow - 1;
            int leftNodeColumn = currentColumn;
            AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);

            //Top
            leftNodeRow = currentRow + 1;
            leftNodeColumn = currentColumn;
            AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);

            //Right
            leftNodeRow = currentRow;
            leftNodeColumn = currentColumn + 1;
            AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);

            //Left 
            leftNodeRow = currentRow;
            leftNodeColumn = currentColumn - 1;
            AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);
        }

        private void AssignNeighbour(int row, int column, ArrayList neighbours)
        {
            if (row != -1 && column != -1 && row < rows && column < columns)
            {
                Node nodeToAdd = nodes[row, column];
                if (!nodeToAdd.IsObstacle)
                {
                    neighbours.Add(nodeToAdd);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (showGrid)
            {
                DebugDrawGrid(transform.position, rows, columns, gridCellSize, Color.blue);
            }

            Gizmos.DrawSphere(transform.position, 0.5f);
            if (showObstacleBlocks)
            {
                Vector3 cellSize = new Vector3(gridCellSize, 1, gridCellSize);
                if (obstacleList != null && obstacleList.Length > 0)
                {
                    foreach (GameObject obstacle in obstacleList)
                    {
                        Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(obstacle.transform.position)), cellSize);
                    }
                }
            }
        }

        private void DebugDrawGrid(Vector3 origin, int rows, int columns, float cellSize, Color color)
        {
            float width = columns * cellSize;
            float height = rows * cellSize;

            for (int i = 0; i < columns + 1; i++)
            {
                Vector3 startPosition = origin + i * cellSize * new Vector3(0, 0, 1);
                Vector3 endPosition = startPosition + width * new Vector3(1, 0, 0);
                Debug.DrawLine(startPosition, endPosition, color);
            }

            for (int i = 0; i < rows + 1; i++)
            {
                Vector3 startPosition = origin + i * cellSize * new Vector3(1, 0, 0);
                Vector3 endPosition = startPosition + height * new Vector3(0, 0, 1);
                Debug.DrawLine(startPosition, endPosition, color);
            }
        }
#endif
    }
}