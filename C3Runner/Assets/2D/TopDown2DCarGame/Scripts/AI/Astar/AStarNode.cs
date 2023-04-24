using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    //The position on the grid
    public Vector2Int gridPosition;

    //List of the nodes neighbours
    public List<AStarNode> neighbours = new List<AStarNode>();

    //Is the node an obstacle
    public bool isObstacle = false;

    //Distance from start point to node
    public int gCostDistanceFromStart = 0;

    //Distance from node to goal
    public int hCostDistanceFromGoal = 0;

    //The total cost of movement to the grid position
    public int fCostTotal = 0;

    //The order in which it was picked
    public int pickedOrder = 0;

    //State to check if the cost has already been calculated
    bool isCostCalculated = false;

    //Constructor
    public AStarNode(Vector2Int gridPosition_)
    {
        gridPosition = gridPosition_;
    }

    public void CalculateCostsForNode(Vector2Int aiPosition, Vector2Int aiDestination)
    {
        //If we have already calculated the cost then we do not need to do it again.
        if (isCostCalculated)
            return;

        gCostDistanceFromStart = Mathf.Abs(gridPosition.x - aiPosition.x) + Mathf.Abs(gridPosition.y - aiPosition.y);

        hCostDistanceFromGoal = Mathf.Abs(gridPosition.x - aiDestination.x) + Mathf.Abs(gridPosition.y - aiDestination.y);

        fCostTotal = gCostDistanceFromStart + hCostDistanceFromGoal;

        isCostCalculated = true;
    }

    public void Reset()
    {
        isCostCalculated = false;
        pickedOrder = 0;
        gCostDistanceFromStart = 0;
        hCostDistanceFromGoal = 0;
        fCostTotal = 0;

    }
}
