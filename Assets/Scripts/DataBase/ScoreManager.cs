using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int level;
    private DBController database;

    [SerializeField] private Text[] textNameUI;
    [SerializeField] private Text[] textLevelUI;
    [SerializeField] private Text[] textScoreUI;

    void Start()
    {
        database = DBController.Instance;
    }

    private void Awake() // TODO: el Awake es temporal para probar el QuickSort
    {
        // chequeamos que los tamaños de los 3 Arrays sean iguales
        if (textNameUI.Length != textLevelUI.Length || textNameUI.Length != textScoreUI.Length)
            Debug.LogError("Los tamaños de los Arrays del Canvas no coinciden");

        var listPlayers = new List<Player>();

        // llenamos la lista con player random
        for (int i = 0; i < textNameUI.Length; i++)
        {
            var player = new Player();
            listPlayers.Add(player);
        }

        // hacemos el QuickSort de la Lista y le indicamos cual es el primer y ultimo elemento que tiene que ordenar
        QuickSort(listPlayers, 0, listPlayers.Count - 1);

        // para ver si se cargo bien la lista
        foreach (var player in listPlayers) 
            print($"Name: {player.Name} / Level: {player.Level} / Score: {player.Score}");

        // actualizamos los Text del Canvas
        for (int i = 0; i < listPlayers.Count; i++)
        {
            var index = (listPlayers.Count - i) - 1;
            textNameUI[i].text = listPlayers[index].Name;
            textLevelUI[i].text = listPlayers[index].Level.ToString();
            textScoreUI[i].text = listPlayers[index].Score.ToString();
        }
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
}
