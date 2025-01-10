public static class Game
{
    const ConsoleColor NARRATIVETEXTCOLOR = ConsoleColor.Magenta;
    const ConsoleColor DESCRIPTIVETEXTCOLOR = ConsoleColor.White;
    const ConsoleColor PLAYERINPUTTEXTCOLOR = ConsoleColor.Cyan;
    const ConsoleColor ENTRANCELIGHTTEXTCOLOR = ConsoleColor.Yellow;
    const ConsoleColor FOUNTAINTEXTCOLOR = ConsoleColor.Blue;

    static int[][] rooms;
    static int difficulty = 0;
    static int[] cavernSizeByDifficulty = [4, 6, 8];
    static (int Row, int Column)[] entrancePositionByDifficulty = { (0, 0), (2, 0), (6, 7) };
    static (int Row, int Column)[] fountainPositionByDifficulty = { (0, 2), (3, 3), (5, 2) };
    static (int Row, int Column)[][] pitsPositionByDifficulty =
    {
            [(0, 1)],
            [(2, 2), (4, 3)],
            [(2, 3),(2, 7),(5, 4),(7, 2)]
        };
    static List<List<(int Row, int Column)>> maelstormsPositionByDifficulty = new List<List<(int Row, int Column)>>
        {
            { new List<(int Row, int Column)> { (3, 1) } },
            { new List<(int Row, int Column)> { (0, 1) } },
            { new List<(int Row, int Column)> { (4, 2), (3, 6) } }
        };
    static List<List<(int Row, int Column)>> amaroksPositionByDifficulty = new List<List<(int Row, int Column)>>
        {
            { new List<(int Row, int Column)> {(2, 1) } },
            { new List<(int Row, int Column)> {(1, 4), (4, 1) } },
            { new List<(int Row, int Column)> {(2, 1), (2, 5), (6, 3) } }
        };

    static (int Row, int Column) playerPosition;
    static bool gameCompleted = false;
    static bool fountainActivated = false;
    static bool playerDead = false;
    static int arrows = 5;

    public static void Boot()
    {
        DisplayDescriptiveText("This game can be played in a 'small', 'medium' or 'large' game.\n" +
            "By default the game plays a 'small' game.\n" +
            "Please choose your option: ");
        EvaluatePlayerInputForDifficulty();

        rooms =
        [
            new int[cavernSizeByDifficulty[difficulty]],
                new int[cavernSizeByDifficulty[difficulty]]
        ];

        playerPosition = entrancePositionByDifficulty[difficulty];

        Console.Clear();
        PlayGame();
    }

    private static void PlayGame()
    {
        DisplayNarrativeText("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.\n" +
            "Light is visible only in the entrance, and no other light is seen anywhere in the caverns.\n" +
            "You must find navigate the Caverns with your other senses.\n" +
            "Look out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will die.\n" +
            "Maelstorms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns. You will be able to hear their growling and groaning in nearby rooms.\n" +
            "Amaroks roam the caverns. Encountering one is certain death, but you can smell their rotten stench in nearby rooms.\n" +
            "You carry with you a bow and a quiver of arrows. You can use them to shoot monsters in the caverns but be warned: you have a limited supply.\n" +
            "Find the Fountain of Objects, activate it, and return to the entrance.");
        DisplayDescriptiveText("You can write the help command to see what actions you can do.");

        while (gameCompleted != true)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine($"You are in the room at (Row:{playerPosition.Row}, Column:{playerPosition.Column}).");
            Console.WriteLine($"You are carrying {arrows} arrows.");

            if (playerPosition == entrancePositionByDifficulty[difficulty])
            {
                if (fountainActivated == true)
                {
                    gameCompleted = true;
                    DisplayNarrativeText("The Fountain of Objects has been reactivated, and you have escaped with your life!\nYou win!");
                    break;
                }
                else
                {
                    DisplayEntranceLightText("You see light in this room coming from outside the cavern. This is the entrance.");
                }
            }
            if (playerPosition == fountainPositionByDifficulty[difficulty])
            {
                if (fountainActivated == false)
                {
                    DisplayFountainText("You hear water dripping in this room. The Fountain of Objects is here!");
                }
                else
                {
                    DisplayFountainText("You hear the rushing waters from the Fountain of Objects. It has ben reactivated!");
                }
            }

            if (pitsPositionByDifficulty[difficulty].Any(x => x == playerPosition))
            {
                playerDead = true;
                DisplayDescriptiveText("You fall through a bottomless pit to your death. Game Over.");
                break;
            }

            foreach ((int Row, int Column) pit in pitsPositionByDifficulty[difficulty].Where(x => IsThreatAroundPlayer(x)))
                DisplayDescriptiveText("You feel a draft. There is a pit in a nearby room.");

            if (amaroksPositionByDifficulty[difficulty].Any(x => x == playerPosition))
            {
                playerDead = true;
                DisplayDescriptiveText("An amarok leaps onto you, biting you fiercely with its teeth, killing you. Game Over.");
                break;
            }

            foreach ((int Row, int Column) pit in amaroksPositionByDifficulty[difficulty].Where(x => IsThreatAroundPlayer(x)))
                DisplayDescriptiveText("You can smell the rotten stench of an amarok in a nearby room.");

            foreach ((int Row, int Column) pit in amaroksPositionByDifficulty[difficulty].Where(x => IsThreatAroundPlayer(x)))
                DisplayDescriptiveText("You hear the growling and groaning of a maelstorm nearby.");


            Console.Write("What do you want to do? ");
            EvaluatePlayerActionsInGameLoop();

            if (maelstormsPositionByDifficulty[difficulty].Any(x => x == playerPosition))
            {
                ApplyMaelstormOnPlayer(maelstormsPositionByDifficulty[difficulty].Where(x => x == playerPosition).First());
                DisplayDescriptiveText("You have found a maelstorm and it sends you away one room north and two rooms east.");
            }
        }
    }

    private static bool IsThreatAroundPlayer((int Row, int Column) threat)
    {
        return threat.Column - playerPosition.Column >= -1 && threat.Column - playerPosition.Column <= 1
               && threat.Row - playerPosition.Row >= -1 && threat.Row - playerPosition.Row <= 1;
    }

    private static void ApplyMaelstormOnPlayer((int Row, int Column) maelstorm)
    {
        if (maelstorm.Row >= rooms[0].Length - 2 && maelstorm.Column <= 2)
            maelstorm = (rooms[0].Length - 1, 0);
        else if (maelstorm.Row >= rooms[0].Length - 2 && maelstorm.Column > 2)
            maelstorm = (rooms[0].Length - 1, maelstorm.Column - 2);
        else if (maelstorm.Row < rooms[0].Length - 2 && maelstorm.Column <= 2)
            maelstorm = (++maelstorm.Row, 0);
        else
            maelstorm = (++maelstorm.Row, maelstorm.Column - 2);

        if (playerPosition.Row <= 1 && playerPosition.Column >= rooms[1].Length - 2)
            playerPosition = (0, rooms[1].Length - 1);
        else if (playerPosition.Row <= 1 && playerPosition.Column <= rooms[1].Length - 2)
            playerPosition = (0, playerPosition.Column + 2);
        else if (playerPosition.Row >= 1 && playerPosition.Column >= rooms[1].Length - 2)
            playerPosition = (--playerPosition.Row, rooms[1].Length - 1);
        else
            playerPosition = (--playerPosition.Row, playerPosition.Column + 2);
    }

    private static void EvaluatePlayerActionsInGameLoop()
    {
        Console.ForegroundColor = PLAYERINPUTTEXTCOLOR;
        string? playerInput = Console.ReadLine();

        if (playerInput == null)
            return;

        switch (playerInput.ToLower())
        {
            case ("move west"):
                if (playerPosition.Column != 0)
                    playerPosition.Column--;
                break;
            case ("move east"):
                if (playerPosition.Column < rooms[1].Length - 1)
                    playerPosition.Column++;
                break;
            case ("move north"):
                if (playerPosition.Row != 0)
                    playerPosition.Row--;
                break;
            case ("move south"):
                if (playerPosition.Row < rooms[0].Length - 1)
                    playerPosition.Row++;
                break;
            case ("enable fountain"):
                if (playerPosition == fountainPositionByDifficulty[difficulty])
                    fountainActivated = true;
                break;
            case ("shoot north"):
                if (arrows == 0)
                    DisplayDescriptiveText("You have no arrows to shoot.");
                else
                {
                    arrows--;
                    amaroksPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row - 1 && x.Column == playerPosition.Column);
                    maelstormsPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row - 1 && x.Column == playerPosition.Column);
                }
                break;
            case ("shoot south"):
                if (arrows == 0)
                    DisplayDescriptiveText("You have no arrows to shoot.");
                else
                {
                    arrows--;
                    amaroksPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row + 1 && x.Column == playerPosition.Column);
                    maelstormsPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row + 1 && x.Column == playerPosition.Column);
                }
                break;
            case ("shoot west"):
                if (arrows == 0)
                    DisplayDescriptiveText("You have no arrows to shoot.");
                else
                {
                    arrows--;
                    amaroksPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row && x.Column == playerPosition.Column - 1);
                    maelstormsPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row && x.Column == playerPosition.Column - 1);
                }
                break;
            case ("shoot east"):
                if (arrows == 0)
                    DisplayDescriptiveText("You have no arrows to shoot.");
                else
                {
                    arrows--;
                    amaroksPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row && x.Column == playerPosition.Column + 1);
                    maelstormsPositionByDifficulty[difficulty].RemoveAll(x => x.Row == playerPosition.Row && x.Column == playerPosition.Column + 1);
                }
                break;
            case ("help"):
                DisplayDescriptiveText("Available commands:\n" +
                    "move <north, west, south, east>: moves your character on the rows and columns. Moving south increases the Row you are in, moving north decreases the Row you are in. Moving east increases the Column you are in while moving west decreases the Column you are in.\n" +
                    "shoot <north, west, south, east>: shoot an arrow to the next room in that direction. If an amarok or maelstorm is in that room, it is struck down.\n" +
                    "enable fountain: enables the Fountain of Objects if you are in the room the Fountain is in.");
                break;
            default:
                break;
        }
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
    }

    private static void EvaluatePlayerInputForDifficulty()
    {
        Console.ForegroundColor = PLAYERINPUTTEXTCOLOR;
        string? playerInput = Console.ReadLine();

        if (playerInput == null)
            return;

        switch (playerInput.ToLower())
        {
            case ("medium"):
                difficulty = 1;
                break;
            case ("large"):
                difficulty = 2;
                break;
            default:
                break;
        }
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
    }

    static void DisplayNarrativeText(string text)
    {
        Console.ForegroundColor = NARRATIVETEXTCOLOR;
        Console.WriteLine(text);
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
    }

    private static void DisplayDescriptiveText(string text)
    {
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
        Console.WriteLine(text);
    }

    private static void DisplayEntranceLightText(string text)
    {
        Console.ForegroundColor = ENTRANCELIGHTTEXTCOLOR;
        Console.WriteLine(text);
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
    }

    private static void DisplayFountainText(string text)
    {
        Console.ForegroundColor = FOUNTAINTEXTCOLOR;
        Console.WriteLine(text);
        Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
    }
}