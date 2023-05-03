
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BugController : UdonSharpBehaviour
{
    [SerializeField] MeshRenderer linkedMeshRenderer;
    [SerializeField] float maxDistanceFromOrigin = 5;
    [SerializeField] float movementSpeed = 1;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float holdDistance = 1;

    public Vector3 target = Vector3.zero;

    public double updateTime;

    float sqrHoldDistance;
    bool reached = false;

    public Material material
    {
        set
        {
            linkedMeshRenderer.material = value;
        }
    }


    public void SetNextTarget()
    {
        target = new Vector3(Random.Range(-maxDistanceFromOrigin, maxDistanceFromOrigin), 0, Random.Range(-maxDistanceFromOrigin, maxDistanceFromOrigin));

        if (transform.parent != null) target += transform.parent.position;

        reached = false;

        SendCustomEventDelayedSeconds(nameof(SetNextTarget), 10, VRC.Udon.Common.Enums.EventTiming.Update);
    }


    private void Start()
    {
        SetNextTarget();

        sqrHoldDistance = holdDistance * holdDistance;
    }

    private void Update()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        sw.Start();

        if (reached) return;

        reached = (transform.position - target).sqrMagnitude < sqrHoldDistance;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target - transform.position, rotationSpeed * Time.deltaTime, 0f));

        transform.Translate(Time.deltaTime * movementSpeed  * Vector3.forward);

        sw.Stop();

        updateTime = sw.Elapsed.TotalSeconds * 1000;
    }
}
