using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class NeverdawnUtility
{
    public static NeverdawnEventBase LoadEvent(XmlNode node)
    {
        NeverdawnEventBase currentEvent = NeverdawnEventBase.LoadEvent(node);

        // Triggerable events
        if (currentEvent != null)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                NeverdawnEventBase childEvent = LoadEvent(childNode);

                if (childEvent != null)
                {
                    currentEvent.children.Add(childEvent);
                }
            }
        }

        return currentEvent;
    }
 

    public static void StoreObject(MonoBehaviour behaviour, Transform parent)
    {
        behaviour.transform.SetParent(parent, true);
        behaviour.transform.localPosition = Vector3.zero;
        behaviour.transform.localRotation = Quaternion.identity;
        behaviour.gameObject.SetActive(false);
    }

    public static void UnStoreObject(MonoBehaviour behaviour, Transform parent = null)
    {
        behaviour.transform.SetParent(parent, true);
        behaviour.gameObject.SetActive(true);
    }


    public static float GetPathLength(Vector3[] corners)
    {
        return GetPathLength(corners, corners.Length);
    }

    public static float GetPathLength(Vector3[] corners, int maxIndex)
    {
        float lng = 0.0f;

        if (corners.Length > 1)
        {
            for (int i = 1; i < maxIndex; ++i)
            {
                lng += Vector3.Distance(corners[i - 1], corners[i]);
            }
        }

        return lng;
    }

    public static float GetPathLength(NavMeshPath path)
    {
        float lng = 0.0f;

        if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return lng;
    }

    public static Vector2 InputToForwardTurn(Transform characterTransform, Vector3 input, float forwardBoost = 1.5f)
    {
        if (input.magnitude == 0.0f)
        {
            return Vector2.zero;
        }

        input.y = 0.0f;

        float forwardAmount = Mathf.Clamp01(Vector3.Dot(characterTransform.forward, input));

        float forward = Mathf.Clamp01(3.0f * Mathf.Pow(forwardAmount, 2) - 2.0f * Mathf.Pow(forwardAmount, 3));
        forward = Mathf.Clamp01(Mathf.Pow(forward, 4) * forwardBoost);

        float angle = Vector3.Angle(characterTransform.forward, input);

        Vector3 cross = Vector3.Cross(characterTransform.forward, input);
        angle = cross.y < 0 ? -angle : angle;

        float turn = Mathf.Clamp(angle / 90.0f, -1.0f, 1.0f);

        return new Vector2(forward, turn);
    }

    internal static Vector3[] RefinePath(IEnumerable<Vector3> c, float p)
    {
        List<Vector3> corners = new List<Vector3>(c);

        int i = 0;
        while (i < corners.Count - 1)
        {
            if (Vector3.Distance(corners[i], corners[i + 1]) > p)
            {
                corners.Insert(i + 1, (corners[i] + corners[i + 1]) / 2.0f);
            }
            else
            {
                i++;
            }
        }

        return corners.ToArray();
    }

    internal static bool SamplePath(Vector3[] corners, float distance, out Vector3 position)
    {
        float[] distances = CreateDistanceArray(corners);

        return SamplePath(corners, distances, distance, out position);
    }

    internal static bool SamplePath(NavMeshPath path, float distance, out Vector3 position)
    {
        float[] distances = CreateDistanceArray(path.corners);

        return SamplePath(path.corners, distances, distance, out position);
    }

    // sample a given path with a given distance array at a given distance
    public static bool SamplePath(Vector3[] corners, float[] distanceArray, float distance, out Vector3 position)
    {
        if (corners.Length == 0)
        {
            position = Vector3.zero;
            return false;
        }

        if (distance < 0.0f)
        {
            position = corners[0];
            return false;
        }

        float dist = distance;

        int i = 0;

        while (i < distanceArray.Length)
        {
            if (dist <= distanceArray[i])
            {
                break;
            }

            dist -= distanceArray[i++];
        }

        if (i < distanceArray.Length)
        {
            position = Vector3.Lerp(corners[i], corners[i + 1], dist / distanceArray[i]);
            return true;
        }
        else
        {
            position = corners[i];
            return false;
        }

    }

    public static float[] CreateDistanceArray(Vector3[] corners)
    {
        if (corners.Length == 0)
        {
            return new float[0];
        }

        float[] result = new float[corners.Length - 1];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Vector3.Distance(corners[i + 1], corners[i]);
        }

        return result;
    }


    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {

        List<Vector3> points;
        List<Vector3> curvedPoints;

        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;

        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }

    internal static Vector3[] PathAlongTiles(List<HexTile> path, float sampleLength, int relaxIterations = 0)
    {
        return RelaxPath(RefinePath(path.Select(p => p.position), sampleLength), relaxIterations);
    }

    internal static Vector3[] RelaxPath(Vector3[] corners, int iterations)
    {
        for (int k = 0; k < iterations; k++)
        {
            for (int i = 1; i < corners.Length - 1; i++)
            {
                corners[i].x = (corners[i - 1].x + corners[i + 1].x) / 2.0f;
                corners[i].z = (corners[i - 1].z + corners[i + 1].z) / 2.0f;
            }
        }

        return corners;
    }

    /// <summary>
    /// Wraps an index, so that it is larger than 0 and smaller than arrayLength
    /// </summary>
    /// <param name="index">The index to wrap</param>
    /// <param name="arrayLength">The length of the array</param>
    /// <returns>The wrapped index</returns>
    internal static int RepeatIndex(int index, int arrayLength)
    {
        if (arrayLength == 0)
            return 0;

        while (index < 0)
            index += arrayLength;

        while (index >= arrayLength)
        {
            index -= arrayLength;
        }

        return index;
    }


    public static void KillIfExists<T>(Behaviour behaviour) where T : Behaviour
    {
        T toKill = behaviour.GetComponent<T>();

        if (toKill != null)
        {
            GameObject.Destroy(toKill);
        }
    }

    public static void DisableIfExists<T>(Behaviour behaviour) where T : Behaviour
    {
        T toKill = behaviour.GetComponentInChildren<T>();

        if (toKill != null)
        {
            toKill.enabled = false;
        }
    }

    public static void EnableIfExists<T>(Behaviour behaviour) where T : Behaviour
    {
        T toKill = behaviour.GetComponentInChildren<T>();

        if (toKill != null)
        {
            toKill.enabled = true;
        }
    }

    internal static void AddIfNotExists<T>(Behaviour behaviour) where T : Behaviour
    {
        T toKill = behaviour.GetComponent<T>();

        if (toKill == null)
        {
            behaviour.gameObject.AddComponent<T>();
        }
    }

    internal static bool PointInOABB(Vector3 point, BoxCollider box)
    {
        point = box.transform.InverseTransformPoint(point) - box.center;

        float halfX = (box.size.x * 0.5f);
        float halfY = (box.size.y * 0.5f);
        float halfZ = (box.size.z * 0.5f);
        if (point.x < halfX && point.x > -halfX &&
           point.y < halfY && point.y > -halfY &&
           point.z < halfZ && point.z > -halfZ)
            return true;
        else
            return false;
    }


   

    private static float getThrowHeight(float x, float v, float a)
    {
        return x * Mathf.Tan(a) - (GameSettings.gravity / (2.0f * Mathf.Pow(Mathf.Cos(a), 2) * Mathf.Pow(v, 2))) * Mathf.Pow(x, 2);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }



   
}

