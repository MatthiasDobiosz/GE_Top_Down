using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right,
    Top,
    Bottom,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
public class MovePointAroundEntity : MonoBehaviour
{
    public Transform point;
    public Transform entity;
    public Direction initialAttackDirection;

    private Vector2 initialAttackDirectionValue = Vector2.zero;
    private Dictionary<(Direction, Direction), Vector2> directionMappings;
    
    void Start()
    {
        directionMappings = new Dictionary<(Direction, Direction), Vector2>();
        if(initialAttackDirectionValue == Vector2.zero){
            initialAttackDirectionValue = entity.InverseTransformPoint(point.position);
            FillDirectionMappings();
        }
    }

    public void MovePoint(float lastX, float lastY)
    {
        double angleInDegrees = Math.Atan2(lastY, lastX) * (180 / Math.PI);

        if (angleInDegrees < 0)
            angleInDegrees += 360;
        
        if (IsInArea(angleInDegrees, 22.5, 67.5))
            // Top Right
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Top));   
        else if (IsInArea(angleInDegrees, 67.5, 112.5))
            // Top
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Top));
        else if (IsInArea(angleInDegrees, 112.5, 157.5))
            // Top Left
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Top));
        else if (IsInArea(angleInDegrees, 157.5, 202.5))
            // Left
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Left));
        else if (IsInArea(angleInDegrees, 202.5, 247.5))
            // Bottom Left
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Bottom));
        else if (IsInArea(angleInDegrees, 247.5, 292.5))
            // Bottom
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Bottom));
        else if (IsInArea(angleInDegrees, 292.5, 337.5))
            // Bottom Right
            point.position = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Bottom));
        else 
        {
            //right
            point.localPosition = directionMappings.GetValueOrDefault((initialAttackDirection, Direction.Right));
        }    
    }

    bool IsInArea(double val, double min, double max)
    {
        return val >= min && val < max;
    }

    void FillDirectionMappings()
    {
        directionMappings[(Direction.Left, Direction.Top)] = new Vector2(initialAttackDirectionValue.y, -initialAttackDirectionValue.x);
        directionMappings[(Direction.Left, Direction.Bottom)] = new Vector2(initialAttackDirectionValue.y, initialAttackDirectionValue.x);
        directionMappings[(Direction.Left, Direction.Right)] = new Vector2(-initialAttackDirectionValue.x, initialAttackDirectionValue.y);
        directionMappings[(Direction.Left, Direction.Left)] = new Vector2(initialAttackDirectionValue.y, initialAttackDirectionValue.x);
    }
}
