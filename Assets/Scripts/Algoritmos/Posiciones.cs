using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posiciones : MonoBehaviour {

	public static Vector3 BuscarPosicionSeguro(Vector3 commienzo_mundo) {

		Vector3 commienzo = new Vector3();
		commienzo.x = Mathf.FloorToInt(commienzo_mundo.x);
		commienzo.y = commienzo_mundo.y;
		commienzo.z = Mathf.FloorToInt(commienzo_mundo.z);

		// Arreglo para saber si una posicion ya fue explorada o no
		bool[,] explorados = new bool[13, 11]; 

		// Cola de posiciones por explorar
		Queue<Vector3> por_explorar = new Queue<Vector3>();

		por_explorar.Enqueue(commienzo);

		while (por_explorar.Count != 0) {
			
			// Exploramos al primero de la cola
			Vector3 activo = por_explorar.Dequeue();

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

					if (choca_con_objeto == false) { // Llega a una posicion segura
						Debug.Log("Econtro posicion segura");
						return siguiente_posicion;
					}
					else {
						continue;
					}
					
					// if (explorados[(int)siguiente_posicion.x, (int)siguiente_posicion.z] == true) continue;

					// Si mi posicion en la matriz explorados es true, ya ha sido explorada
					// explorados[(int)siguiente_posicion.x, (int)siguiente_posicion.z] = true;
					// por_explorar.Enqueue(siguiente_posicion);
				}	
			}
		}


		// Si no encuentra lugar seguro...
		Debug.Log("No hay lugar seguro");
		return (commienzo_mundo);
	}
}
