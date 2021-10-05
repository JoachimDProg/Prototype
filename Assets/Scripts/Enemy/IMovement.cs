using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    Vector3 Move(Vector3 position, Vector3 initialUp, Vector3 currentUp, float movementSpeed);
}

public class NormalMove : IMovement
{
    public Vector3 Move(Vector3 position, Vector3 initialUp, Vector3 currentUp, float movementSpeed)
    {
        float posY = movementSpeed * Time.deltaTime * -1;
        Vector3 newPosition = new Vector3(0, posY);

        return newPosition;
    }
}

public class SeekMove : IMovement
{
    public Vector3 Move(Vector3 position, Vector3 initialUp, Vector3 currentUp, float movementSpeed)
    {
        float posX = movementSpeed * Time.deltaTime * currentUp.x;
        float posY = movementSpeed * Time.deltaTime * currentUp.y;
        Vector3 newPosition = new Vector3(posX, posY);

        return newPosition;
    }
}

public class SineMove : IMovement
{
    public Dictionary<string, float> sineParam;

    public Vector3 Move(Vector3 position, Vector3 initialUp, Vector3 currentUp, float movementSpeed)
    {
        float sin = MathWave.CalculateSine(position.y, sineParam["frequency"], sineParam["offset"], sineParam["amplitude"], sineParam["inverted"]);
        
        float posX = movementSpeed * Time.deltaTime * sin;
        float posY = movementSpeed * Time.deltaTime * initialUp.y;
        Vector3 newPosition = new Vector3(posX, posY);

        return newPosition;
    }
}
