namespace JwtSample;

public class JwtSample
{
    public static void Main()
    {
        Console.WriteLine("User Name: ");
        string userName = Console.ReadLine();
        
        Console.WriteLine("Password: ");
        string password = Console.ReadLine();

        UserService userService = new UserService();
        Console.WriteLine(userService.Login(userName, password));

        Console.ReadKey();
    }
}