using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Robot : Jugador {
	
	public LayerMask solidos_capa;
	
	public override void Start() {
		 base.Start();
		 Bomba.cuandoSePlanta += this.reaccionarABombas;
	}
	
	private void reaccionarABombas() {
		GameObject[] bombas = GameObject.FindGameObjectsWithTag("Bomb");
		Bomba bomba = bombas[0].GetComponent<Bomba>();

		RaycastHit hit;
		Ray ray = new Ray(bomba.transform.position, Vector3.back);
		Debug.DrawLine(ray.origin, ray.origin + Vector3.back, Color.red, .5f);

		if (Physics.Raycast(ray, out hit, bomba.longitud_propagacion, solidos_capa)) {
			if (hit.collider.tag == "Player") {
				Debug.Log("¿Alo?, robot al habla! una bomba ha sido plantada!!"+ bombas.Length);
			}
		}
	}
}
