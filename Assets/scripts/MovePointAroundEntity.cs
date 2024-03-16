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
                SetNewPointPosition(directionCoordinates[2*(index+1)].x, directionCoordinates[2*(index+1)].y, points[index]);
            }
            else{
                // Left
                SetNewPointPosition(directionCoordinates[6*(index+1)].x, directionCoordinates[6*(index+1)].y, points[index]);
            }
        }
        else
        {
            if (lastY > 0)
            {
                if (lastX > 0){
                    // Top Right
                    SetNewPointPosition(directionCoordinates[1*(index+1)].x, directionCoordinates[1*(index+1)].y, points[index]);
                }
                else if (lastX < 0){
                    // Top Left
                    SetNewPointPosition(directionCoordinates[7*(index+1)].x, directionCoordinates[7*(index+1)].y, points[index]);
                }
                else{
                    // Top
                    SetNewPointPosition(directionCoordinates[index == 0 ? 0 : (8*index)].x, directionCoordinates[index == 0 ? 0 : (8*index)].y, points[index]);
                }
            }
            else if (lastY < 0)
            {
                if (lastX > 0){
                    // Bottom Right
                    SetNewPointPosition(directionCoordinates[3*(index+1)].x, directionCoordinates[3*(index+1)].y, points[index]);
                }
                else if (lastX < 0){
                    // Bottom Left
                    SetNewPointPosition(directionCoordinates[5*(index+1)].x, directionCoordinates[5*(index+1)].y, points[index]);
                }
                else{
                    // Bottom
                    SetNewPointPosition(directionCoordinates[4*(index+1)].x, directionCoordinates[4*(index+1)].y, points[index]);
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
