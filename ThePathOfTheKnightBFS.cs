using System.Collections.Generic;
using System;
using ThePathofKnight;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Spatial;

public class ThePathOfTheKnightBFS : IThePathOfTheKnight
{
    // Chess board size
    int N = 8;

    // Possible moves for a knight
    int[] knightMovesX = { 2, 1, -1, -2, -2, -1, 1, 2 };
    int[] knightMovesY = { 1, 2, 2, 1, -1, -2, -2, -1 };    

    private Position Parent { get; set; }

    private bool IsInside(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < N && y < N);
    }

    //Implementing BreadthFirstSearch
    private async Task<List<KnightPosition>> FindShortestPath(KnightPosition start, KnightPosition end)
    {
        bool[,] visited = new bool[N, N];
        
        Queue<KnightPosition> queue = new Queue<KnightPosition>();

        queue.Enqueue(start);
        visited[start.X, start.Y] = true;
        
        while (queue.Count > 0)
        {
            KnightPosition current = queue.Dequeue();

            if (current.X == end.X && current.Y == end.Y)
            {
                List<KnightPosition> path = new List<KnightPosition>();

                while(current != null)
                {
                    if(current.X != start.X && current.Y != start.Y )
                    {
                        path.Add(current);
                    }                    
                    current = current.Parent;
                }

                path.Reverse();
                return path;
            }

            // Iterate over all possible knight moves
            for (int i = 0; i < N; i++)
            {
                int nextX = current.X + knightMovesX[i];
                int nextY = current.Y + knightMovesY[i];

                if (IsInside(nextX, nextY) && !visited[nextX, nextY])
                {
                    visited[nextX, nextY] = true;
                    queue.Enqueue(new KnightPosition(nextX, nextY, current.Distance + 1, current));
                }
            }
        }        
        return new List<KnightPosition>();
    }

    public async Task<TheKnightPathRecord> FindShortest((int, int) start, (int, int) end)
    {        
        var shortestPath = await FindShortestPath(new KnightPosition(start.Item1, start.Item2), new KnightPosition(end.Item1,end.Item2));

        string totalPath = string.Empty;
        foreach (var path in shortestPath)
        {
            if (string.IsNullOrEmpty(totalPath))
            {
                totalPath = $"{path.X.ToString()},{path.Y.ToString()}";
            }
            else
            {
                totalPath += $" {path.X.ToString()},{path.Y.ToString()}";
            }            
        }

        TheKnightPathRecord response = new TheKnightPathRecord();
        response.ShortestPath = totalPath;
        response.NumberOfMoves = shortestPath.Count;
        response.MinimumRetentionDate = DateTime.Now.AddYears(10);
        response.CreatedOn = DateTime.Now;
        response.OperationId = Guid.NewGuid().ToString();
        response.Id = response.OperationId;
        response.Starting = start.ToString();
        response.Ending = end.ToString();
        response.IsDeleted = false;

        if (shortestPath.Count < 1)
        {
            response.IsFound = false;
            return response;
        }

        response.IsFound = true;

        return response;
    }
}