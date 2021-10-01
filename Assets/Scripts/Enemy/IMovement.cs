using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    Vector3 Move(Vector3 position, Vector3 up, float movementSpeed);
}

public class NormalMove : IMovement
{
    public Vector3 Move(Vector3 position, Vector3 up, float movementSpeed)
    {
        float posY = movementSpeed * Time.deltaTime * up.y;
        Vector3 newPosition = new Vector3(0, posY);

        return newPosition;
    }
}

public class SineMove : IMovement
{
    public Dictionary<string, float> sineParam;
    

    public Vector3 Move(Vector3 position, Vector3 up, float movementSpeed)
    {
        float sin = MathWave.CalculateSine(position.y, sineParam["frequency"], sineParam["offset"], sineParam["amplitude"], sineParam["inverted"]);
        
        float posX = movementSpeed * Time.deltaTime * sin;
        float posY = movementSpeed * Time.deltaTime * up.y;
        Vector3 newPosition = new Vector3(posX, posY);

        return newPosition;
    }
}
