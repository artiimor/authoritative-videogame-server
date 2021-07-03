using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Mono.Data.Sqlite;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

public class DatabaseManager
{
    private static string conn = "URI=file:" + Application.dataPath + "/Database.s3db";

    private static string salt = "asuid";

    private static char[] SQLWhiteListArray = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', // Lowercase
                                        'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'ñ',
                                        'z', 'x', 'c', 'v', 'b', 'n', 'm',
                                        'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', // Uppercase
                                        'A', 'S','D', 'F', 'G', 'H', 'J', 'K', 'L', 'Ñ',
                                        'Z', 'X', 'C', 'V', 'B', 'N', 'M',
                                        '-', '_', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'}; // Special symbols
    private static List<char> SQLWhiteList = new List<char>(SQLWhiteListArray);

    public static bool insertUser(string _username, string _email, string _password)
    {
        /*Check white list*/
        foreach (char c in _username)
        {
            if (!SQLWhiteList.Contains(c))
            {
                return false;
            }
        }

        foreach (char c in _email)
        {
            if (!SQLWhiteList.Contains(c) && c != '@' && c != '.')
            {
                return false;
            }
        }

        foreach (char c in _password)
        {
            if (!SQLWhiteList.Contains(c))
            {
                return false;
            }
        }
        Debug.Log("Lista blanca revisada");

        /*Lest check this*/
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = $"INSERT INTO user_data (username, email, password) VALUES(@user, @email, @password);";
        dbcmd.CommandText = sqlQuery;

        var parameterUsername = dbcmd.CreateParameter();
        parameterUsername.ParameterName = "@user";
        parameterUsername.Value = _username;

        var parameterEmail = dbcmd.CreateParameter();
        parameterEmail.ParameterName = "@email";
        parameterEmail.Value = _email;

        var parameterPassword = dbcmd.CreateParameter();
        parameterPassword.ParameterName = "@password";
        parameterPassword.Value = cypherPassword(_password + salt);

        dbcmd.Parameters.Add(parameterUsername);
        dbcmd.Parameters.Add(parameterEmail);
        dbcmd.Parameters.Add(parameterPassword);

        dbcmd.Prepare();

        int _rowsAffected = dbcmd.ExecuteNonQuery();

        return (_rowsAffected == 1); // true if there is one field, otherwise false
    }


    public static bool authenticateUser(string _username, string _password)
    {
        bool _return;

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM user_data WHERE username = @user AND password = @password ;";
        dbcmd.CommandText = sqlQuery;

        var parameterUsername = dbcmd.CreateParameter();
        parameterUsername.ParameterName = "@user";
        parameterUsername.Value = _username;

        var parameterPassword = dbcmd.CreateParameter();
        parameterPassword.ParameterName = "@password";
        parameterPassword.Value = cypherPassword(_password + salt);

        dbcmd.Parameters.Add(parameterUsername);
        dbcmd.Parameters.Add(parameterPassword);

        dbcmd.Prepare();

        IDataReader reader = dbcmd.ExecuteReader();

        _return = reader.Read();        

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return _return; // true if there is one field, otherwise false
        
    }

    private static string cypherPassword(string _password)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(_password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            Debug.Log(_password + " => " + sb.ToString());
            return sb.ToString();
        }
    }
}
