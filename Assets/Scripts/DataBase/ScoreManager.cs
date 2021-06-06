using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int level;
    private DBController database;

    void Start()
    {
        database = DBController.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            print("Insert a Random Player");
            InsertRandomPlayerInRanking();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            print("Get Last Player Ranking");
            GetLastPlayerRanking();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            print("Get Ranking Level: " + level);
            GetLevelRanking(level);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            print("Get All Ranking");
            GetAllRanking();
        }
    }

    public void InsertPlayer(string name) //Inserta un player en tabla Player (solo usar en Main Menu) 
    {
        PlayerGlobal.Instance.Name = name; //Seteamos el name del PlayerGlobal 
        Player player = new Player();
        player.Name = name;

        //TODO: DESCOMENTAR EL AGREGADO A DATABASE (cuando se solucione el tema del score en general para no generar 20mil entradas de Players y nada en ranking)
        //database.InsertPlayer(player2); //lo insertamos en la base
        //player.Id = database.GetLastPlayerId(); //Obtenemos el id del ultimo player insertado 
        print(PlayerGlobal.Instance.Name + " " + PlayerGlobal.Instance.Id);
    }

    public void InsertPlayerInRanking() //Esto se haria cuando se termina un nivel
    {
        var player = new Player(PlayerGlobal.Instance.Name, PlayerGlobal.Instance.Level, PlayerGlobal.Instance.Score);
        player.Time = PlayerGlobal.Instance.Time;
        database.InsertRanking(player);
    }

    public void InsertRandomPlayerInRanking() //Genera un player random, lo inserta en player y luego en ranking
    {
        var player = new Player();
        database.InsertPlayer(player); //lo insertamos en la base
        player.Id = database.GetLastPlayerId(); //Obtenemos el id del ultimo player insertado 
        print($"{player.Id} {player.Name} {player.Level} {player.Score} {player.Time}");
        database.InsertRanking(player);
        var ranking = database.GetLatestRanking();
        print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
    }

    private void GetLastPlayerRanking()
    {
        var ranking = database.GetLatestRanking();
        print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
    }

    public void GetLevelRanking(int level)
    {
        var rankings = database.GetAllRankingsFromLevel(level);
        foreach (var ranking in rankings)
        {
            print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
        }
    }

    private void GetAllRanking()
    {
        print("AllRanking");
        var rankings = database.GetAllRankings();
        foreach (var ranking in rankings)
        {
            print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
        }
    }
}
