namespace Sql.Utilities;

public static class DatabaseUtilities
{
    public static void DisplayDatabase(this IDataBaseService dataBaseService)
    {
        try
        {
            Console.WriteLine("id | service | email | password");
            foreach (var row in dataBaseService.GetAllValues())
            {
                Console.WriteLine($@"{row["id"]} {row["site"]} {row["email"]} {row["password"]}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}