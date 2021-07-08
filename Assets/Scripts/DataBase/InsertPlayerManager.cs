using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertPlayerManager : MonoBehaviour
{
    private DBController database;

    void Start()
    {
        database = DBController.Instance;
    }

    public void InsertPlayer(string name) //Inserta un player en tabla Player (solo usar en Main Menu) 
    {
        PlayerGlobal.Instance.Name = name; //Seteamos el name del PlayerGlobal 
        Player player = new Player();
        player.Name = name;

        database.InsertPlayer(player); //lo insertamos en la base
        PlayerGlobal.Instance.Id = database.GetLastPlayerId(); //Obtenemos el id del ultimo player insertado 
        print(PlayerGlobal.Instance.Name + " " + PlayerGlobal.Instance.Id);
    }

}
