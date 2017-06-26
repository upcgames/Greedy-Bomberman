using UnityEngine;
using System.Collections;
using System;

public class Robot : Jugador {
    
	public override void Start() {
		 base.Start();
		 Jugador.cuandoPlantanBombas += reaccionarABombas;
	}
	
	private void reaccionarABombas() {
		Debug.Log("¿Alo?, robot al habla! una bomba ha sido plantada!!");
	}
}
