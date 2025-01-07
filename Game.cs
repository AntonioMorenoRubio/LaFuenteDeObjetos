﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        static (int Row, int Column) entrancePosition = (0, 0);
        static (int Row, int Column) playerPosition = entrancePosition;
        static (int Row, int Column) fountainPosition = (0, 2);
        static bool gameCompleted = false;
        static bool fountainActivated = false;

        public static void Boot()
        {
            Console.WriteLine("Booting...");

            rooms =
            [
                new int[4],
                new int[4]
            ];

            //Console.WriteLine($"Player position at x:{playerPosition.Column}, y:{playerPosition.Row}.");
            //Console.WriteLine($"Fountain position at x:{fountainPosition.Column}, y:{fountainPosition.Row}.");

            //Console.WriteLine("Prueba de textos:");
            //DisplayNarrativeText("Prueba de texto narrativo.");
            //DisplayDescriptiveText("Prueba de texto descriptivo.");
            //DisplayPlayerInputText("Prueba de texto de entrada del jugador.");
            //DisplayEntranceLightText("Prueba de texto de la luz de la entrada.");
            //DisplayFountainText("Prueba de texto de la fuente de objetos.");

            Console.Clear();
            PlayGame();
        }

        private static void PlayGame()
        {
            while (gameCompleted != true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------");
                Console.WriteLine($"You are in the room at (Row:{playerPosition.Row}, Column:{playerPosition.Column}).");

                if (playerPosition == entrancePosition)
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
                if (playerPosition == fountainPosition)
                {
                    if (fountainActivated == false)
                    {
                        DisplayFountainText("You hear water dripping in this room. The Fountain of Objects is here!");
                    } else
                    {
                        DisplayFountainText("You hear the rushing waters from the Fountain of Objects. It has ben reactivated!");
                    }
                }
                Console.Write("What do you want to do? ");
                EvaluatePlayerInput();
                Console.ForegroundColor = DESCRIPTIVETEXTCOLOR;
            }
        }

        private static void EvaluatePlayerInput()
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
                    if (playerPosition.Column != 3)
                        playerPosition.Column++;
                    break;
                case ("move north"):
                    if (playerPosition.Row != 0)
                        playerPosition.Row--;
                    break;
                case ("move south"):
                    if (playerPosition.Row != 3)
                        playerPosition.Row++;
                    break;
                case ("enable fountain"):
                    if (playerPosition == fountainPosition)
                        fountainActivated = true;
                    break;
                default:
                    break;
            }
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
