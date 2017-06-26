using UnityEngine;
using System.Collections;
using System;

public class Humano : Jugador {

    void Update() {
        this.actualizarMovimiento();
    }

    private void actualizarMovimiento() {
        this._animator.SetBool("Walking", false);
        UpdatePlayer1Movement();
    }

    private void UpdatePlayer1Movement() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            this._rigibody.velocity = new Vector3(this._rigibody.velocity.x, this._rigibody.velocity.y, this.velocidad);
            this._transform.rotation = Quaternion.Euler(0, 0, 0);
            this._animator.SetBool("Walking",true);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            this._rigibody.velocity = new Vector3(this.velocidad, this._rigibody.velocity.y, this._rigibody.velocity.z);
            this._transform.rotation = Quaternion.Euler(0, 90, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            this._rigibody.velocity = new Vector3(this._rigibody.velocity.x, this._rigibody.velocity.y, - this.velocidad);
            this._transform.rotation = Quaternion.Euler(0, 180, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            this._rigibody.velocity = new Vector3(- this.velocidad, this._rigibody.velocity.y, this._rigibody.velocity.z);
            this._transform.rotation = Quaternion.Euler(0, 270, 0);
            this._animator.SetBool("Walking", true);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.plantarBomba();
        }
    }
}
