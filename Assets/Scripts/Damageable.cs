using UnityEngine;
using System.Collections;

public delegate void OnDeadCallback();

public class Damageable : MonoBehaviour {
    float m_health;
    bool m_isDead = false;
    public float Health {
        get { return m_health; }
        set {
            m_health = value;

            if (m_health < 0f && !m_isDead) {
                Dead ();
            }
        }
    }

    public float startingHealth = 1f;
    public OnDeadCallback onDead = null;

	// Use this for initialization
	void Start () {
        m_health = startingHealth;
	}

    void Dead() {
        Debug.Log (name + " is dead.");
        m_isDead = true;
        if (onDead != null) {
            onDead ();
        }
    }
}
