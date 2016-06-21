using UnityEngine;
using System.Collections;

public class PartyMemberState {
    protected PartyMember m_member;

    public string Name {
        get { return GetType ().Name; }
    }

    public PartyMemberState(PartyMember member) {
        m_member = member;
    }

    public virtual void Enter() {
        // Debug.Log (string.Format ("[{0}] Entered state {1}", m_member.name, Name));
    }
    public virtual void Exit() {
        // Debug.Log (string.Format ("[{0}] Exited state {1}", m_member.name, Name));
    }
    public virtual void FixedUpdate() { }
}

public class PartyMemberIdleState : PartyMemberState {
    public PartyMemberIdleState(PartyMember member) : base(member) { }

    public override void Enter() {
        base.Enter ();
        m_member.MemberRigidbody.velocity = Vector3.zero;
    }
}

public class PartyMemberStayInFormationState : PartyMemberState {
    public PartyMemberStayInFormationState(PartyMember member) : base(member) { }

    public override void FixedUpdate() {
        if (!m_member.HasReachedTarget ()) {
            m_member.MemberRigidbody.velocity = m_member.transform.forward * m_member.maxSpeed;
        }
        else {
            m_member.MemberRigidbody.velocity = Vector3.zero;
        }
    }
}

public class PartyMemberWalkingState : PartyMemberState {
    public PartyMemberWalkingState(PartyMember member) : base(member) { }

    public override void FixedUpdate() {
        if (m_member.HasReachedTarget ()) {
            m_member.PopState ();
            return;
        }
        else {
            m_member.MemberRigidbody.velocity = m_member.transform.forward * m_member.maxSpeed;
        }
    }
}
