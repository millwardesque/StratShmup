using UnityEngine;
using System.Collections;

public class PartyController : MonoBehaviour {
    PartyMember[] m_partyMembers;
    Camera m_camera;

    public float formationDistance = 1f;

    void Awake() {
        m_partyMembers = GetComponentsInChildren<PartyMember> ();
        Debug.Log ("Found " + m_partyMembers.Length + " party member(s)");
    }
        
	void Start () {
        m_camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButton (0)) {
            Ray clickRay = m_camera.ScreenPointToRay (Input.mousePosition);
            RaycastHit[] hits;

            // @TODO Run this against just the ground layer once docs are available and I'm off the plane...
            hits = Physics.RaycastAll (clickRay);
            for (int i = 0; i < hits.Length; ++i) {
                if (hits [i].collider.tag == "Ground") {
                    Vector3 newPosition = hits [i].point;
                    for (int j = 0; j < m_partyMembers.Length; ++j) {
                        m_partyMembers [j].MovementTarget = new Vector3 (newPosition.x, m_partyMembers[j].transform.position.y, newPosition.z);
                    }
                }
            }
        }
	}
}
