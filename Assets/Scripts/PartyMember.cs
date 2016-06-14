using UnityEngine;
using System.Collections;

public class PartyMember : MonoBehaviour {
    Rigidbody m_rigidbody;
    float m_targetDistanceThreshold = 0.2f; // Minimum distance to target at which point we can say we've 'reached' it.

    public float maxSpeed = 1f;

    Vector3 m_movementTarget;
    public Vector3 MovementTarget {
        get { return m_movementTarget; }
        set {
            m_movementTarget = value;
        }
    }

    void Awake() {
        m_rigidbody = GetComponent<Rigidbody> ();
    }

    void FixedUpdate() {
        Vector3 targetVector = m_movementTarget - transform.position;
        if (targetVector.magnitude > m_targetDistanceThreshold) {
            m_rigidbody.velocity = targetVector.normalized * maxSpeed;
        }
        else {
            m_rigidbody.velocity = Vector3.zero;
        }
    }
}
