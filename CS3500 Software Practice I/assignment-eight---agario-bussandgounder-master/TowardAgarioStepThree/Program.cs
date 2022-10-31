// See https://aka.ms/new-console-template for more information
using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using System.Text.Json;
using TowardAgarioStepThree;

Console.WriteLine("Hello, World!");

Networking? channel = new Networking(NullLogger.Instance, onConnect, onDisconnect, onMessage, '\n');

channel.Connect("localhost", 11000);

channel.ClientAwaitMessagesAsync(true);

//will wait for "enter" key to be pressed before disconnecting - blocking method
Console.ReadLine();

channel.Disconnect();

void onConnect(Networking network)
{

}

void onDisconnect(Networking network)
{

}

void onMessage(Networking network, string message)
{
    //string deserializerString = JsonSerializer.Deserialize<String>(message);
    Console.WriteLine(message);
    //if (message.StartsWith(Protocols.CMD_Food))
    //{
    //    string substring = message.Substring(Protocols.CMD_Food.Length);
    //    List<FoodStep3> deserializedFood = JsonSerializer.Deserialize<List<FoodStep3>>(substring);


    //    //string[] deserializedFoodArr = deserializedFood.ToArray();

    //    for (int i = 0; i < 10; i++)
    //    {
    //        Console.WriteLine($"Food #{i+1}: "+deserializedFood[i].toString());
    //    }
    //}
}
