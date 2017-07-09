using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum RobotEstado {
	Observando,
	Escapando,
	Atacando
}

public class Bot : Jugador {
	
	private Collider _collider;

	public RobotEstado estado;

	public override void Start() {
		 base.Start();
		 Bomba.cuandoSePlanta += this.reaccionarABombas;
		 this._collider = this.GetComponent<Collider>(); 
	}

	void Escapar() {
		Debug.Log("Tengo que escapar!");
	
		Vector3 posicion_segura = Posiciones.BuscarPosicionSeguro(this.transform.position);
		this.transform.position = posicion_segura;
	}

	// Funcion que se llama cada vez aue se planta una bomba,
	// Nos sirve para saber si el bot esta en peligro
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
		{ // Si estamos al alcance de una explision
			this.Escapar();
		}
	}

	// Cuando se muera este bot debemos tambien,
	// desuscribir sus eventos
	private void OnDestroy() {
		Bomba.cuandoSePlanta -= this.reaccionarABombas;
	}
}