public class CharacterUtils
{
    internal static List<Character> GetCharactersInRange(IEnumerable<Character> characters, Vector3 position, float radius)
    {
        return characters.Where(c => Vector3.Distance(position, c.position) <= radius).ToList();
    }

    internal static Character GetClosestCharacter(IEnumerable<Character> characters, Vector3 position)
    {
        return characters.Where(c => c.isAlive).OrderBy(c => Vector3.Distance(c.transform.position, position)).FirstOrDefault();
    }

    internal static Character[] GetCharactersInRange(IEnumerable<Character> characters, HexTile hexTile, int maxRange)
    {
        List<Character> charactersInRange = new List<Character>();

        foreach (Character character in characters)
        {
            int distance = Pathfinder.Distance(hexTile, character.currentTile);

            if (distance >= 0 && distance <= maxRange)
            {
                charactersInRange.Add(character);
            }
        }

        return charactersInRange.ToArray();
    }
}

public class ProjectileUtils {
 

    /// <summary>
    /// Calculates the two possible initial angles that could be used to fire a projectile at the supplied
    /// speed to travel the desired distance
    /// </summary>
    /// <param name="speed">Initial speed of the projectile</param>
    /// <param name="distance">Distance along the horizontal axis the projectile will travel</param>
    /// <param name="yOffset">Elevation of the target with respect to the initial fire position</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <param name="angle0"></param>
    /// <param name="angle1"></param>
    /// <returns>False if the target is out of range</returns>
    public static bool LaunchAngle(float speed, float distance, float yOffset, float gravity, out float angle0, out float angle1)
    {
        angle0 = angle1 = 0;

        float speedSquared = speed * speed;

        float operandA = Mathf.Pow(speed, 4);
        float operandB = gravity * (gravity * (distance * distance) + (2 * yOffset * speedSquared));

        // Target is not in range
        if (operandB > operandA)
            return false;

        float root = Mathf.Sqrt(operandA - operandB);

        angle0 = Mathf.Atan((speedSquared + root) / (gravity * distance));
        angle1 = Mathf.Atan((speedSquared - root) / (gravity * distance));

        return true;
    }

