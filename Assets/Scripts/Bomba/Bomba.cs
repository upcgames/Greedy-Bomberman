using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour {
    public GameObject explosion_prefab;
    public LayerMask solidos_capa;

    public float tiempo_para_destruccion = 3f;
    public float tiempo_desaparece = .3f;
    public float velocidad_propragacion = .05f;

    public int longitud_propagacion = 2;

    private bool _ha_explotado = false;

    // Eventos
    public delegate void onBombaAction();
	public static event onBombaAction cuandoSePlanta;    

    void Start() {
        Invoke("Explota", this.tiempo_para_destruccion);

        if (cuandoSePlanta != null) {
            cuandoSePlanta();
        }
    }
    
    void OnTriggerEnter(Collider colisionador) {
        if (colisionador.CompareTag("Explosion") && this._ha_explotado == false) {
            CancelInvoke("Explota");
            Explota();
        }
    }

    void Explota() {
        Instantiate(this.explosion_prefab, this.transform.position, Quaternion.identity);

        StartCoroutine(crearExplosion(Vector3.forward));
        StartCoroutine(crearExplosion(Vector3.right));
        StartCoroutine(crearExplosion(Vector3.back));
        StartCoroutine(crearExplosion(Vector3.left));

        GetComponent<MeshRenderer>().enabled = false;
        _ha_explotado = true;
        this.transform.Find("Collider").gameObject.SetActive(false);
        Destroy(this.gameObject, this.tiempo_desaparece);
    }

    IEnumerator crearExplosion(Vector3 direccion) {
        
        for (int distancia = 1; distancia <= this.longitud_propagacion; distancia++) {
            RaycastHit golpe;

            Physics.Raycast(
                this.transform.position + new Vector3(0,.5f,0),
                direccion,
                out golpe,
                distancia,
                this.solidos_capa
            );
            
            if (golpe.collider == false) {
                Instantiate(
                    this.explosion_prefab,
                    this.transform.position + distancia * direccion,
                    this.explosion_prefab.transform.rotation
                );
            } 
            else {
                break;
            }

            yield return new WaitForSeconds(this.velocidad_propragacion);
        }
    }
}