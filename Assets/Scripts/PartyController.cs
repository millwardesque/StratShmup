using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PartyController : MonoBehaviour {
    PartyMember[] m_partyMembers;
    Camera m_camera;

    private PartyMember m_selectedMember;
       
    private Material m_savedMaterial;
    public Material selectedMaterial;
    public float formationDistance = 1f;

    void Awake() {
        m_partyMembers = GetComponentsInChildren<PartyMember> ();
    }
        
	void Start () {
        m_camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown (0)) {
            Ray clickRay = m_camera.ScreenPointToRay (Input.mousePosition);
            RaycastHit[] hits;

            hits = Physics.RaycastAll (clickRay);
            for (int i = 0; i < hits.Length; ++i) {
                if (hits [i].collider.tag == "Player Party" && hits [i].collider.GetComponent<PartyMember> () != null) {
                    SelectPartyMember (hits [i].collider.GetComponent<PartyMember> ());
                    break;
                }
            }
        }
        else if (Input.GetMouseButton (0)) {
            Ray clickRay = m_camera.ScreenPointToRay (Input.mousePosition);
            RaycastHit[] hits;

            hits = Physics.RaycastAll (clickRay);
            for (int i = 0; i < hits.Length; ++i) {
                if (hits [i].collider.tag == "Ground") {
                    Vector3 newPosition = hits [i].point;

                    m_partyMembers [0].MovementTarget = new Vector3 (newPosition.x, m_partyMembers[0].transform.position.y, newPosition.z);
                    break;
                }
            }
        }

        KeepInFormation ();
	}

    void SelectPartyMember(PartyMember member) {
        if (m_selectedMember == member) {
            DeselectPartyMember ();
        } else {
            if (m_selectedMember != null) {
                DeselectPartyMember ();
            }
            m_savedMaterial = member.GetComponentInChildren<Renderer> ().material;
            m_selectedMember = member;
            m_selectedMember.GetComponentInChildren<Renderer>().material = selectedMaterial;
        }
    }

    void DeselectPartyMember() {
        m_selectedMember.GetComponentInChildren<Renderer>().material = m_savedMaterial;
        m_selectedMember = null;
    }

    void KeepInFormation() {
        PartyMember leader = m_partyMembers [0];
        int midwayElement = m_partyMembers.Length / 2;

        // Calculate the formation slot positions.
        PartyMember[] formationSlots = new PartyMember[m_partyMembers.Length];
        for (int i = 0; i < formationSlots.Length; ++i) {
            formationSlots [i] = null;
        }

        // Get the slot positions.
        Vector3[] slots = GetLineAbreastSlots (leader, midwayElement);

        // Reserve the central slot for the leader.
        formationSlots [midwayElement] = leader;
         
        // Find the closest slot for each remaining party member.
        for (int i = 1; i < m_partyMembers.Length; ++i) {
            float bestSqrDistance = 0f;
            int bestSlot = -1;
            for (int j = 0; j < formationSlots.Length; ++j) {
                if (formationSlots[j] == null) {
                    float sqrDistance = (slots [j] - m_partyMembers [i].transform.position).sqrMagnitude;
                    if (bestSlot == -1 || sqrDistance < bestSqrDistance) {
                        bestSlot = j;
                        bestSqrDistance = sqrDistance;
                    }
                }
            }

            formationSlots [bestSlot] = m_partyMembers [i];
            m_partyMembers [i].MovementTarget = slots[bestSlot];
        }
    }
        
    Vector3[] GetLineAbreastSlots(PartyMember leader, int midwayElement) {
        Vector3[] slots = new Vector3[m_partyMembers.Length];
        for (int i = 0; i < slots.Length; ++i) {
            int squaresFromLeader = i - midwayElement;
            Vector3 leaderOffset = squaresFromLeader * formationDistance * leader.transform.right;
            slots [i] = leader.transform.position + leaderOffset;
        }

        return slots;
    }

    public void OnPartyMemberDead(PartyMember deadMember) {
        PartyMember[] newPartyMembers = new PartyMember[m_partyMembers.Length - 1];
        int j = 0;
        for (int i = 0; i < m_partyMembers.Length; ++i) {
            if (m_partyMembers[i] != null && m_partyMembers[i] != deadMember) {
                newPartyMembers [j] = m_partyMembers [i];
                j++;
            }
        }

        if (0 == newPartyMembers.Length) {
            Debug.Log ("GAME OVER!!!!");
            SceneManager.LoadScene ("Sandbox", LoadSceneMode.Single);
        }
        else {
            m_partyMembers = newPartyMembers;
        }
    }
}