    /// <summary>
    /// Calculates the initial launch speed required to hit a target at distance with elevation yOffset.
    /// </summary>
    /// <param name="distance">Planar distance from origin to the target</param>
    /// <param name="yOffset">Elevation of the origin with respect to the target</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <param name="angle">Initial launch angle in radians</param>
    /// <returns>Initial launch speed</returns>
    public static float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }

    /// <summary>
    /// Calculates how long a projectile will stay in the air before reaching its target
    /// </summary>
    /// <param name="speed">Initial speed of the projectile</param>
    /// <param name="angle">Initial launch angle in radians</param>
    /// <param name="yOffset">Elevation of the target with respect to the initial fire position</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <returns></returns>
    public static float TimeOfFlight(float speed, float angle, float yOffset, float gravity)
    {
        float ySpeed = speed * Mathf.Sin(angle);

        float time = (ySpeed + Mathf.Sqrt((ySpeed * ySpeed) + 2 * gravity * yOffset)) / gravity;

        return time;
    }

    /// <summary>
    /// Samples a series of points along a projectile arc
    /// </summary>
    /// <param name="iterations">Number of points to sample</param>
    /// <param name="speed">Initial speed of the projectile</param>
    /// <param name="distance">Distance the projectile will travel along the horizontal axis</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <param name="angle">Initial launch angle in radians</param>
    /// <returns>Array of sampled points with the length of the supplied iterations</returns>
    public static Vector2[] ProjectileArcPoints(int iterations, float speed, float distance, float gravity, float angle)
    {
        float iterationSize = distance / iterations;

        float radians = angle;

        Vector2[] points = new Vector2[iterations + 1];

        for (int i = 0; i <= iterations; i++)
        {
            float x = iterationSize * i;
            float t = x / (speed * Mathf.Cos(radians));
            float y = -0.5f * gravity * (t * t) + speed * Mathf.Sin(radians) * t;

            Vector2 p = new Vector2(x, y);

            points[i] = p;
        }

        return points;

    }

    public static float calculateDelta(float x, float y, float velocity) {
        return velocity * velocity * velocity * velocity - GameSettings.gravity * (GameSettings.gravity * x * x + 2 * y * velocity * velocity);
    }
        /**
         * Checks if projectile can hit (x, y) coordinate with initial velocity length under given gravity.
         * @param x
         * @param y
         * @param velocity initial velocity
         * @param gravity gravity value; should be greater than 0
         * @return
         */
        public static bool canHitCoordinate(float x, float y,  float velocity) {
            return calculateDelta(x, y, velocity) >= 0;
        }
 
        /**
         * Calculates angle to hit given (x, y) coordinate with given velocity and gravity.
         * @param x
         * @param y
         * @param velocity initial velocity
         * @param gravity gravity value; should be greater than 0
         * @return angle in radians
         */
        public static float calculateAngle1ToHitCoordinate(float x, float y, float velocity) {
            if (x == 0) return y > 0 ? -Mathf.PI * 0.5f : Mathf.PI * 0.5f;
            float delta = calculateDelta(x, y, velocity);
            float sqrtDelta = Mathf.Sqrt(delta);
            return Mathf.Atan((velocity * velocity - sqrtDelta)/(GameSettings.gravity * x));;
        }
 
        /**
         * Calculates angle to hit given (x, y) coordinate with given velocity and gravity.
         * @param x
         * @param y
         * @param velocity initial velocity
         * @param gravity gravity value; should be greater than 0
         * @return angle in radians
         */
        public static float calculateAngle2ToHitCoordinate(float x, float y, float velocity) {
            if (x == 0) return -Mathf.PI * 0.5f;
            float delta = calculateDelta(x, y, velocity);
            float sqrtDelta = Mathf.Sqrt(delta);
            return Mathf.Atan((velocity * velocity + sqrtDelta)/(GameSettings.gravity * x));
        }

        public static bool tryHitWithBallisticCast(Frame target, Vector3 position, Vector3 velocity, float angle, out Vector3 force)
        {
            force = Vector3.RotateTowards(velocity, Vector3.up, angle, 0.0f);

           

            // Cast the ballistic curve to see, if it hits the target, otherwise, keep looking!
            RaycastHit hit;

            if (ProjectileUtils.BallisticCast(position, force, out hit))
            {
                Frame d = hit.collider.GetComponent<Frame>();

                if (d == target)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool BallisticCast(Vector3 position, Vector3 velocity, out RaycastHit hit, float precision = 0.5f)
        {
            float t = 0.0f;
            float step = precision / velocity.magnitude;
            Vector3 prevPosition = position;

            for (int j = 0; j < 1000; j++)
            {
                t += step;

                Vector3 pos = position + velocity * t + Vector3.down * (9.8f / 2.0f) * t * t;

                // NeverdawnUtility.DrawLine(prevPosition, pos, Color.white, 5.0f);

                if (Physics.Linecast(prevPosition, pos, out hit))
                {
                    return true;
                }

                prevPosition = pos;
            }

            hit = new RaycastHit();
            return false;
        }

       
        
}