using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointAroundEntity : MonoBehaviour
{
    public Transform[] points;
    public Transform entity;
    public Vector2[] directionCoordinates;

    public void MovePoint(float lastX, float lastY, int index)
    {        
        double angle = Math.Atan2(lastY, lastX) * (180 / Math.PI);

        if(angle < 0)
        {
            angle += 360;
        }

        if (IsInArea(angle, 22.5, 67.5))
        {
            // Top Right
            SetNewPointPosition(directionCoordinates[(8*index)+1].x, directionCoordinates[(8*index)+1].y, points[index]);
            return;
        }
        if (IsInArea(angle, 67.5, 112.5))
        {
            // Top
            SetNewPointPosition(directionCoordinates[8*index].x, directionCoordinates[8*index].y, points[index]);
            return;
        }
        if (IsInArea(angle, 112.5, 157.5))
        {
            // Top Left
            SetNewPointPosition(directionCoordinates[(8*index)+7].x, directionCoordinates[(8*index)+7].y, points[index]);
            return;
        }
        if (IsInArea(angle, 157.5, 202.5))
        {
            // Left
            SetNewPointPosition(directionCoordinates[(8*index)+6].x, directionCoordinates[(8*index)+6].y, points[index]);
            return;
        }
        if (IsInArea(angle, 202.5, 247.5))
        {
            // Bottom Left
            SetNewPointPosition(directionCoordinates[(8*index)+5].x, directionCoordinates[(8*index)+5].y, points[index]);
            return;
        }
        if (IsInArea(angle, 247.5, 292.5))
        {
            // Bottom
            SetNewPointPosition(directionCoordinates[(8*index)+4].x, directionCoordinates[(8*index)+4].y, points[index]);
            return;
        }
        if (IsInArea(angle, 292.5, 337.5))
        {
            // Bottom Right
            SetNewPointPosition(directionCoordinates[(8*index)+3].x, directionCoordinates[(8*index)+3].y, points[index]);
            return;
        }

        // Right
        SetNewPointPosition(directionCoordinates[(8*index)+2].x, directionCoordinates[(8*index)+2].y, points[index]);
        return;


    }

    bool IsInArea(double val, double min, double max)
    {
        return val >= min && val < max;
    }

    void SetNewPointPosition(float x, float y, Transform point)
    {
        Vector2 newPosition = Vector2.zero;
        newPosition.x = x;
        newPosition.y = y;
        point.localPosition = newPosition;
    }
}
