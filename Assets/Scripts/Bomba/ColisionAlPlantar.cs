using UnityEngine;
using System.Collections;

public class ColisionAlPlantar : MonoBehaviour {

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
