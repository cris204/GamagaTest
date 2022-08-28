using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum WallState
{
    // 0000 -> No Walls
    // 1111 -> left, right, up, down
    LEFT  = 1,  // 0001
    RIGHT = 2,  // 0010
    UP    = 4,  // 0100
    DOWN  = 8,  // 1000

    VISITED = 128 // 1000 0000    
}

public struct Position
{
    public int x;
    public int y;
}

public struct Neighbour
{
    public Position position;
    public WallState shareWall;
}

public static class MazeGenerator
{

    public static WallState GetOppositeWall(WallState wall)
    {
        switch (wall) {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT:  return WallState.RIGHT;
            case WallState.UP:    return WallState.DOWN;
            case WallState.DOWN:  return WallState.UP;

            default: return WallState.LEFT;
        }
    }

    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        var rng = new System.Random();
        var positionStack = new Stack<Position>();
        var position = new Position { x = rng.Next(0, width), y = rng.Next(0, height) };

        maze[position.x, position.y] |= WallState.VISITED; // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0) {

            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if(neighbours.Count > 0) {
                positionStack.Push(current);

                int randomIndex = rng.Next(0, neighbours.Count);
                Neighbour randomNeighbour = neighbours[randomIndex];

                Position nPosition = randomNeighbour.position;
                maze[current.x, current.y] &= ~randomNeighbour.shareWall;

                maze[nPosition.x, nPosition.y] &= ~GetOppositeWall(randomNeighbour.shareWall);
                maze[nPosition.x, nPosition.y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }

        }

        return maze;
    }

    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        List<Neighbour> neighbourList = new List<Neighbour>();

        //Left
        if (p.x > 0) { 
            if(!maze[p.x-1, p.y].HasFlag(WallState.VISITED)) {
                neighbourList.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x - 1,
                        y = p.y
                    },
                    shareWall = WallState.LEFT
                });
            }
        }

        //Down
        if (p.y > 0) {
            if (!maze[p.x, p.y -1].HasFlag(WallState.VISITED)) {
                neighbourList.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x,
                        y = p.y - 1
                    },
                    shareWall = WallState.DOWN
                });
            }
        }

        //up
        if (p.y < height - 1) {
            if (!maze[p.x, p.y + 1].HasFlag(WallState.VISITED)) {
                neighbourList.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x ,
                        y = p.y + 1
                    },
                    shareWall = WallState.UP
                });
            }
        }

        //Righ
        if (p.x < width - 1) {
            if (!maze[p.x + 1, p.y].HasFlag(WallState.VISITED)) {
                neighbourList.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x + 1,
                        y = p.y
                    },
                    shareWall = WallState.RIGHT
                });
            }
        }

        return neighbourList;
    }

    public static NodeData[,] nodesData;

    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initialState = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

        nodesData = new NodeData[width, height];

        for (int i = 0; i < width; ++i) { //Cambiar por i++
            
            for (int j = 0; j < height; j++) {
                maze[i, j] = initialState; //1111
                nodesData[i, j] = new NodeData(); //1111
                nodesData[i, j].Index = new Vector2(i, j);

                if(j > 0) {
                    nodesData[i, j].AddNeighbour(nodesData[i, j - 1], WallState.DOWN);
                }

                if (i > 0) {
                    nodesData[i, j].AddNeighbour(nodesData[i - 1, j], WallState.LEFT);
                }

            }

        }


        return ApplyRecursiveBacktracker(maze, width, height);
    }
}
