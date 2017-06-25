using UnityEngine;
using System.Collections;
using System;

public enum JugadoresKey {
    Ninguno,
    JugadorA,
    JugadorB,
    JugadorC,
    JugadorD

}

public class Jugador : MonoBehaviour {

    public ControladorGlobal controlador_global;
    //Player parameters
    public bool ya_murio = false;
    public JugadoresKey jugador_key;
    public float velocidad = 5f;

    [Range(1, 6)]
    public int bombas = 2;

    //Prefabs
    public GameObject bomba_prefab;

    // Para el cache
    private Rigidbody _rigibody;
    private Transform _transform;
    private Animator _animator;

    void Start() {
        this._rigibody = GetComponent<Rigidbody>();
        this._transform = this.transform;
        this._animator = this._transform.Find("PlayerModel").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        this.actualizarMovimiento();
    }

    private void actualizarMovimiento() {
        this._animator.SetBool("Walking", false);

        if (this.jugador_key == JugadoresKey.JugadorA) {
            UpdatePlayer1Movement();
        }
    }

    private void UpdatePlayer1Movement() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            this._rigibody.velocity = new Vector3(this._rigibody.velocity.x, this._rigibody.velocity.y, this.velocidad);
            this._transform.rotation = Quaternion.Euler(0, 0, 0);
            this._animator.SetBool("Walking",true);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            this._rigibody.velocity = new Vector3(this.velocidad, this._rigibody.velocity.y, this._rigibody.velocity.z);
            this._transform.rotation = Quaternion.Euler(0, 90, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            this._rigibody.velocity = new Vector3(this._rigibody.velocity.x, this._rigibody.velocity.y, - this.velocidad);
            this._transform.rotation = Quaternion.Euler(0, 180, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            this._rigibody.velocity = new Vector3(- this.velocidad, this._rigibody.velocity.y, this._rigibody.velocity.z);
            this._transform.rotation = Quaternion.Euler(0, 270, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.plantarBomba();
        }
    }

    private void plantarBomba() {
        Instantiate(
            this.bomba_prefab,
            new Vector3(
                Mathf.RoundToInt(this._transform.position.x),
                Mathf.RoundToInt(this._transform.position.y),
                Mathf.RoundToInt(this._transform.position.z)
            ),
            this.bomba_prefab.transform.rotation
        );
    }

    public void OnTriggerEnter(Collider otro) {
        if (otro.CompareTag("Explosion")) {
            this.ya_murio = true;
            this.controlador_global.jugadorMurio(this.jugador_key);
            Destroy(this.gameObject);
        }
    }
}
