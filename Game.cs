
namespace _31_FountainOfObjects
{
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
        static (int Row, int Column)[] entrancePosition = { (0, 0), (2, 0), (6, 7) };
        static (int Row, int Column)[] fountainPosition = { (0, 2), (3, 3), (5, 2) };
        static (int Row, int Column)[][] pitsPositionByDifficulty =
        {
            [(0, 1)],
            [(2, 2), (4, 3)],
            [(2, 3),(2, 7),(5, 4),(7, 2)]
        };
        static (int Row, int Column)[][] maelstormsPositionByDifficulty =
        {
            [(3, 1)],
            [(0, 1)],
            [(4, 2),(3, 6)]
        };
        static (int Row, int Column)[][] amaroksPositionByDifficulty =
        {
            [(2, 1)],
            [(1, 4), (4, 1)],
            [(2, 1),(2, 5),(6, 3)]
        };

        static (int Row, int Column) playerPosition;
        static bool gameCompleted = false;
        static bool fountainActivated = false;
        static bool playerDead = false;

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

            playerPosition = entrancePosition[difficulty];

            Console.Clear();
            PlayGame();
        }

        private static void PlayGame()
        {
            while (gameCompleted != true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------");
                Console.WriteLine($"You are in the room at (Row:{playerPosition.Row}, Column:{playerPosition.Column}).");

                if (playerPosition == entrancePosition[difficulty])
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
                if (playerPosition == fountainPosition[difficulty])
                {
                    if (fountainActivated == false)
                    {
                        DisplayFountainText("You hear water dripping in this room. The Fountain of Objects is here!");
                    } else
                    {
                        DisplayFountainText("You hear the rushing waters from the Fountain of Objects. It has ben reactivated!");
                    }
                }

                foreach ((int, int) pit in pitsPositionByDifficulty[difficulty])
                {
                    if (playerPosition == pit)
                    {
                        playerDead = true;
                        DisplayDescriptiveText("You fall through a bottomless pit to your death. Game Over.");
                        break;
                    }
                    else if (pit.Item2 - playerPosition.Column >= -1 && pit.Item2 - playerPosition.Column <= 1
                        && pit.Item1 - playerPosition.Row >= -1 && pit.Item1 - playerPosition.Row <= 1)
                    {
                        DisplayDescriptiveText("You feel a draft. There is a pit in a nearby room.");
                    }
                }
                if (playerDead)
                    break;

                foreach ((int, int) amarok in amaroksPositionByDifficulty[difficulty])
                {
                    if (playerPosition == amarok)
                    {
                        playerDead = true;
                        DisplayDescriptiveText("An amarok leaps onto you, biting you fiercely with its teeth, killing you. Game Over.");
                        break;
                    }
                    else if (amarok.Item2 - playerPosition.Column >= -1 && amarok.Item2 - playerPosition.Column <= 1
                        && amarok.Item1 - playerPosition.Row >= -1 && amarok.Item1 - playerPosition.Row <= 1)
                    {
                        DisplayDescriptiveText("You can smell the rotten stench of an amarok in a nearby room.");
                    }
                }
                if (playerDead)
                    break;

                foreach ((int, int) maelstorm in maelstormsPositionByDifficulty[difficulty])
                {
                    if (maelstorm.Item2 - playerPosition.Column >= -1 && maelstorm.Item2 - playerPosition.Column <= 1
                        && maelstorm.Item1 - playerPosition.Row >= -1 && maelstorm.Item1 - playerPosition.Row <= 1)
                    {
                        DisplayDescriptiveText("You hear the growling and groaning of a maelstorm nearby.");
                    }
                }

                Console.Write("What do you want to do? ");
                EvaluatePlayerActionsInGameLoop();

                for (int i = 0; i < maelstormsPositionByDifficulty[difficulty].Length; i++)
                {
                    (int, int) maelstorm = maelstormsPositionByDifficulty[difficulty][i];
                    if (playerPosition == maelstorm)
                    {
                        if (maelstorm.Item1 >= rooms[0].Length - 2 && maelstorm.Item2 <= 2)
                            maelstorm = (rooms[0].Length - 1, 0);
                        else if (maelstorm.Item1 >= rooms[0].Length - 2 && maelstorm.Item2 > 2)
                            maelstorm = (rooms[0].Length - 1, maelstorm.Item2 - 2);
                        else if (maelstorm.Item1 < rooms[0].Length - 2 && maelstorm.Item2 <= 2)
                            maelstorm = (++maelstorm.Item1, 0);
                        else
                            maelstorm = (++maelstorm.Item1, maelstorm.Item2 - 2);

                        if (playerPosition.Row <= 1 && playerPosition.Column >= rooms[1].Length - 2)
                            playerPosition = (0, rooms[i].Length - 1);
                        else if (playerPosition.Row <= 1 && playerPosition.Column <= rooms[1].Length - 2)
                            playerPosition = (0, playerPosition.Column + 2);
                        else if (playerPosition.Row >= 1 && playerPosition.Column >= rooms[1].Length - 2)
                            playerPosition = (--playerPosition.Row, rooms[i].Length - 1);
                        else
                            playerPosition = (--playerPosition.Row, playerPosition.Column + 2);

                        DisplayDescriptiveText("You have found a maelstorm and it sends you away one room north and two rooms east.");
                        break;
                    }
                }
            }
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
                    if (playerPosition == fountainPosition[difficulty])
                        fountainActivated = true;
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

        private static void DisplayPlayerInputText(string text)
        {
            Console.ForegroundColor = PLAYERINPUTTEXTCOLOR;
            Console.WriteLine(text);
            Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
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
}
