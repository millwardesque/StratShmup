using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player Party") {
            Debug.Log ("YOU WIN!");
            SceneManager.LoadScene ("Sandbox");
        }
    }
}
