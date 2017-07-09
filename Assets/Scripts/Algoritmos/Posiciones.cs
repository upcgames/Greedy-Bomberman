using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posiciones : MonoBehaviour {

	public static Vector3 BuscarPosicionSegura(Vector3 comienzo_mundo, bool[,] posiciones_muerte = null) {

		Vector3 comienzo = new Vector3();
		comienzo.x = Mathf.FloorToInt(comienzo_mundo.x);
		comienzo.y = comienzo_mundo.y;
		comienzo.z = Mathf.FloorToInt(comienzo_mundo.z);

		// Arreglo para saber que posiciones seran afectadas por
		if (posiciones_muerte == null) {
			posiciones_muerte = new bool[13, 11];
		}

		posiciones_muerte[(int)comienzo.x, (int)comienzo.z] = true;

		Vector3 activo = comienzo;

		// Consultamos la seguridad de las 4 posiciones adyacentes
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				
				// No exploramos las diagonales
				if (Mathf.Abs(i) == Mathf.Abs(j)) continue;
				
				Vector3 siguiente_posicion = new Vector3(activo.x + i, activo.y,  activo.z + j);
				
				Collider[] objetos = Physics.OverlapSphere(siguiente_posicion, .25f);

				bool choca_con_objeto = false;
	
				// Si la posicion contiene una pared, una bomba o una explosión
				foreach (Collider objeto in objetos) {

					if (objeto.CompareTag("Block") || objeto.CompareTag("Explosion") || objeto.CompareTag("BombCollider")) {
						choca_con_objeto = true;
						break;
					}
				}
				if (choca_con_objeto) continue;

				// No choca con nigun objeto
				// Llega a una posicion libre, pero todavia no se sabe si es segura
				// Hay que validar si la posicion eventualmente será alcanzada por una bomba

				// Esta posicion ya fue detectada como posición que sera afecta por las explosiones
				if (posiciones_muerte[(int)siguiente_posicion.x, (int)siguiente_posicion.z]) continue;

				GameObject[] bombas = GameObject.FindGameObjectsWithTag("Bomb");
				Bomba bomba = bombas[bombas.Length - 1].GetComponent<Bomba>();

				if (
					(bomba.transform.position.z == siguiente_posicion.z && bomba.transform.position.x + bomba.longitud_propagacion >= siguiente_posicion.x) ||
					(bomba.transform.position.z == siguiente_posicion.z && bomba.transform.position.x + bomba.longitud_propagacion <= siguiente_posicion.x) ||
					(bomba.transform.position.x == siguiente_posicion.x && bomba.transform.position.z + bomba.longitud_propagacion >= siguiente_posicion.z) ||
					(bomba.transform.position.x == siguiente_posicion.x && bomba.transform.position.z + bomba.longitud_propagacion <= siguiente_posicion.z)
				)
				{ // Estoy al alcanze de una explosion, no puedo quedarme aca
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
