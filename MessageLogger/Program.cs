// See https://aka.ms/new-console-template for more information
using MessageLogger;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

Console.WriteLine("Welcome to Message Logger!");
Console.WriteLine();

//Console.WriteLine("Let's create a user pofile for you.");
//Console.Write("What is your name? ");
string name;

//Console.Write("What is your username? (one word, no spaces!) ");
string username;
User user;

//Console.WriteLine();
//Console.WriteLine("To log out of your user profile, enter `log out`.");

//Console.WriteLine();
//Console.Write("Add a message (or `quit` to exit): ");

string userInput = "";

List<User> users = new List<User>() { };
using (var context = new MessageLoggerContext())
{
    if (context.Users != null)
    {
        users = context.Users.Include(u => u.Messages).ToList();
        Console.WriteLine(users);
    }
}
while (userInput.ToLower() != "quit")
{
    Console.Write("Would you like to log in a `new` or `existing` user?");
    userInput = Console.ReadLine() ?? "";
    if (userInput.ToLower() == "new")
    {
        Console.Write("What is your name? ");
        name = Console.ReadLine() ?? "";
        Console.Write("What is your username? (one word, no spaces!) ");
        username = Console.ReadLine() ?? "";
        user = new User(name, username);
        users.Add(user);

        using (var context = new MessageLoggerContext())
        {
            //add user to database
            context.Users.Add(user);
            context.SaveChanges();
        }

        Console.Write("Add a message: ");

        userInput = Console.ReadLine() ?? "";
    }
    else
    {
        Console.Write("What is your username? ");
        username = Console.ReadLine() ?? "";
        user = null;
        foreach (var existingUser in users)
        {
            if (existingUser.Username == username)
            {
                user = existingUser;
            }
        }

        if (user != null)
        {
            Console.Write("Add a message: ");
            userInput = Console.ReadLine() ?? "";
        }
        else
        {
            Console.WriteLine("could not find user");
            userInput = "quit";
        }
    }

    while (userInput.ToLower() != "log out" && userInput.ToLower() != "quit")
    {
        var newMessage = new Message(userInput);
        user.Messages.Add(newMessage);

        using (var context = new MessageLoggerContext())
        {
            // Retrieve the user from the database based on a condition (e.g., username)
            var userFromDatabase = context.Users.FirstOrDefault(u => u.Username == user.Username);

            if (userFromDatabase != null)
            {
                // Add a new message to the user's message collection
                userFromDatabase.Messages.Add(newMessage);

                // Save changes to the database
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("User not found!");
            }
        }

        foreach (var message in user.Messages)
        {
            Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
        }

        Console.Write("Add a message: ");

        userInput = Console.ReadLine();
        Console.WriteLine();
    }
}

Console.WriteLine("Thanks for using Message Logger!");
foreach (var u in users)
{
    Console.WriteLine($"{u.Name} wrote {u.Messages.Count} messages.");
}
