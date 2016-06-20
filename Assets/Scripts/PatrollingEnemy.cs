using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PatrollingEnemy : Enemy {
    bool m_isMovingToEnd;
    Rigidbody m_rigidbody;

    public Vector3 startPoint;
    public Vector3 endPoint;
    public float patrolSpeed;
    public float waypointThreshold = 0.2f;

    void Awake() {
        m_rigidbody = GetComponent<Rigidbody> ();
    }
	// Use this for initialization
	void Start () {
        Vector3 direction = (endPoint - transform.position).normalized;
        m_rigidbody.velocity = direction * patrolSpeed;
        m_isMovingToEnd = true;

        GetComponent<Damageable> ().onDead = OnDead;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (m_isMovingToEnd) {
            if ((endPoint - transform.position).magnitude < waypointThreshold) {
                m_isMovingToEnd = false;
                Vector3 direction = (startPoint - transform.position).normalized;
                m_rigidbody.velocity = direction * patrolSpeed;
            }
        }
        else {
            if ((startPoint - transform.position).magnitude < waypointThreshold) {
                m_isMovingToEnd = true;
                Vector3 direction = (endPoint - transform.position).normalized;
                m_rigidbody.velocity = direction * patrolSpeed;
            }
        }
	}

    void OnDead() {
        GameObject.Destroy (gameObject);
    }
}
