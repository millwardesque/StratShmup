using UnityEngine;
using System.Collections;

public enum PartyMemberState {
    Idle,
    Walking
}
public class PartyMember : MonoBehaviour {
    Rigidbody m_rigidbody;
    float m_targetDistanceThreshold = 0.2f; // Minimum distance to target at which point we can say we've 'reached' it.

    PartyMemberState m_state;
    public PartyMemberState State {
        get { return m_state; }
        set {
            if (value == PartyMemberState.Idle) {
                m_rigidbody.velocity = Vector3.zero;
            }

            m_state = value;
        }
    }
    public float maxSpeed = 1f;

    Vector3 m_movementTarget;
    public Vector3 MovementTarget {
        get { return m_movementTarget; }
        set {
            Vector3 direction = (value - transform.position).normalized;
            m_rigidbody.MoveRotation (Quaternion.FromToRotation (new Vector3 (0f, 0f, 1f), direction));
            m_movementTarget = value;
            State = PartyMemberState.Walking;
        }
    }

    void Awake() {
        m_rigidbody = GetComponent<Rigidbody> ();
    }


    void Start () {
        GetComponent<Damageable> ().onDead = OnDead;
    }

    void OnDead() {
        FindObjectOfType<PartyController> ().OnPartyMemberDead (this);
        GameObject.Destroy (gameObject);
    }

    void FixedUpdate() {
        if (State == PartyMemberState.Walking) {
            Vector3 targetVector = m_movementTarget - transform.position;

            if (targetVector.magnitude > m_targetDistanceThreshold) {
                m_rigidbody.velocity = transform.forward * maxSpeed;
            } else {
                State = PartyMemberState.Idle;
            }
        }
    }
}
