using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posiciones : MonoBehaviour {

	// Si la posicion contiene una pared, una bomba o una explosión
	static bool devuelvecolisionador(Vector3 posicion) {

		Collider[] objetos = Physics.OverlapSphere(posicion, .25f);
		foreach (Collider objeto in objetos) {
			if (objeto.CompareTag("Block") || objeto.CompareTag("Explosion") || objeto.CompareTag("BombCollider")) {
				return true;
			}
		}

		return false;
	}


	public static Vector3 BuscarPosicionSegura(Vector3 comienzo_mundo, bool[,] posiciones_muerte = null) {

		Vector3 comienzo = new Vector3();
		comienzo.x = Mathf.Round(comienzo_mundo.x);
		comienzo.y = comienzo_mundo.y;
		comienzo.z = Mathf.Round(comienzo_mundo.z);

		// Arreglo para saber que posiciones seran afectadas por
		if (posiciones_muerte == null) {
			posiciones_muerte = new bool[13, 11];
		}

		posiciones_muerte[(int)comienzo.x, (int)comienzo.z] = true;

		Vector3 activo = comienzo;

		// Consultamos la seguridad de las 4 posiciones adyacentes
		for (int x = -1; x <= 1; x++) {
			for (int z = -1; z <= 1; z++) {
				
				// No exploramos las diagonales
				if (Mathf.Abs(x) == Mathf.Abs(z)) continue;
				
				Vector3 siguiente_posicion = new Vector3(activo.x + x, activo.y,  activo.z + z);
				
				if (devuelvecolisionador(siguiente_posicion)) continue;

				// No choca con nigun objeto
				// Llega a una posicion libre, pero todavia no se sabe si es segura
				// Hay que validar si la posicion eventualmente será alcanzada por una bomba

				// Esta posicion ya fue detectada como posición que sera afecta por las explosiones
				if (posiciones_muerte[(int)siguiente_posicion.x, (int)siguiente_posicion.z]) continue;

				GameObject[] bombas = GameObject.FindGameObjectsWithTag("Bomb");

				bool peligro_explosion = false;
		
				foreach (GameObject bomba_gameObject in bombas) {
					
					Bomba bomba = bomba_gameObject.GetComponent<Bomba>();
					
					if (bomba.transform.position.z == siguiente_posicion.z && siguiente_posicion.x >= bomba.transform.position.x && bomba.transform.position.x + bomba.longitud_propagacion >= siguiente_posicion.x)
						peligro_explosion = true;
					if (bomba.transform.position.z == siguiente_posicion.z && siguiente_posicion.x <= bomba.transform.position.x  && bomba.transform.position.x - bomba.longitud_propagacion <= siguiente_posicion.x)
						peligro_explosion = true;
					if (bomba.transform.position.x == siguiente_posicion.x && siguiente_posicion.z >= bomba.transform.position.z  && bomba.transform.position.z + bomba.longitud_propagacion >= siguiente_posicion.z)
						peligro_explosion = true;
					if (bomba.transform.position.x == siguiente_posicion.x && siguiente_posicion.z <= bomba.transform.position.z  && bomba.transform.position.z - bomba.longitud_propagacion <= siguiente_posicion.z)
						peligro_explosion = true;
				}

				if (peligro_explosion) { // Estoy al alcanze de una explosion, no puedo quedarme aca

					// Antes de hacer recursivdad, verifica si hay una solucion antes de terminar esta iteracion 
					for (int xx = x; xx <= 1; xx++) {
						for (int zz = z + 1; zz <= 1; zz++) {
							if (Mathf.Abs(xx) == Mathf.Abs(zz)) continue;	
							
							Vector3 siguiente_posicion_acortada = new Vector3(comienzo.x + xx, comienzo.y,  comienzo.z + zz);
							
							if (posiciones_muerte[(int)siguiente_posicion_acortada.x, (int)siguiente_posicion_acortada.z]) continue;
			
							if (devuelvecolisionador(siguiente_posicion_acortada) == false) {
								return siguiente_posicion_acortada;
							}

						}
					}

					Vector3 posicion_backtrack = BuscarPosicionSegura(siguiente_posicion, posiciones_muerte);

					if (posicion_backtrack == Vector3.zero) {
						continue;
					}
					else {
						return posicion_backtrack;
					}
				}
				Debug.Log("Econtré posicion segura");
				return siguiente_posicion;
			}
		}

		// Si no encuentra lugar seguro en esta iteracion
		// Vector3.zero actuara como nuestro null
		return Vector3.zero;
	}
}
