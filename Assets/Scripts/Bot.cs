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
	public RobotEstado estado;

	private Collider _collider;

	public Vector3 siguiente_posicion;

	public int nuevox;
	public int nuevoz;

	public override void Start() {
		 base.Start();
		 Bomba.cuandoSePlanta += this.reaccionarABombas;
		 this._collider = this.GetComponent<Collider>(); 
	}

	public void Update() {
		if (this.estado == RobotEstado.Escapando || this.estado == RobotEstado.Atacando) {
			float step = this.velocidad * Time.deltaTime;
			this.transform.LookAt(this.siguiente_posicion);
        	this.transform.position = Vector3.MoveTowards(transform.position, this.siguiente_posicion, step);
		}
	}

	public void mover(Vector3 nueva_posicion) {
		if (this.estado == RobotEstado.Observando) {
			this.estado = RobotEstado.Escapando;
		}
		this.siguiente_posicion = nueva_posicion;
		this._animator.SetBool("Walking",true);
	}

	void Escapar() {
		Debug.Log("Tengo que escapar!");
	
		Vector3 posicion_segura = Posiciones.BuscarPosicionSegura(this.transform.position);
		if (posicion_segura == Vector3.zero) {
			Debug.Log("NO se encontro posision segura");
		}
		else {
			this.mover(posicion_segura);
			this.estado = RobotEstado.Escapando;
		}
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
