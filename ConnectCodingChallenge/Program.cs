using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.Text;

namespace ConnectCodingChallenge;

public class Program
{
    public static void Main(string[] args)
    {
        var client = new MongoClient("mongodb+srv://interview-user:mHDdJA3Nh59q9i@staging-wskil.mongodb.net/connect-stage?ssl=true&authSource=admin&retryWrites=true&w=majority");
        var db = client.GetDatabase("connect-stage");
        var users = GetUsersCollection(db);

        int option;
        do
        {
            option = GetOption();

            switch (option)
            {
                case 1:
                    AddUser(db);
                    users = GetUsersCollection(db);
                    break;
                case 2:
                    Console.WriteLine(CreateUsersString(users));
                    Console.WriteLine("Number of users: " + users.Count + "\r\n\r\n");
                    break;
                case 3:
                    WriteToCSV(users);
                    break;
                case 4:
                    return;
                default:
                    break;
            }

        } while (option != 4);
    }

    private static List<BsonDocument> GetUsersCollection(IMongoDatabase db)
    {
        return db.GetCollection<BsonDocument>("interview-users").Find(new BsonDocument()).ToList();
    }

    private static void WriteToCSV(List<BsonDocument> users)
    {
        string path = (Path.Combine(Directory.GetCurrentDirectory() + @"\users_list.csv"));
        using (var writer = File.CreateText(path))
        {
            foreach (var user in users)
            {
                writer.WriteLine(user);
            }
        }

        Console.WriteLine("Users written to CSV");
    }

    private static string CreateUsersString(List<BsonDocument> users)
    {
        StringBuilder sb = new();
        foreach (var user in users)
        {
            var userDetails = $"{user.GetValue("firstName")} {user.GetValue("lastName")} {user.GetValue("phone")}";
            sb.AppendLine(userDetails);
        }

        return sb.ToString();
    }

    private static int GetOption()
    {
        Console.WriteLine("Choose an option (1-4)");
        Console.WriteLine("1 - Add User");
        Console.WriteLine("2 - Write users to console");
        Console.WriteLine("3 - Write users to csv file");
        Console.WriteLine("4 - Quit");
        var option = Console.ReadLine();
        Console.WriteLine("\r\n");

        int returnOption = 0;
        if (int.TryParse(option, out returnOption) && returnOption > 0 && returnOption <= 4)
        {
            return returnOption;
        }

        Console.WriteLine($"{option} is invalid. Enter an option 1 - 4");
        return GetOption();
    }

    static void AddUser(IMongoDatabase db)
    {
        Console.Write("Enter your first name: ");
        var firstName = Console.ReadLine();
        Console.Write("Enter your last name: ");
        var lastName = Console.ReadLine();

        var user = new BsonDocument
        {
            { "firstName", firstName },
            { "lastName", lastName },
            { "phone", "123 123-123" },
            { "displayName", "test-token"},
            { "email", "token.test@gmail.com" },
            { "role" , "member" },
            { "affiliateId", "61a1125aeff6eba6415ff9f1" },
            { "referredBy", "dpz7gADsc0pOSufi1xzjg" },
            { "language", "en" },
            { "firebaseUid", "vVefqVH3G0bJvWzV4aMIHSWtLcm1" },
            { "permissions", "" },
            { "walletAddresses", "" },
            { "number", "001925" },
            { "currency", "USD" },
            { "profilePhotoUrl", "" },
            { "utmInfo", "" },
            { "lastLogin", "" },
            { "unsubscriptions", "" },
            { "communicationConsent", "" }, 
            { "wallet", "" },
};

        var usersCollection = db.GetCollection<BsonDocument>("interview-users");
        try
        {
           usersCollection.InsertOne(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Insert user failed. {ex.Message}");
            return;
        }

        Console.WriteLine("User successfully added");
        Console.Write(user);
    }
}
