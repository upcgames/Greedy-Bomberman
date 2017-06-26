using UnityEngine;
using System.Collections;

public class ControladorGlobal : MonoBehaviour {

    public float tiempo_para_empate = .3f;
    private int _numero_muertos = 0;
    private JugadoresKey _ultimo_muerto = JugadoresKey.Ninguno;

    public void jugadorMurio(JugadoresKey jugadorKey) {
        this._numero_muertos ++;
        this._ultimo_muerto = jugadorKey;
        
        if (this._numero_muertos == 1) {
            Invoke("revisarOtrosMueren", this.tiempo_para_empate);
        }
    }
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
