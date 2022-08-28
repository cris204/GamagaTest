using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LineRenderer pathLine;
    public List<Node> path;


    public LayerMask unwalkableMask;
    public float nodeRadius;
    private Vector3 gridWorldSize;
    private Node[,] grid;
    private GameController gameController;

    private Transform playerPoint;
    private Transform finalPoint;

    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    public bool onlyDisplayPathGizmos;
    public int MaxSize {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    private void Start()
    {
        gameController = GameController.Instance;
        playerPoint = gameController.player.transform;
        finalPoint = gameController.mazeRender.finalObjectTransform;
    }

    private void Update()
    {
        if (gameController.NeedToShowPath()) {
            if (path == null) return;
            pathLine.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++) {

                pathLine.SetPosition(i, path[i].worldPosition);
            }
        }
    }

    public void SetGridSize(int x, int z)
    {
        gridWorldSize = Vector3.up;
        gridWorldSize.x = x;
        gridWorldSize.z = z;
    }


    public void GeneratePath()
    {
        nodeDiameter = (nodeRadius) * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.forward * gridWorldSize.z/2;

        for (int x = 0; x < gridSizeX; x++) {

            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if( x == 0 && y == 0) {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >=0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (onlyDisplayPathGizmos) {
            if (path != null) {
                foreach (Node n in path) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        } else {
            if (grid != null) {
                foreach (Node n in grid) {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (path != null) {
                        if (path.Contains(n)) {
                            Gizmos.color = Color.black;
                        }
                    }
                    if (n == GetNodeFromWorldPoint(finalPoint.position)) {
                        Gizmos.color = Color.green;
                    }

                    if (n == GetNodeFromWorldPoint(playerPoint.position)) {
                        Gizmos.color = Color.blue;
                    }

                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }

}
