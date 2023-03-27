// See https://aka.ms/new-console-template for more information
using MessageLogger;

Console.WriteLine("Welcome to Message Logger!");
Console.Write("Add a message (or `quit` to exit): ");

string userInput = Console.ReadLine();
List<Message> messages = new List<Message>();

while (userInput.ToLower() != "quit")
{
    messages.Add(new Message(userInput));
    foreach (var message in messages)
    {
        Console.WriteLine($"{message.CreatedAt:t}: {message.Content}");
    }

    Console.Write("Add a message (or `quit` to exit): ");

    userInput = Console.ReadLine();
}

Console.WriteLine($"Thanks for using Message Logger! During this session you logged {messages.Count} messages");
