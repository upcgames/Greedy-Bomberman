using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum RobotEstado {
	Observando,
	Escapando,
	Atacando
}

public class Robot : Jugador {
	
	private Collider _collider;

	public RobotEstado estado;

	public override void Start() {
		 base.Start();
		 Bomba.cuandoSePlanta += this.reaccionarABombas;
		 this._collider = this.GetComponent<Collider>(); 
	}

	void Escapar() {
		
	}
	
	private void reaccionarABombas() {
		GameObject[] bombas = GameObject.FindGameObjectsWithTag("Bomb");
		Bomba bomba = bombas[bombas.Length - 1].GetComponent<Bomba>();

		RaycastHit golpe;
		if (
			this._collider.Raycast(new Ray(bomba.transform.position, Vector3.forward), out golpe, bomba.longitud_propagacion) ||
			this._collider.Raycast(new Ray(bomba.transform.position, Vector3.right), out golpe, bomba.longitud_propagacion) ||
			this._collider.Raycast(new Ray(bomba.transform.position, Vector3.back), out golpe, bomba.longitud_propagacion) ||
			this._collider.Raycast(new Ray(bomba.transform.position, Vector3.left), out golpe, bomba.longitud_propagacion)
			)
		{
			this.Escapar();
		}
	}
}
