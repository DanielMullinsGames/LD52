using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomMath {

    public enum Quadrant
    {
        TopRight,
        TopLeft,
        BottomLeft,
        BottomRight,
    }

    public enum Facing
    {
        Left,
        Right,
        Up,
        Down,
    }

    public enum CardinalDirection
    {
        North,
        East,
        South,
        West,
    }

    public static Vector2 DirectionToVector(CardinalDirection dir)
    {
        switch (dir)
        {
            case CardinalDirection.East:
                return new Vector2(1f, 0f);
            case CardinalDirection.South:
                return new Vector2(0f, -1f);
            case CardinalDirection.West:
                return new Vector2(-1f, 0f);
            default:
                return new Vector2(0, 1f);
        }
    }

    public static int IncrementAround<T>(int val, List<T> list)
    {
        int newVal = val + 1;
        if (newVal > list.Count - 1)
        {
            newVal = 0;
        }
        return newVal;
    }

    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n = default(Vector3))
    {
        n = n == default(Vector3) ? Vector3.forward : n;
        return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static float GetAngleOnCircle(Vector2 center, Vector2 point) {
		float opposite = Vector2.Distance(point, new Vector2(center.x, point.y));
		float adjacent = Vector2.Distance((Vector2)center, new Vector2(center.x, point.y));
		
		int quadrant = GetQuadrant(center, point);
		float mouseAngle =  (quadrant == 2 || quadrant == 4 ? Mathf.PI * 0.5f - Mathf.Atan2(adjacent, opposite): Mathf.Atan2(adjacent, opposite)) + (quadrant -1) * Mathf.PI * 0.5f;
		
		return mouseAngle;
	}
	
	public static int GetQuadrant(Vector2 circleCenter, Vector2 point) {
		if (point.x > circleCenter.x && point.y > circleCenter.y)
			return 1;
		else if (point.x > circleCenter.x && point.y < circleCenter.y)
			return 4;
		else if (point.x < circleCenter.x && point.y < circleCenter.y)
			return 3;
		else
			return 2;
	}

	public static Vector2 PolarOffset(Vector2 origin, float distance, float angle) {
    	float x = origin.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
    	float y = origin.y + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
 		return new Vector2(x,y);
	}
	
	public static float WrapAngle(float angle) {
		if (angle < 0) {
			return 360 - Mathf.Abs(angle);	
		}
		else if (angle > 360) {
			return angle - 360;	
		}
		else 
			return angle;
	}

    public static float AngleDelta(float angle1, float angle2)
    {
        float wrapped1 = angle1 > 180f ? angle1 - 360f : angle1;
        float wrapped2 = angle2 > 180f ? angle2 - 360f : angle2;

        return Mathf.Abs(wrapped1 - wrapped2);
    }
	
	public static int modulo(int x, int m) {
		if (m == 0)
			return 0;
		else 
   			return (x%m + m)%m;
	}

	public static float RandomValueInRange(float min, float max) {
		return min + (Random.value * (max - min));
	}

	public static Vector2 VectorFromFacing(Facing facing)
	{
		switch (facing)
		{
			case Facing.Down:
				return Vector2.down;
			case Facing.Left:
				return Vector2.left;
			case Facing.Right:
				return Vector2.right;
			default:
				return Vector2.up;
		}
	}

    public static Vector2 MidPoint(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f);
    }

    public static Vector3 MidPoint(Vector3 p1, Vector3 p2)
    {
        return new Vector3((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f, (p1.z + p2.z) / 2f);
    }

    public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
	{
		lineDir.Normalize();
		var v = pnt - linePnt;
		var d = Vector3.Dot(v, lineDir);
		return linePnt + lineDir * d;
	}
		
	public static Vector2 NearestPointOnLineSegment(Vector2 lineStart, Vector2 lineEnd, Vector2 point) {

		float l2 = Mathf.Pow(lineStart.x - lineEnd.x, 2) + Mathf.Pow(lineStart.y - lineEnd.y, 2);
		float t = Mathf.Max(0, Mathf.Min(1, Vector2.Dot(point - lineStart, lineEnd - lineStart) / l2));
		Vector2 projection = lineStart + t * (lineEnd - lineStart);
		return projection;
	}

    public static bool PassedTargetPoint(Vector2 movingPoint, Vector2 moveDirection, Vector2 targetPoint, bool ignoreX = false, bool ignoreY = false)
    {
        bool passedX = false;
        if (   (moveDirection.x > 0f && movingPoint.x > targetPoint.x)
            || (moveDirection.x < 0f && movingPoint.x < targetPoint.x)
            || (moveDirection.x == 0f))
        {
            passedX = true;
        }

        if (ignoreY)
        {
            return passedX;
        }

        bool passedY = false;
        if (   (moveDirection.y > 0f && movingPoint.y > targetPoint.y)
            || (moveDirection.y < 0f && movingPoint.y < targetPoint.y)
            || (moveDirection.y == 0f))
        {
            passedY = true;
        }

        if (ignoreX)
        {
            return passedY;
        }

        return passedX && passedY;
    }

    public static int DirectionLeftOrRight(Vector3 forward, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(forward, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1; // Right
        }
        else if (dir < 0f)
        {
            return -1; // Left
        }
        else {
            return 0;
        }
    }

    public static Vector2 ConvertToXZ(Vector3 vec3D)
    {
        return new Vector2(vec3D.x, vec3D.z);
    }
}
