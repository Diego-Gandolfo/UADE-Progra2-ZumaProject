﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private RankingLineScript[] rankingLine;

    private DBController database;

    private void Awake()
    {
        database = DBController.Instance;
    }

    void Start()
    {
        GetLevelRanking(level);
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

    public void InsertPlayerInRanking(int score, int level, float time) //Esto se haria cuando se termina un nivel
    {
        //PlayerGlobal.Instance.Level = level;
        //PlayerGlobal.Instance.Score = score;
        //PlayerGlobal.Instance.Time = time.ToString();

        var player = new Player(PlayerGlobal.Instance.Name, PlayerGlobal.Instance.Level, PlayerGlobal.Instance.Score);
        player.Id = PlayerGlobal.Instance.Id;
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
        QuickSort(rankings, 0, rankings.Count - 1);
        UpdateRankingList(rankings);

        CheckCurrentPlayerInRanking(rankings, PlayerGlobal.Instance.RankingId);
    }

    private void GetAllRanking()
    {
        var rankings = database.GetAllRankings();
        QuickSort(rankings, 0, rankings.Count - 1);
        UpdateRankingList(rankings);
    }

    private void UpdateRankingList(List<Player> players)
    {
        for (int i = 0; i < rankingLine.Length; i++)
        {
            var index = (players.Count - i) - 1;
            rankingLine[i].SetNickname(players[index].Name);
            rankingLine[i].SetScore(players[index].Score);
            rankingLine[i].SetTime(players[index].Time);
        }
    }

    private void CleanRankingList()
    {
        string clean = "-";

        for (int i = 0; i < rankingLine.Length; i++)
        {
            rankingLine[i].SetNickname(clean);
            rankingLine[i].SetScore(int.Parse(clean));
            rankingLine[i].SetTime(clean);
        }
    }

    private int Partition(List<Player> arr, int left, int right)
    {
        int pivot;
        int aux = (left + right) / 2;
        pivot = arr[aux].Score;

        while (true)
        {
            while (arr[left].Score < pivot)
            {
                left++;
            }
            while (arr[right].Score > pivot)
            {
                right--;
            }
            if (left < right)
            {
                Player temp = arr[right];
                arr[right] = arr[left];
                arr[left] = temp;
            }
            else
            {
                return right;
            }
        }
    }

    private void QuickSort(List<Player> arr, int left, int right)
    {
        int pivot;
        if (left < right)
        {
            pivot = Partition(arr, left, right);
            if (pivot > 1)
            {
                // mitad del lado izquierdo del vector
                QuickSort(arr, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                // mitad del lado derecho del vector
                QuickSort(arr, pivot + 1, right);
            }
        }
    }

    private void CheckCurrentPlayerInRanking(List<Player> players, int id)
    {
        print("chequeamos ranking id: " + PlayerGlobal.Instance.RankingId);
        bool isPlayerThere = false;
        for (int i = 0; i < rankingLine.Length; i++) //El largo del ranking
        {
            var index = (players.Count - i) - 1; //No se porque, esto lo hizo Diego
            if (players[index].RankingId == id) //Si el ID del player en este puesto es igual al player
            {
                //TODO: Hacele un efecto a la caja o algo para resaltar su posicion
                isPlayerThere = true;
                print($"SI ESTABA: PUESTO: {index} DATOS: {players[index].Id} {players[index].Name} {players[index].Score}");
            }
        }

        print(isPlayerThere);
        if (!isPlayerThere) //Si el player no esta en el TOP 5, entonces recorre TODA la lista de ese nivel 
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].RankingId == id)
                {
                    //TODO: Agregalo en una caja extra abajo, mostrando su posicion

                    print($"NO ESTA: PUESTO: {i} DATOS: {players[i].Id} {players[i].Name} {players[i].Score}");

                    i = players.Count;//Frenamos el for
                }
            }
        }

        print("llegamos al final");
    }

}
