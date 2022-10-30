using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float speed;
    
    // Update is called once per frame
    public void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speed, Space.Self);
    }
}
