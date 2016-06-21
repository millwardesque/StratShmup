using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyMember : MonoBehaviour {
    protected Camera m_camera;
    Stack<PartyMemberState> m_states;

    public float maxSpeed = 1f;
    public int meleeStrength = 1;

    Rigidbody m_rigidbody;
    public Rigidbody MemberRigidbody {
        get { return m_rigidbody; }
    }

    Vector3 m_movementTarget;
    public Vector3 MovementTarget {
        get { return m_movementTarget; }
        set {
            Vector3 direction = (value - transform.position).normalized;
            m_rigidbody.MoveRotation (Quaternion.FromToRotation (new Vector3 (0f, 0f, 1f), direction));
            m_movementTarget = value;
        }
    }

    void Awake() {
        m_states = new Stack<PartyMemberState> ();
        m_rigidbody = GetComponent<Rigidbody> ();
    }

    void Start () {
        m_states.Clear ();
        PushState (new PartyMemberStayInFormationState(this));

        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GetComponent<Damageable> ().onDead = OnDead;
    }

    public void PushState(PartyMemberState state) {
        if (m_states.Count > 0) {
            m_states.Peek ().Exit ();
        }
        m_states.Push (state);
        state.Enter ();
    }

    public PartyMemberState PeekState() { 
        return m_states.Peek ();
    }

    public PartyMemberState PopState() { 
        if (m_states.Count > 0) {
            PartyMemberState popped = m_states.Pop ();
            popped.Exit ();

            if (m_states.Count > 0) {
                m_states.Peek ().Enter ();
            }
            return popped;
        }
        else {
            return null;
        }
    }
   
    void OnDead() {
        FindObjectOfType<PartyController> ().OnPartyMemberDead (this);
        GameObject.Destroy (gameObject);
    }

    void FixedUpdate() {
        ProcessInput ();
        if (m_states.Count > 0) {
            PeekState ().FixedUpdate ();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.tag == "Enemy") {
            col.collider.GetComponent<Damageable>().Health -= calculateMeleeDamage();
        }
    }

    protected virtual int calculateMeleeDamage() {
        return meleeStrength;
    }

    public virtual void ProcessInput() { }

    public bool HasReachedLocation(Vector3 location) {
        return (location - transform.position).magnitude <= GameConfig.TargetDistanceThreshold;
    }

    public bool HasReachedTarget() {
        return HasReachedLocation (m_movementTarget);
    }
}
