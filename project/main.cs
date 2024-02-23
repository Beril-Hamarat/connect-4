using System;
using System.Collections.Generic;

class Connect4
{
    static void Main()
    {

        bool win = false; //a boolean to check if the player has won or not
        bool full = false; //a boolean to check if the board is full
        int again = 0; //integer to check if the user wants to restart the game or not
        int dropChoice; //holds the dropChoice of the activePlayer
        int level; //holds the chosen Difficulty
        char[,] board = new char[10, 10]; //matrix of chars


        //------------welcome to the game:------------------------------
        Console.BackgroundColor = ConsoleColor.DarkMagenta; //change background color
        Console.WriteLine("Welcome to Connect #!");
        Console.WriteLine();
        Console.ResetColor(); //resets bgcolor to default

        //Choose Difficulty
        level = chooseDifficulty();
        Console.Clear(); //clear console

        // Create a List of symbols for the player to choose from   
        char[] symbols = { 'X', 'O', 'Y', 'Z' };
        List<char> symbolsList = new List<char>(symbols);

        // Create a List of colors for the player to choose from   
        string[] colors = { "Red", "Blue", "Green", "Yellow" };
        List<string> colorsList = new List<string>(colors);

        //print difficulty and rules
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        if (level == 0) 
            Console.WriteLine("EASY, Let's Play Connect3!");
        else if (level == 1)
            Console.WriteLine("NORMAL, Let's Play Connect4!");
        else
            Console.WriteLine("HARD, Let's Play Connect5!");
        Console.ResetColor();

        int l = level + 3; //connect3 / connect4 / connect5
        Console.WriteLine();
        Console.WriteLine("Connect " + l + " of the same colored symbol horizontally, vertically, or diagonally to win!");
        Console.WriteLine();
        Console.WriteLine("Press Enter to start playing...");
        Console.ReadLine();
        Console.Clear();


        //---------------Get Players details:------------------------

        //player1 info
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("PLAYER ONE ");
        Console.WriteLine();
        Console.ResetColor();
        Player player1 = getDetails(symbolsList, colorsList); //get he player's chosen symbol and color
        Console.Clear();

        //player2 info
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("PLAYER TWO ");
        Console.WriteLine();
        Console.ResetColor();
        Player player2 = getDetails(symbolsList, colorsList);
        Console.Clear();

   

        //---------------Main Game Loop-----------------------------


        //keep repeating until the user decides to end the game
        do
        {
            //display the board
            DisplayBoard(board, player1, player2);

            //first player's turn
            dropChoice = PlayerDrop(board, player1, player2); //get the drop choice of first player
            CheckBellow(board, player1, dropChoice, player2); //check the drop-choice of first player
            DisplayBoard(board, player1, player2); //display the board after the selection made by first player

            //check if the player will win according to the chosen difficulty level
            if (level == 0) //easy
                win = CheckThree(board, player1, dropChoice); //check if player1 collected 3
            else if (level == 1) //normal
                win = CheckFour(board, player1,dropChoice); //check if player1 collected 4
            else //hard
                win = CheckFive(board, player1, dropChoice); //check if player1 collected 5

            if (win) //if win == true
            {
                PlayerWin(player1); //announce the winner
                again = restart(board); //check if the user wants to restart game
                if (again != 1)
                    break; //exit the while loop and stop the game 
                else
                    continue; //skip the following lines of code and restart loop 
            }

            //second player's turn
            dropChoice = PlayerDrop(board, player2, player1);
            CheckBellow(board, player2, dropChoice, player1);
            DisplayBoard(board, player1, player2);

            if (level == 0)
                win = CheckThree(board, player2, dropChoice); 
            else if (level == 1)
                win = CheckFour(board, player2, dropChoice); 
            else
                win = CheckFive(board, player2, dropChoice); 

            if (win)
            {
                PlayerWin(player2);
                again = restart(board);
                if (again != 1)
                    break;
                else
                    continue;
            }

            //check if the board became full
            full = FullBoard(board);
            if (full)
            {
                Console.WriteLine();
                Console.WriteLine("The board is full, it is a draw!");
                Console.WriteLine();
                again = restart(board);
            }

        } while (again != 2); //keep repeating until user enters 2

    }



