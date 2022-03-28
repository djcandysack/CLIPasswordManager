namespace Sql;

public static class Program
{
    private static void Main()
    {
        var passwordManager = new PasswordManager(new DataBaseService());
        passwordManager.Run();
    }
}