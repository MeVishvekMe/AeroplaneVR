using UnityEngine;

public class BaseFollower : MonoBehaviour {
    public Rigidbody airplaneRb;
    private Rigidbody baseRb;

    void Awake()
    {
        baseRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        baseRb.MovePosition(airplaneRb.position + new Vector3(0f, 0.5f, 0f));
        baseRb.MoveRotation(airplaneRb.rotation);
    }
}