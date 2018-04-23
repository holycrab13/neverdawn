using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BallisticCursor : AbilityCursorBase
{
    private Vector3 direction;

    private Vector3 inputVector;

    private float inputAngle;

    private float velocity;

    private Vector3 root;

    private MeshFilter meshFilter;

    [SerializeField]
    private float startWidth;

    [SerializeField]
    private float endWidth;

    [SerializeField]
    private Transform targetTransform;

    public float minPower { get { return maxPower / 2.0f; } }

    public float maxPower { get; set; }

    public Vector3 projectileSpawn { get; set; }

    public Vector3 targetVelocity { get; private set; }


    protected override bool OnConfirmCursor()
    {
        targetVelocity = Vector3.RotateTowards(direction, Vector3.up, inputAngle, 0.0f) * velocity;
        return true;
    }

    protected override void OnInitializeCursor()
    {
        NeverdawnCamera.AddTargetLerped(targetTransform);

        meshFilter = GetComponent<MeshFilter>();

        root = character.transform.position + character.transform.TransformDirection(projectileSpawn);
        inputVector = character.transform.forward * minPower;

        inputAngle = Mathf.PI / 4.0f;

        updateCursor();
    }


    void OnDestroy()
    {
        NeverdawnCamera.RemoveTargetLerped(targetTransform);
    }

    public override void UpdateValues(Vector3 input, float intensity)
    {
        if (input.sqrMagnitude > 0.0f || intensity != 0.0f)
        {
            inputVector.x += 1.5f * input.x;
            inputVector.z += 1.5f * input.z;
            inputAngle += intensity;

            inputAngle = Mathf.Clamp(inputAngle, -Mathf.PI / 3.0f, Mathf.PI / 3.0f);

            if (inputVector.magnitude > maxPower)
            {
                inputVector = inputVector.normalized * maxPower;
            }

            if (inputVector.magnitude < minPower)
            {
                inputVector = inputVector.normalized * minPower;
            }

            updateCursor();
        }

    }

    private void updateCursor()
    {
        velocity = inputVector.magnitude;

        direction.y = 0.0f;
        direction.x = inputVector.x;
        direction.z = inputVector.z;
        direction.Normalize();

        character.transform.forward = direction;
        root = character.transform.position + character.transform.TransformDirection(projectileSpawn);

        float maxSampleDistance = 6.0f;
        float t = 0.0f;

        List<Vector3> positions = new List<Vector3>();
        Vector3 v0 = Vector3.RotateTowards(direction, Vector3.up, inputAngle, 0.0f) * velocity;
        Vector3 lastPos = root;
        Vector3 side = Vector3.Cross(v0, Vector3.up).normalized;
       
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float width = startWidth;

        vertices.Add(root + width * side);
        vertices.Add(root - width * side);


        int index = 0;

        while(t < maxSampleDistance)
        {
            t += 0.1f;
            Vector3 pos = root + v0 * t + Vector3.down * (GameSettings.gravity / 2.0f) * t * t;

            width = Mathf.Lerp(startWidth, endWidth, t / maxSampleDistance);

            vertices.Add(pos + width * side);
            vertices.Add(pos - width * side);

            triangles.Add(index + 0);
            triangles.Add(index + 2);
            triangles.Add(index + 3);

            triangles.Add(index + 1);
            triangles.Add(index + 0);
            triangles.Add(index + 3);

            index += 2;

            positions.Add(pos);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        meshFilter.sharedMesh = mesh;

        targetTransform.position = root + (Mathf.Pow(velocity, 2.0f) / GameSettings.gravity) * Mathf.Sin(2.0f * inputAngle) * direction;
    }

    private float getThrowHeight(float x)
    {
        return x * Mathf.Tan(inputAngle) - (GameSettings.gravity / (2.0f * Mathf.Pow(Mathf.Cos(inputAngle), 2) * Mathf.Pow(velocity, 2))) * Mathf.Pow(x, 2);
    }

    // y =  x * Mathf.Tan(inputAngle) - (GameSettings.gravity / (2.0f * Mathf.Pow(Mathf.Cos(inputAngle), 2) * Mathf.Pow(velocity, 2))) * Mathf.Pow(x, 2);

    protected override void OnHideCursor()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    protected override bool OnCancelCursor()
    {
        return true;
    }
}
