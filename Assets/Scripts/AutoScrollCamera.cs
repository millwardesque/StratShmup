using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
[RequireComponent (typeof(Rigidbody))]
public class AutoScrollCamera : MonoBehaviour {
    Camera m_camera;

    public Vector3 scrollVelocity;
    public float boundaryWidth;

    void Awake() {
        m_camera = GetComponent<Camera> ();

        CreateBoundaries ();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += scrollVelocity * Time.fixedDeltaTime;
	}

    void CreateBoundaries() {
        // @TODO Calculate this from actual camera size
        float z = 10f;
        float x = 25f;

        // Flip the Y & Z coordinates here to account for the x-axis rotation

        BoxCollider topCollider = gameObject.AddComponent<BoxCollider>();
        topCollider.center = new Vector3 (0, z, transform.position.y / 2f);
        topCollider.size = new Vector3(x * 2f, boundaryWidth * 2f, transform.position.y);

        BoxCollider leftCollider = gameObject.AddComponent<BoxCollider>();
        leftCollider.center = new Vector3 (-x, 0, transform.position.y / 2f);
        leftCollider.size = new Vector3(boundaryWidth * 2f, z * 2f, transform.position.y);

        BoxCollider rightCollider = gameObject.AddComponent<BoxCollider>();
        rightCollider.center = new Vector3 (x, 0, transform.position.y / 2f);
        rightCollider.size = new Vector3(boundaryWidth * 2f, z * 2f, transform.position.y);

        BoxCollider bottomCollider = gameObject.AddComponent<BoxCollider>();
        bottomCollider.center = new Vector3 (0, -z, transform.position.y / 2f);
        bottomCollider.size = new Vector3(x * 2f, boundaryWidth * 2f, transform.position.y);
    }
}
