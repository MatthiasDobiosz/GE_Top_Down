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
        if (Mathf.Abs(lastX) > Mathf.Abs(lastY))
        {
            if (lastX > 0){
                //right
                SetNewPointPosition(directionCoordinates[(8*index)+2].x, directionCoordinates[(8*index)+2].y, points[index]);
            }
            else{
                // Left
                SetNewPointPosition(directionCoordinates[(8*index)+6].x, directionCoordinates[(8*index)+6].y, points[index]);
            }
        }
        else
        {
            if (lastY > 0)
            {
                if (lastX > 0){
                    // Top Right
                    SetNewPointPosition(directionCoordinates[(8*index)+1].x, directionCoordinates[(8*index)+1].y, points[index]);
                }
                else if (lastX < 0){
                    // Top Left
                    SetNewPointPosition(directionCoordinates[(8*index)+7].x, directionCoordinates[(8*index)+7].y, points[index]);
                }
                else{
                    // Top
                    SetNewPointPosition(directionCoordinates[8*index].x, directionCoordinates[8*index].y, points[index]);
                }
            }
            else if (lastY < 0)
            {
                if (lastX > 0){
                    // Bottom Right
                    SetNewPointPosition(directionCoordinates[(8*index)+3].x, directionCoordinates[(8*index)+3].y, points[index]);
                }
                else if (lastX < 0){
                    // Bottom Left
                    SetNewPointPosition(directionCoordinates[(8*index)+5].x, directionCoordinates[(8*index)+5].y, points[index]);
                }
                else{
                    // Bottom
                    SetNewPointPosition(directionCoordinates[(8*index)+4].x, directionCoordinates[(8*index)+4].y, points[index]);
                }
            }
        }
    }

    void SetNewPointPosition(float x, float y, Transform point)
    {
        Vector2 newPosition = Vector2.zero;
        newPosition.x = x;
        newPosition.y = y;
        point.localPosition = newPosition;
    }
}
