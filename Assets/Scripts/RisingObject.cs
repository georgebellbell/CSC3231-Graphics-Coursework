using UnityEngine;
public class RisingObject : MonoBehaviour
{
    [SerializeField] Transform player;

    [Header("Raising Statue")]
    [SerializeField] float activationRange;
    [SerializeField] float riseAmount;
    [SerializeField] float riseSpeed = 5;

    [Header("Particle Effects")]
    [SerializeField] ParticleSystem groundSystem;
    [SerializeField] ParticleSystem weatherSystem;

    [Header("Meteor")]
    [SerializeField] bool meteorPillar;
    [SerializeField] ActivateMeteor activateMeteor;

    Vector3 start;
    Vector3 destination;
    float distanceBetween;
    private void Start()
    {
        groundSystem.Stop();
        start = transform.position;
        destination = new Vector3(transform.position.x, transform.position.y + riseAmount, transform.position.z);
    }

    private void Update()
    {
        // if the statue is currently transitioning between being underground and fully raised, the smoke particle effect will play
        if (transform.position == destination || transform.position == start)
        {
            groundSystem.Stop();
        }
        else if (!groundSystem.isPlaying)
        {
            groundSystem.Play();
        }

        // Calculates distance between player and statue
        distanceBetween = Vector3.Distance(transform.position, player.position);

        // if distance is less than activation range, statue will begin rising
        if (distanceBetween < activationRange)
        {
            // if this pillar is the meteor pillar, going close will enable the activate meteor script and allow meteor to be spawned
            if (meteorPillar)
            {
                activateMeteor.enabled = true;
            }

            // approaching the statue will trigger a unique weather effect
            if (!weatherSystem.isPlaying)
            {
                weatherSystem.Play();
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * riseSpeed);
            
        }
        // otherwise statue will move down to original position
        else
        {
            if (meteorPillar)
            {
                activateMeteor.enabled = false;
            }

            if (weatherSystem.isPlaying)
            {
                weatherSystem.Stop();
            }
            transform.position = Vector3.MoveTowards(transform.position, start, Time.deltaTime * riseSpeed);

        }

    }

}
