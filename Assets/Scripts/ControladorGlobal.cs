using UnityEngine;
using System.Collections;

public class ControladorGlobal : MonoBehaviour {

    public float tiempo_para_empate = .3f;
    private int _numero_muertos = 0;
    private JugadoresKey _ultimo_muerto = JugadoresKey.Ninguno;

    // Función de Unity que se llama antes de que comience el juego
    // Será la encargada de generar el mapa, matrices y demás.
    // Por eso esta clase se llama "ControladorGlobal"
    public void Awake() {

    }

    // Función que se llama cada vez que un jugador muere
    public void jugadorMurio(JugadoresKey jugadorKey) {
        this._numero_muertos ++;
        this._ultimo_muerto = jugadorKey;
        
        if (this._numero_muertos == 1) {
            Invoke("revisarOtrosMueren", this.tiempo_para_empate);
        }
    }

    // Se hace una revision adicional de quienes más murieron,
    // Porque despues de que muera uno, pueden morir otros, no necesariamente en ese mismo frame
    // Para eso sirve la variable tiempo_para_empate, basicamente es cuanto tiempo vamos a esperar para
    // Saber el resultado final de una serie de explosiones asesinas
    public void revisarOtrosMueren() {
        if (this._numero_muertos == 1) {
            if (this._ultimo_muerto == JugadoresKey.JugadorA) {
                Debug.Log("Ganó Azul!");
            }
            else if (this._ultimo_muerto == JugadoresKey.JugadorB) {
                Debug.Log("Ganó Rojo!");
            }
        }
        else if (this._numero_muertos == 2) {
                Debug.Log("Murieron todos!, nadie ganó!");
        }
    }
}
