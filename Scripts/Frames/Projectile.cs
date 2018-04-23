using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour {

    public Vector3 velocity { get; set; }

    [SerializeField]
    private bool useGravity;

    [SerializeField]
    private float speed;

    private Vector3 startPosition;

    private float timeSinceLaunch;

    [SerializeField]
    private TrailRenderer trailRender;

    void Start()
    {
        startPosition = transform.position;
        
        if(trailRender != null)
            trailRender.Clear();
    }

    void Update()
    {
        timeSinceLaunch += Time.deltaTime;

        float gravity = useGravity ? GameSettings.gravity : 0.0f;

        Vector3 nextPosition = startPosition + velocity * timeSinceLaunch + Vector3.down * (gravity / 2.0f) * timeSinceLaunch * timeSinceLaunch;

        Vector3 dir = (nextPosition - transform.position).normalized;

        transform.forward = dir;
        transform.position = nextPosition;
    }

    public Character character { get; set; }

    public float damage { get; set; }

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}
