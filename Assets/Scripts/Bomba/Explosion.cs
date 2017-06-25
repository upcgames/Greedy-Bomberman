using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    public float delay = .5f;

    void Start() {
        Destroy(this.gameObject, this.delay);
    }
}