    //----------------------FUNCTIONS-----------------------------

    //choose difficulty:
    static int chooseDifficulty()
    {
        // Create an array of difficulty levels for the player to choose from   
        string[] difficulty = { "Easy", "Normal", "Hard" };
        string level; //stores user's choice as a string
        int levelChoice; //stores the choice as an integer

        do
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Choose Difficulty: ");
            //print difficulty levels
            for (int i = 0; i < difficulty.Length; i++)
            {
                if (i == 0)
                    Console.ForegroundColor = ConsoleColor.Cyan; //easy
                if (i == 1)
                    Console.ForegroundColor = ConsoleColor.Yellow; //normal
                if (i == 2)
                    Console.ForegroundColor = ConsoleColor.Red; //hard

                Console.Write(difficulty[i] + "(" + (i + 1) + ") ");
            }
            Console.ResetColor();
            level = Console.ReadLine(); //get user input

            //repeat if the user makes invalid entry
            //tryparse: Convert a string representation of number to an integer, If the string cannot be converted, then the int.TryParse method returns false
        } while (level.Length < 1 || int.TryParse(level, out levelChoice) == false || levelChoice < 1 || levelChoice > difficulty.Length);

        levelChoice--;

        return levelChoice; 
    }


    //Get player name and symbol
    static Player getDetails(List<char> symbols, List<string> colors)
    {

        string name;
        string sm;
        int smChoice;
        string cl;
        int clChoice;
        char sym;

        Console.ForegroundColor = ConsoleColor.Gray;

        //get player name
        do
        {
            Console.Write("Please Enter Your Name: ");
            name = Console.ReadLine();

        } while (name.Length < 1);

        //get player symbol
        do
        {
            Console.Write("Pick Your Symbol ");
            for (int i = 0; i < symbols.Count; i++)
            {
                Console.Write(symbols[i] + "(" + (i + 1) + ") ");
            }
            Console.Write(": ");
            sm = Console.ReadLine();

        } while (sm.Length < 1 || int.TryParse(sm, out smChoice) == false || smChoice < 1 || smChoice > symbols.Count);
        smChoice--;

        //get player color
        do
        {
            Console.Write("Pick Your Color ");
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i] == "Red")
                    Console.ForegroundColor = ConsoleColor.Red;
                if (colors[i] == "Blue")
                    Console.ForegroundColor = ConsoleColor.Blue;
                if (colors[i] == "Green")
                    Console.ForegroundColor = ConsoleColor.Green;
                if (colors[i] == "Yellow")
                    Console.ForegroundColor = ConsoleColor.Yellow;
 
                Console.Write(colors[i] + "(" + (i + 1) + ") ");
                Console.ResetColor();
            }
            Console.Write(": ");
            cl = Console.ReadLine();

        } while (cl.Length < 1 || int.TryParse(cl, out clChoice) == false || clChoice < 1 || clChoice > colors.Count);
        clChoice--;


        cl = colors[clChoice]; //store the player choice 
        colors.RemoveAt(clChoice); //remove the element chosen from the list

        sym = symbols[smChoice];
        symbols.RemoveAt(smChoice);

        Player p = new Player(name, sym, cl); //an object of the class player    
        Console.WriteLine();
        Console.ResetColor();
        return p; //return the object
    }


    //Change the foreground color according to the player's choice
    static void CheckColor(string color)
    {
        switch (color)
        {
            case "Red":
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;

            case "Blue":
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                break;

            case "Green":
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                break;

            case "Yellow":
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;

            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }


    //Display board
    static void DisplayBoard(char[,] board, Player p1, Player p2)
    {
        Console.Clear();

        //size of matrix:
        int rows = 6;
        int columns = 7;

        //for loop to display matrix
        for (int i = 1; i <= rows; i++)
        {
            Console.Write("|");
            for (int j = 1; j <= columns; j++)
            {
                //check if the there is an already selected position
                if (board[i, j] != p1.symbol && board[i, j] != p2.symbol)
                {
                    board[i, j] = '*';
                    Console.ResetColor();
                }
                else if (board[i, j] == p1.symbol)
                {
                    CheckColor(p1.color);
                }
                else if (board[i, j] == p2.symbol)
                {
                    CheckColor(p2.color);
                }
                Console.Write(board[i, j]); //print to console
            }
            Console.ResetColor();
            Console.WriteLine("|");
        }
    }

    //get the player's drop-place selection
    static int PlayerDrop(char[,] board, Player activePlayer, Player secondPlayer)
    {
        int dropChoice; //stores the number entered by user
        string s; //used in case the user enters a letter instead of a number
        Console.WriteLine();
        CheckColor(activePlayer.color);
        Console.WriteLine(activePlayer.name + "'s Turn ");
        Console.ResetColor();
        Console.WriteLine();
        Console.Write("Please enter a number between 1 and 7: ");

        //prompt player to select a drop-place
        do
        {
            s = Console.ReadLine(); //get the input as a string

            //check whether the number is valid or not
            if (int.TryParse(s, out dropChoice) == false || dropChoice < 1 || dropChoice > 7)
            {
                Console.Write("Please enter a valid number: ");
            }

            //check if the coloumn is full
            else if (int.TryParse(s, out dropChoice) == true && (board[1, dropChoice] == activePlayer.symbol || board[1, dropChoice] == secondPlayer.symbol))
            {
                Console.Write("This column is full, please pick another one: ");
            }


            
        } while (int.TryParse(s, out dropChoice) == false || dropChoice < 1 || dropChoice > 7 || board[1, dropChoice] == activePlayer.symbol || board[1, dropChoice] == secondPlayer.symbol);

        return dropChoice;
    }

    //check if the drop-place is already taken
    static void CheckBellow(char[,] board, Player activePlayer, int dropChoice, Player secondPlayer)
    {
        int rows = 6;
        bool placed = false; //a boolean to check if the symbol was placed or not yet

        do
        {
            if (board[rows, dropChoice] != activePlayer.symbol && board[rows, dropChoice] != secondPlayer.symbol)
            {
                board[rows, dropChoice] = activePlayer.symbol;
                placed = true;
            }
            else
                rows--; //get the upper row

        } while (!placed); //repeat until the symbol is placed
    }

    //Check if the board became full
    static bool FullBoard(char[,] board)
    {
        bool full = false;
        int f = 0; //counts how many full cols in a row 
        for (int i = 1; i <= 7; i++)
        {
            if (board[1, i] != '*')
                f++; //increment is the place is already taken
        }
        if (f == 7)
            full = true; //if whole row is full

        return full;
    }


    //Check if the player connected 3
    static bool CheckThree(char[,] board, Player activePlayer, int drop)
    {
        char sm = activePlayer.symbol;
        bool win = false;

        //cover all options
        for (int i = 7; i > 0; i--)
        {
            for (int j = drop; j > 0; j--)
            {
                //no error will be received when decrementing the index, because board[0,j], board[i,0], board[7,j] and board[i,8] 
                //are left empty, these empty rows/cols will break the if statement and stop it from causing the "index out of range error"
                if (board[i, j] == sm &&            // X
                    board[i - 1, j - 1] == sm &&    //   X
                    board[i - 2, j - 2] == sm )     //     X
                {
                    win = true;
                }
                else if (board[i, j] == sm &&
                         board[i, j - 1] == sm &&   // X X X  <=
                         board[i, j - 2] == sm )
                {
                    win = true;
                }

                else if (board[i, j] == sm &&       // X
                         board[i - 1, j] == sm &&   // X 
                         board[i - 2, j] == sm)     // X
                {
                    win = true;
                }

                else if (board[i, j] == sm &&           //       X
                         board[i - 1, j + 1] == sm &&   //     X
                         board[i - 2, j + 2] == sm)     //   X
  
                {
                    win = true;
                }

                else if (board[i, j] == sm &&
                         board[i, j + 1] == sm &&   // => X X X
                         board[i, j + 2] == sm)
                {
                    win = true;
                }
            }
        }
        return win;
    }

    //Check if the player connected 4
    static bool CheckFour(char[,] board, Player activePlayer, int drop)
    {
        char sm = activePlayer.symbol;
        bool win = false;

        //cover all options
        for (int i = 7; i > 0; i--)
        {
            for (int j = drop; j > 0; j--)
            {
                //no error will be received when decrementing the index, because board[0,j], board[i,0], board[7,j] and board[i,8] 
                //are left empty, these empty rows/cols will break the if statement and stop it from causing the "index out of range error"
                if (board[i, j] == sm &&            // X
                    board[i - 1, j - 1] == sm &&    //   X
                    board[i - 2, j - 2] == sm &&    //     X
                    board[i - 3, j - 3] == sm)      //       X
                {
                    win = true;
                }
                else if (board[i, j] == sm &&
                         board[i, j - 1] == sm &&   // X X X X  <=
                         board[i, j - 2] == sm &&
                         board[i, j - 3] == sm)
                {
                    win = true;
                }

                else if (board[i, j] == sm &&       // X
                         board[i - 1, j] == sm &&   // X 
                         board[i - 2, j] == sm &&   // X
                         board[i - 3, j] == sm)     // X
                {
                    win = true;
                }

                else if (board[i, j] == sm &&           //       X
                         board[i - 1, j + 1] == sm &&   //     X
                         board[i - 2, j + 2] == sm &&   //   X
                         board[i - 3, j + 3] == sm)     // X
                {
                    win = true;
                }

                else if (board[i, j] == sm &&
                         board[i, j + 1] == sm &&   // => X X X X 
                         board[i, j + 2] == sm &&
                         board[i, j + 3] == sm)
                {
                    win = true;
                }
            }
        }
        return win;
    }

    //Check if the player connected 5
    static bool CheckFive(char[,] board, Player activePlayer, int drop)
    {
        char sm = activePlayer.symbol;
        bool win = false;

        //cover all options
        for (int i = 7; i > 0; i--)
        {
            for (int j = drop; j > 0; j--)
            {
                //no error will be received when decrementing the index, because board[0,j], board[i,0], board[7,j] and board[i,8] 
                //are left empty, these empty rows/cols will break the if statement and stop it from causing the "index out of range error"
                if (board[i, j] == sm &&            // X
                    board[i - 1, j - 1] == sm &&    //   X
                    board[i - 2, j - 2] == sm &&    //     X
                    board[i - 3, j - 3] == sm &&    //       X
                    board[i - 4, j - 4] == sm)      //         X
                {
                    win = true;
                }
                else if (board[i, j] == sm &&
                         board[i, j - 1] == sm &&   // X X X X X<=
                         board[i, j - 2] == sm &&
                         board[i, j - 3] == sm &&
                         board[i, j - 4] == sm)
                {
                    win = true;
                }

                else if (board[i, j] == sm &&       // X
                         board[i - 1, j] == sm &&   // X 
                         board[i - 2, j] == sm &&   // X
                         board[i - 3, j] == sm &&   // X
                         board[i - 4, j] == sm)     // X
                {
                    win = true;
                }

                else if (board[i, j] == sm &&           //         X
                         board[i - 1, j + 1] == sm &&   //       X
                         board[i - 2, j + 2] == sm &&   //     X
                         board[i - 3, j + 3] == sm &&   //   X
                         board[i - 4, j + 4] == sm)     // X 
                {
                    win = true;
                }

                else if (board[i, j] == sm &&
                         board[i, j + 1] == sm &&   // => X X X X X
                         board[i, j + 2] == sm &&
                         board[i, j + 3] == sm &&
                         board[i, j + 4] == sm)
                {
                    win = true;
                }
            }
        }
        return win;
    }

    //Call when a player wins
    static void PlayerWin(Player activePlayer)
    {
        CheckColor(activePlayer.color);
        Console.WriteLine();
        Console.WriteLine((activePlayer.name).ToUpper() + " IS THE WINNER!!!");
        Console.WriteLine();
        Console.ResetColor();
    }


    //to restart game
    static int restart(char[,] board)
    {
        int restart;
        string s;

        do
        {
            Console.Write("Do you want to restart the game? ");
            Console.Write("Yes(1) No(2): ");  
            s = Console.ReadLine();
        } while (int.TryParse(s, out restart) == false || restart < 1 || restart > 2);

        if (restart == 1)
        { //reset board
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    board[i, j] = '*';
                }
            }
            Console.Clear();
        }
        else
            Console.WriteLine("Goodbye!, Hope to see you again soon :)"); //end game

        return restart;
    }
}