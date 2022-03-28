using System.Collections.Specialized;
using System.Data.SQLite;

namespace Sql;

public class DataBaseService : IDataBaseService
{
    private static readonly string Cs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.sqlite3");
    private readonly SQLiteConnection _connection = new($"Data Source={Cs};");

    public bool CreateDatabase()
    {
        if (File.Exists(Cs)) return File.Exists(Cs);
        SQLiteConnection.CreateFile(Cs);
        _connection.Open();
        using var cmd = new SQLiteCommand(_connection);
        
        
        
        cmd.CommandText = @"CREATE TABLE passwords(id INTEGER PRIMARY KEY, site TEXT, email TEXT, password TEXT)";
        cmd.ExecuteNonQuery();

        _connection.Close();

        return File.Exists(Cs);
    }

    public bool AddDatabase()
    {
        string password = null!;

        Console.WriteLine("Enter login site...");
        var site = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter your email...");
        var email = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Do you want to generate a password or enter your own? (z/x)");

        if (Console.ReadKey(true).Key == ConsoleKey.Z)
        {
            string length;
            do
            {
                Console.WriteLine("How long of a password do you want?");

                length = Console.ReadLine() ?? "";
            } while (!int.TryParse(length, out _));

            password = RandomPasswordProvider.GetRandomPassword(int.Parse(length));
            Console.WriteLine("Generating...");
        }
        else if (Console.ReadKey(true).Key == ConsoleKey.X)
        {
            Console.WriteLine("Enter your password...");
            password = Console.ReadLine() ?? string.Empty;
        }

        _connection.Open();
        using var cmd = new SQLiteCommand(_connection);

        cmd.CommandText = "INSERT INTO passwords(site, email, password) VALUES(@site, @email, @password)";

        cmd.Parameters.AddWithValue("@site", $"{site}");
        cmd.Parameters.AddWithValue("@email", $"{email}");
        cmd.Parameters.AddWithValue("@password", $"{password}");
        cmd.Prepare();

        var cond = cmd.ExecuteNonQuery();

        _connection.Close();

        return cond switch
        {
            1 => true,
            0 => false,
            _ => true
        };
    }

    public bool EditDatabase(int rowId, string? username, string? service, string? password) 
    {
        _connection.Open();
        using var cmd = new SQLiteCommand(_connection);

        if (username == null && service == null && password == null)
        {
            throw new NullReferenceException("you must set at least one of the parameters to something");
        }

        cmd.CommandText = "UPDATE passwords SET ";
        var hasSecondParameter = false;

        if (username != null)
        {
            cmd.CommandText += $"email = '{username}'";
            hasSecondParameter = true;
        }

        if (hasSecondParameter)
        {
            cmd.CommandText += ", ";
            hasSecondParameter = false;
        }

        if (service != null)
        {
            cmd.CommandText += $"site = '{service}'";
            hasSecondParameter = true;
        }

        if (hasSecondParameter)
        {
            cmd.CommandText += ", ";
        }

        if (password != null) 
        {
            cmd.CommandText += $"password = '{password}'";
        }

        cmd.CommandText += $" WHERE id = {rowId};";

        var cond = cmd.ExecuteNonQuery();
        
        _connection.Close();
        return cond switch
        {
            1 => true,
            0 => false,
            _ => true
        };
    }

    public bool DeleteDatabase()
    {
        Console.WriteLine("Which row do you want to remove?");
        string removeRow;
        do
        {
            removeRow = Console.ReadLine() ?? "";
        } while (!int.TryParse(removeRow, out _));

        Console.WriteLine("Are you sure? 'y'");
        var cki = Console.ReadKey(true);

        if (cki.Key.ToString() != "Y") return false;
        _connection.Open();
        using var cmd = new SQLiteCommand(_connection);
        cmd.CommandText = "DELETE FROM passwords WHERE id = @RemoveRow;";

        cmd.Parameters.AddWithValue("@removeRow", $"{removeRow}");
        cmd.Prepare();
        
        

        var cond = cmd.ExecuteNonQuery();

        _connection.Close();

        return cond switch
        {
            1 => true,
            0 => false,
            _ => true
        };
    }

    public IEnumerable<NameValueCollection> GetAllValues()
    {
        _connection.Open();

        const string stm = "SELECT * FROM passwords";
        using var cmd = new SQLiteCommand(stm, _connection);
        using var rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            yield return rdr.GetValues();
        }
        _connection.Close();
    }
}