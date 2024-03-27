using UnityEngine;

public class OrbitalMovement : MonoBehaviour
{
    public Transform player; // Reference to the player
    [SerializeField] private float minOrbitDistance = 2.0f; // Minimum orbit distance
    [SerializeField] private float maxOrbitDistance = 5.0f; 
    [SerializeField] float orbitDistance = 2.0f; // Distance from the player
    
    [SerializeField] float orbitDegreesPerSec = 180.0f; // Speed of orbit
    
    Vector3 playerMovementDirection;
    float movementSpeedModifier = 1.0f;

    void Update()
    {
        //UpdateOrbitDistance();
        OrbitAround();
    }
    
    public void UpdatePlayerMovement(Vector3 movementDirection)
    {
        playerMovementDirection = movementDirection;
    }

    void UpdateOrbitDistance()
    {
        Vector3 mouseWorldPosition = GameManager.Instance.GetMouseWorldPosition();
        float distanceToMouse = Vector3.Distance(player.position, mouseWorldPosition);
        orbitDistance = Mathf.Clamp(distanceToMouse, minOrbitDistance, maxOrbitDistance);
    }

    void OrbitAround()
    {
        // Calculate the orbit position
        transform.position = player.position + 
                             new Vector3(Mathf.Cos(Time.time * orbitDegreesPerSec * Mathf.Deg2Rad) * orbitDistance, 
                                 0, 
                                 Mathf.Sin(Time.time * orbitDegreesPerSec * Mathf.Deg2Rad) * orbitDistance) + Vector3.up;
        
      
    }
}
