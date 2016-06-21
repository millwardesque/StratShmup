using UnityEngine;
using System.Collections;

public class ScoutTrackingEnemyState : PartyMemberState {
    Enemy m_enemy;
    public Enemy TrackedEnemy {
        get { return m_enemy; }
        set { 
            m_enemy = value;
        }
    }

    public ScoutTrackingEnemyState(PartyMember member) : base(member) { }

    public override void FixedUpdate() {
        if (TrackedEnemy == null) {
            m_member.PopState ();
            return;
        }

        m_member.MovementTarget = TrackedEnemy.transform.position;
        if (m_member.HasReachedTarget()) {
            m_member.PopState ();
            return;
        }
        else {
            m_member.MemberRigidbody.velocity = m_member.transform.forward * m_member.maxSpeed;
        }
    }
}

public class Scout : PartyMember {
    public override void ProcessInput() {
        if (Input.GetMouseButtonDown(1)) {
            Ray clickRay = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;

            hits = Physics.RaycastAll(clickRay);
            for (int i = 0; i < hits.Length; ++i) {
                if (hits[i].collider.tag == "Enemy") {
                    ScoutTrackingEnemyState newState = new ScoutTrackingEnemyState (this);
                    newState.TrackedEnemy = hits [i].collider.GetComponent<Enemy> ();
                    PushState (newState);
                    break;
                }
            }
        }
    }
}
