using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DBController : MonoBehaviour
{
    static public DBController instance;
    private string path;
    private IDbConnection connection;

    public void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        instance = this;

        path = $"URI=file:{Application.dataPath}/dbZuma.s3db";

        CreateTablePlayer(); //Si o si tiene que ir Player primero
        CreateTableRanking(); //Porque Ranking tiene una llave foranea de Player
    }

    #region DATABASE_COMUNICATION

    private void StablishConnection()
    {
        connection = new SqliteConnection(path);
    }

    private void SetQueries(string query)
    {
        try
        {
            StablishConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteReader();

            command.Dispose();
            command = null;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
        finally
        {
            connection.Close();
            connection = null;
        }
    }

    private List<string> GetQueries(string query, int atributesCount)
    {
        var list = new List<string>();

        try
        {
            StablishConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string tupla = string.Empty;

                for (int i = 0; i < atributesCount; i++)
                {
                    tupla += $"{reader.GetValue(i).ToString()}/";
                }

                list.Add(tupla);
            }

            command.Dispose();
            command = null;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
        finally
        {
            connection.Close();
            connection = null;
        }

        return list;
    }

    private IDataReader GetQueries2(string query)
    {
        IDataReader reader;

        try
        {
            StablishConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            reader = command.ExecuteReader();

            command.Dispose();
            command = null;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
        finally
        {
            connection.Close();
            connection = null;
        }

        return reader;
    }

    #endregion

    #region SET_TYPE_QUERIES

    public void CreateTablePlayer()
    {
        string query = "CREATE TABLE IF NOT EXISTS Player (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "Name VARCHAR(75) NOT NULL)";
        SetQueries(query);
    }

    public void CreateTableRanking()
    {
        string query = "CREATE TABLE IF NOT EXISTS Ranking (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "Id_Player INTEGER NOT NULL," +
            "Level INTEGER NOT NULL," +
            "Score INTEGER NOT NULL," +
            "Time VARCHAR(10) NOT NULL," +
            "FOREIGN KEY(Id_Player) REFERENCES Player(Id))";
        SetQueries(query);
    }

    public void InsertRanking(Player player)
    { 
        string query = "INSERT INTO Ranking (ID_Player, Level, Score, Time) " +
                $"VALUES('{player.Name}', '{player.Level}', '{player.Score}')";
        SetQueries(query);
    }

    public void InsertPlayer(Player player)
    {
        string query = "INSERT INTO Player (Name) " +
                $"VALUES('{player.Name}')";
        SetQueries(query);
    }

    public void DeleteRanking(Player player)
    {
        string query = $"DELETE FROM Ranking WHERE ID = {player.Id}";
        SetQueries(query);
    }

    #endregion

    #region GET_TYPE_QUERIES

    public List<Player> GetAllRankingsFromLevel(int level)
    {
        var playerList = new List<Player>();
        var query = "SELECT Player.Name, Ranking.Level, Ranking.Score, Ranking.Time" +
                    "FROM Ranking INNER JOIN Player ON Player.ID = Ranking.ID_Player" +
                    $"WHERE Ranking.Level = {level}";
        var list = GetQueries(query, 4);

        foreach (var tupla in list)
        {
            var values = tupla.Split('/');

            var player = new Player(values[0], int.Parse(values[1]), int.Parse(values[2]));
            player.Time = values[3];
            playerList.Add(player);
        }

        return playerList;
    }

    //public Player GetRankingById(int id)
    //{
    //    var playerList = new List<Player>();
    //    Player player = null;

    //    var query = $"SELECT Id, Name, Level, Score FROM Ranking WHERE Id = {id}";
    //    var list = GetQueries(query, 4);

    //    foreach (var tupla in list)
    //    {
    //        var values = tupla.Split('/');

    //        player = new Player(values[1], int.Parse(values[2]), int.Parse(values[3]));
    //        player.Id = int.Parse(values[0]);
    //    }

    //    return player;
    //}

    public int GetLastPlayerId() 
    {
        var query = $"SELECT ID FROM Player ORDER BY ID DESC LIMIT 1;"; //Devuelve el ID del ultimo argegado a la lista como int
        var list = GetQueries(query, 1); //Es solo el id, por eso un solo atributo
        var values = list[0].Split('/'); //pero igual le tenemos que sacar la barra que viene de base
        return int.Parse(values[0]); //y lo convertimos a int.
    }

    public Player GetLatestRanking()
    {
        var playerList = new List<Player>();
        Player player = null;

        var query = $"SELECT Player.Name, Ranking.Level, Ranking.Score, Ranking.Time FROM Ranking" + 
                $"INNER JOIN Player ON Player.ID = Ranking.ID_Player ORDER BY Ranking.Id DESC LIMIT 1;";
        var list = GetQueries(query, 4);

        foreach (var tupla in list)
        {
            var values = tupla.Split('/');

            player = new Player(values[0], int.Parse(values[1]), int.Parse(values[2]));
            player.Time = values[3].ToString();
        }

        return player;
    }

    #endregion
}

