using System;

class Player
{

    public string name;
    public char symbol;
    public string color;

    //Constructor
    public Player() { }

    //constructor with parameters
    public Player(string playerName, char id, string cl)
    {
        name = playerName;
        symbol = id;
        color = cl;
    }

    //Copy constructor
    public Player(Player playerInfo)
    {
        name = playerInfo.name;
        symbol = playerInfo.symbol;
        color = playerInfo.color;
    }
}