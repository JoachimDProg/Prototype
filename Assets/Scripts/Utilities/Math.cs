using UnityEngine;

public static class Math
{
    //** VECTOR **//

    // Return the square of a value
    public static float Square(float value)
    {
        return value * value;
    }

    // Return the distance between two points
    public static float GetDistance(Vector3 point1, Vector3 point2) /// point1 = current position; point2 = objective position
    {
        /// Square root: (point1.coords - point2.coords) ^ 2 
        return Mathf.Sqrt(Square(point2.x - point1.x) + Square(point2.y - point1.y) + Square(point2.z - point1.z));
    }

    // Return the magnitude of a vector from the origin
    public static float GetMagnitude(Vector3 vector)
    {
        return GetDistance(vector, new Vector3(0, 0, 0));
    }

    // Return Vector between objects
    public static Vector3 GetVector(Vector3 point1, Vector3 point2)
    {
        Vector3 vectorBetweenPoints = new Vector3(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z);
        return vectorBetweenPoints;
    }

    public static Vector3 GetVectorExt(this Vector3 point1, Vector3 point2)
    {
        Vector3 vectorBetweenPoints = new Vector3(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z);
        return vectorBetweenPoints;
    }

    // Return the normal form of a vector
    public static Vector3 GetNormal(Vector3 vector)
    {
        /// divide each parameter by the magnitude to give normal vector
        return vector / GetMagnitude(vector);
    }

    //** ANGLE AND ROTATION **//

    // Return the dot product of two vector
    public static float GetDot(Vector3 vector1, Vector3 vector2) /// vector1 = vector up; vector2 = vector between vector1 and objective
    {
        /// dot = vector1 * vector2
        return (vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
    }

    // Return the angle between two vectors
    public static float GetAngle(Vector3 vector1, Vector3 vector2) /// vector1 = vector up; vector2 = vector between vector1 and objective
    {
        /// if cross.z < 0 (clockwise turn) we need to reverse the angle; Ex: 360 - 30 = 330
        float sign = Mathf.Sign(GetCross(vector1, vector2).z);

        /// @ = cos^-1 (dot / ( ||v1|| * ||v2|| )
        /// return radians. For degrees: * 180/PI
        return sign * Mathf.Acos(GetDot(vector1, vector2) / (GetMagnitude(vector1) * GetMagnitude(vector2)));
    }

    // Return the cross product of two vectors
    public static Vector3 GetCross(Vector3 vector1, Vector3 vector2) /// vector1 = current direction; vector2 = desired direction
    {
        return new Vector3(vector1.y * vector2.z - vector1.z * vector2.y,
                           vector1.z * vector2.x - vector1.x * vector2.z,
                           vector1.x * vector2.y - vector1.y * vector2.x);
    }

    // Return rotation position
    public static Vector3 Rotate(Vector3 vector, float angle) /// vector = player up; angle = angle in radians
    {
        /// vector.x' = x * Cos(@) - y * Sin(@)  
        /// vector.y' = x * Sin(@) + y * Cos(@)
        return new Vector3(vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle),
                           vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle),
                           0f);
    }

    // Return translation position
    public static Vector3 Translate(Vector3 position, Vector3 facing, Vector3 verticalVector) /// position = player position, facing = player up, vector = vertical movement axis
    {
        // to eliminate NaN error
        if (GetDistance(new Vector3(0, 0, 0), verticalVector) <= 0) return position;

        // get angle between world y-axis and player y-axis
        float angle = GetAngle(verticalVector, facing);

        // get angle between world y-axis and up ( return 0 for forward movement, return - 180 for backward movement)
        float worldAngle = GetAngle(verticalVector, new Vector3(0, 1, 0));

        // rotate the vertical vector to match the player up so the player moves according to his y-axis and not the world y-axis
        verticalVector = Rotate(verticalVector, angle + worldAngle);

        // add player position to vertical vector
        float x = position.x + verticalVector.x;
        float y = position.y + verticalVector.y;
        float z = position.z + verticalVector.z;

        Debug.Log("Angle: " + angle * 180 / Mathf.PI);
        Debug.Log("World Angle: " + worldAngle * 180 / Mathf.PI);

        return new Vector3(x, y, z);
    }

    // Auto Move functions

    /// <summary>
    /// Calculate direction from player position to objective position
    /// Calculate angle between player forward vector and objective vector
    /// Return rotation value
    /// </summary>
    /// <param name="forward"> player forward position </param>
    /// <param name="position"> player position </param>
    /// <param name="objective"> objective position </param>
    /// <returns></returns>
    public static Vector3 LookAt2D(Vector3 forward, Vector3 position, Vector3 objective)
    {
        /// Vector3 direction = objective.ToVector() - position.ToVector();
        Vector3 direction = new Vector3(objective.x - position.x, objective.y - position.y, position.z);
        float angle = GetAngle(forward, direction);
        Vector3 newDirection = Rotate(forward, angle);
        return newDirection;
    }

    // automatically move to
    public static Vector3 MoveTo(Vector3 position, Vector3 objective)
    {
        Vector3 direction = objective - position;
        Vector3 dirNormal = GetNormal(direction);
        return dirNormal;
    }

    // automatically turn
    public static Vector3 TurnAngle(Vector3 position, float angle)
    {
        return Rotate(position, angle);
    }
}
