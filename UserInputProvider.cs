namespace Sql;

public static class UserInputProvider
{
    public static UserActions GetUserInput()
    {
        var xlsx = Enum.GetValues(typeof(UserActions)).Cast<UserActions>().Aggregate<UserActions,
            string>(null!, (current, actions) => current + $"{actions}, ");

        Console.WriteLine($"Would you like to {xlsx}passwords?");

        string input;
        do
        {
            input = Console.ReadLine() ?? "";
        } while (!Enum.TryParse<UserActions>(input, true, out _));

        var tryParse = Enum.TryParse(input, out UserActions action);

        return action;
    }
}