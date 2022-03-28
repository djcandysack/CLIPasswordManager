using Sql.Utilities;
namespace Sql;
public class PasswordManager
{
    private readonly IDataBaseService _dataBaseService;

    public PasswordManager(IDataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }

    public void Run()
    {
        try
        {
            var value = _dataBaseService.CreateDatabase();
            Console.WriteLine(value
                ? "Database OK"
                : "Failed to generate or locate database");
            do
            {
                var action = UserInputProvider.GetUserInput();
                bool cond;
                switch (action)
                {
                    case UserActions.add:
                        cond = _dataBaseService.AddDatabase();
                        Console.WriteLine(cond ? "Login added" : "Failed to add login");
                        break;
                    case UserActions.read:
                        _dataBaseService.DisplayDatabase();
                        break;
                    case UserActions.delete:
                        _dataBaseService.DisplayDatabase();
                        cond = _dataBaseService.DeleteDatabase();
                        Console.WriteLine(cond ? "Login deleted" : "Failed to delete login");
                        break;
                    case UserActions.edit:
                        _dataBaseService.DisplayDatabase();
                        
                        Console.WriteLine("Give row you want to edit");
                        var id = Convert.ToInt32(Console.ReadLine());
                        
                        Console.WriteLine("Give new login email (leave blank to not edit this field)");
                        var un = Console.ReadLine();
                        
                        Console.WriteLine("Give new login service (leave blank to not edit this field)");
                        var ss = Console.ReadLine();
                        
                        Console.WriteLine("Give new login password (leave blank to not edit this field)");
                        var pw = Console.ReadLine();
                        
                        cond = _dataBaseService.EditDatabase(id, un, ss, pw);
                        Console.WriteLine(cond ? "Login edited" : "Failed to edit login or user cancelled");
                        break;
                    case UserActions.gen:
                        string length;
                        Console.WriteLine("How long of a password do you want?");
                        do
                        {
                            length = Console.ReadLine() ?? "";
                        } while (!int.TryParse(length, out _));

                        Console.WriteLine("Your password: " +
                                          RandomPasswordProvider.GetRandomPassword(Convert.ToInt32(length)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}