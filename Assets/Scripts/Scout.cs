using UnityEngine;
using System.Collections;

public enum ScoutState {
    Normal,
    TrackingEnemy
};
public class Scout : PartyMember {
    Enemy m_target;

    ScoutState m_subState;
    public ScoutState SubState {
        get { return m_subState; }
        set {
            m_subState = value;

            if (m_subState == ScoutState.Normal) {
                m_target = null;
                State = PartyMemberState.Idle;
            }
            else if (m_subState == ScoutState.TrackingEnemy) {
                OnUpdate = OnTrackingEnemy;
            }
        }
    }

    public override void ProcessInput() {
        if (Input.GetMouseButtonDown(1)) {
            Ray clickRay = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;

            hits = Physics.RaycastAll(clickRay);
            for (int i = 0; i < hits.Length; ++i) {
                if (hits[i].collider.tag == "Enemy") {
                    m_target = hits[i].collider.GetComponent<Enemy>();
                    SubState = ScoutState.TrackingEnemy;
                    break;
                }
            }
        }
    }

    protected void OnTrackingEnemy() {
        Vector3 targetVector = m_movementTarget - transform.position;
        if (targetVector.magnitude > m_targetDistanceThreshold) {
            m_rigidbody.velocity = transform.forward * maxSpeed;
        }
        else {
            State = PartyMemberState.Idle;
        }
    }

    
}
