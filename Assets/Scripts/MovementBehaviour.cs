using UnityEngine;
using System.Collections;

public interface MovementBehaviour {
    void OnMovementUpdate(GameObject mover);
}

public class IdleMovementBehaviour : MovementBehaviour{
    public void OnMovementUpdate(GameObject mover) {
        // Do nothing since we're idling...    
    }
}

public class WalkingMovementBehaviour : MovementBehaviour {
    Vector3 m_movementTarget;
    public Vector3 MovementTarget {
        get { return m_movementTarget; }
        set {
            m_movementTarget = value;
        }
    }

    float m_targetDistanceThreshold;
    public float TargetDistanceThreshold {
        get { return m_targetDistanceThreshold; }
        set {
            m_targetDistanceThreshold = value;
        }
    }

    float m_maxSpeed;
    public float MaxSpeed {
        get { return m_maxSpeed; }
        set { m_maxSpeed = value; }
    }

    public void OnMovementUpdate(GameObject mover) {
        Vector3 targetVector = m_movementTarget - mover.transform.position;

        if (targetVector.magnitude > TargetDistanceThreshold) {
            mover.GetComponent<Rigidbody>().velocity = mover.transform.forward * MaxSpeed;
        }
        else {
            State = PartyMemberState.Idle;
        }
    }
}

