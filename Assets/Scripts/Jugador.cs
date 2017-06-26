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
    protected Rigidbody _rigibody;
    protected Transform _transform;
    protected Animator _animator;

    // Eventos
    public delegate void onBombaAction();
	public static event onBombaAction cuandoPlantanBombas;

   public virtual void Start() {
        this._rigibody = GetComponent<Rigidbody>();
        this._transform = this.transform;
        this._animator = this._transform.Find("PlayerModel").GetComponent<Animator>();
    }

    protected void plantarBomba() {
        Instantiate(
            this.bomba_prefab,
            new Vector3(
                Mathf.RoundToInt(this._transform.position.x),
                Mathf.RoundToInt(this._transform.position.y),
                Mathf.RoundToInt(this._transform.position.z)
            ),
            this.bomba_prefab.transform.rotation
        );

        if (cuandoPlantanBombas != null) {
            cuandoPlantanBombas();
        }
    }

    protected void OnTriggerEnter(Collider otro) {
        if (otro.CompareTag("Explosion")) {
            this.ya_murio = true;
            this.controlador_global.jugadorMurio(this.jugador_key);
            Destroy(this.gameObject);
        }
    }
}
