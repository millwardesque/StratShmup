using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetComponent<Damageable> ().onDead = OnDead;
	}

    void OnDead() {
        GameObject.Destroy (gameObject);
    }
}
