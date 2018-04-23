using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WalkCursor : AbilityCursorBase
{
    private NavMeshAgent agent;

    private NavMeshPath navMeshPath;


    private MeshRenderer meshRenderer;

    private Vector3 root;

    private  GradientColorKey[] gradientColorKeys;

    private MeshFilter meshFilter;

    [SerializeField]
    private float width;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    public Color[] colors;

    private Vector3 targetDirection;

    private float turnSpeed;

   
    protected override void OnInitializeCursor()
    {
        NeverdawnCamera.AddTargetLerped(targetTransform);

        agent = targetTransform.GetComponent<NavMeshAgent>();
        meshFilter = GetComponent<MeshFilter>();
        navMeshPath = new NavMeshPath();
        meshRenderer = GetComponent<MeshRenderer>();

        NavMeshAnimator walker = character.GetComponentInChildren<NavMeshAnimator>();
        turnSpeed = 0.5f * walker.turnSpeed;
        root = character.position;
        agent.Warp(root);

        createGradient();
    }

    void OnDestroy()
    {
        NeverdawnCamera.RemoveTargetLerped(targetTransform);
    }

    protected override bool OnConfirmCursor()
    {
        if(navMeshPath.status != NavMeshPathStatus.PathInvalid)
        {
            return true;
        }

        return false;
    }

    public override void UpdateValues(Vector3 direction, float intensity)
    {
        if (isActive)
        {
            agent.Move(direction);

            createPath();

            if (targetDirection.sqrMagnitude > 0.1f)
            {
                float angle = Vector3.SignedAngle(character.forward, targetDirection, Vector3.up);

                if (Mathf.Abs(angle) > 10.0f)
                {
                    character.transform.Rotate(Vector3.up, Mathf.Sign(angle) * turnSpeed * Time.deltaTime);
                }
            }
            // character.forward = Vector3.RotateTowards(character.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
        }
    }

    private void createGradient()
    {
        Gradient gradient = new Gradient();

        gradientColorKeys = new GradientColorKey[4];
        gradientColorKeys[0] = new GradientColorKey(Color.green, 0.0f);
        gradientColorKeys[1] = new GradientColorKey(Color.green, 0.0f);
        gradientColorKeys[2] = new GradientColorKey(Color.red, 0.0f);
        gradientColorKeys[3] = new GradientColorKey(Color.red, 1.0f);

        gradient.colorKeys = gradientColorKeys;

    }

    private void createPath()
    {
        if (NavMesh.CalculatePath(root, targetTransform.position, NavMesh.AllAreas, navMeshPath))
        {
            if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
            {
                float pathLength = NeverdawnUtility.GetPathLength(navMeshPath);

                path = NeverdawnUtility.RelaxPath(NeverdawnUtility.RefinePath(navMeshPath.corners, 0.2f), 10);

                for (int i = 0; i < path.Length; i++)
                {
                    RaycastHit rayHit;

                    // experimental: stick to ground
                    if (Physics.Raycast(path[i] + 2 * Vector3.up, Vector3.down, out rayHit, 4f, 1 << 9))
                    {
                        path[i] = rayHit.point + 0.05f * Vector3.up;
                    }
                }

                if (path.Length > 1)
                {
                    List<Vector3> vertices = new List<Vector3>();
                    List<int> triangles = new List<int>();
                    List<Color> vertexColors = new List<Color>();

                    Vector3 dir = (path[1] - path[0]);
                    dir.y = 0.0f;
                    dir.Normalize();

                    Vector3 side = Vector3.Cross(dir, Vector3.up).normalized;

                    targetDirection = dir;

                    int colorIndex = 0;

                    vertices.Add(path[0] + width * side);
                    vertices.Add(path[0] - width * side);
                    vertexColors.Add(colors[colorIndex]);
                    vertexColors.Add(colors[colorIndex]);

                    int index = 0;

                    for (int i = 1; i < path.Length - 1; i++)
                    {
                        dir = ((path[i] - path[i - 1]).normalized + (path[i + 1] - path[i]).normalized) / 2.0f;
                        side = Vector3.Cross(dir, Vector3.up).normalized;

                        float distance = NeverdawnUtility.GetPathLength(path, i);

                        colorIndex = distance > maxDistance ? 1 : 0;
                        // colorIndex = NeverdawnUtility.RepeatIndex((int)(distance / walkAbility.maxDistance), colors.Length);

                        vertices.Add(path[i] + width * side);
                        vertices.Add(path[i] - width * side);

                        vertexColors.Add(colors[colorIndex]);
                        vertexColors.Add(colors[colorIndex]);

                        triangles.Add(index + 0);
                        triangles.Add(index + 2);
                        triangles.Add(index + 3);

                        triangles.Add(index + 1);
                        triangles.Add(index + 0);
                        triangles.Add(index + 3);

                        index += 2;

                        //if(distance < walkAbility.maxDistance && NeverdawnUtility.GetPathLength(line, i + 1) > walkAbility.maxDistance)
                        //{
                        //    Vector3 maxPos = Vector3.zero;

                        //    NeverdawnUtility.SamplePath(line, walkAbility.maxDistance, out maxPos);


                        //}
                    }

                    dir = path[path.Length - 1] - path[path.Length - 2];
                    side = Vector3.Cross(dir, Vector3.up).normalized;

                    vertices.Add(path[path.Length - 1] + width * side);
                    vertices.Add(path[path.Length - 1] - width * side);

                    vertexColors.Add(colors[colorIndex]);
                    vertexColors.Add(colors[colorIndex]);

                    triangles.Add(index + 0);
                    triangles.Add(index + 2);
                    triangles.Add(index + 3);

                    triangles.Add(index + 1);
                    triangles.Add(index + 0);
                    triangles.Add(index + 3);

                    vertices.Add(path[path.Length - 1] + 1.5f * width * side);
                    vertices.Add(path[path.Length - 1] - 1.5f * width * side);
                    vertices.Add(path[path.Length - 1] + 0.3f * dir.normalized);

                    vertexColors.Add(colors[colorIndex]);
                    vertexColors.Add(colors[colorIndex]);
                    vertexColors.Add(colors[colorIndex]);

                    triangles.Add(vertices.Count - 1);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 3);



                    Mesh mesh = new Mesh();
                    mesh.vertices = vertices.ToArray();
                    mesh.triangles = triangles.ToArray();
                    mesh.colors = vertexColors.ToArray();

                    meshFilter.sharedMesh = mesh;

                }
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach(Vector3 pos in navMeshPath.corners)
        {
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }

    protected override void OnHideCursor()
    {
        meshRenderer.enabled = false;
    }

    protected override bool OnCancelCursor()
    {
        return true;
    }

    public float maxDistance { get; set; }


    public Vector3[] path { get; private set; }
}
