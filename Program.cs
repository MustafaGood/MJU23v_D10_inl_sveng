         using System;
using System.Collections.Generic;
using System.IO;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary = new List<SweEngGloss>();

        class SweEngGloss
        {
            public string word_swe, word_eng;

            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe;
                this.word_eng = word_eng;
            }

            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0];
                this.word_eng = words[1];
            }
        }

        static void Loader(string filePath)
        {
            try
            {
                using (StreamReader fileReader = new StreamReader(filePath))
                {
                    dictionary = new List<SweEngGloss>();
                    string line = fileReader.ReadLine();
                    while (line != null)
                    {
                        SweEngGloss glossary = new SweEngGloss(line);
                        dictionary.Add(glossary);
                        line = fileReader.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{filePath}' not found.");
            }
        }

        static void Translator(string translate)
        {
            bool translationFound = false;
            foreach (SweEngGloss glossary in dictionary)
            {
                if (glossary.word_swe == translate)
                {
                    Console.WriteLine($"English for {glossary.word_swe} is {glossary.word_eng}");
                    translationFound = true;
                }
                else if (glossary.word_eng == translate)
                {
                    Console.WriteLine($"Swedish for {glossary.word_eng} is {glossary.word_swe}");
                    translationFound = true;
                }
            }

            if (!translationFound)
            {
                Console.WriteLine($"The word '{translate}' doesn't exist in the dictionary.");
            }
        }

        static void Delete(string word_swe, string word_eng)
        {
            int countBeforeRemoval = dictionary.Count;
            dictionary.RemoveAll(g => (g.word_swe == word_swe && g.word_eng == word_eng) || (g.word_swe == word_eng && g.word_eng == word_swe));

            int countAfterRemoval = dictionary.Count;
            int removedCount = countBeforeRemoval - countAfterRemoval;

            if (removedCount > 0)
            {
                Console.WriteLine($"Removed {removedCount} word(s) from the dictionary.");
            }
            else
            {
                Console.WriteLine($"No matching word found in the dictionary.");
            }
        }

        static string ReadWord(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static void Main(string[] args)
        {
            string defaultFile = "sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");

            while (true)
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];

                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                else if (command == "help")
                {
                    string[] help =
                    {
                        "  help            - display this help",
                        "  load            - load all words from a file to the list",
                        "  list            - display all currently loaded words",
                        "  new             - add new words to the list",
                        "  delete          - delete a word from the list",
                        "  translate       - translate a specific word from the list",
                    };

                    foreach (string info in help)
                    {
                        Console.WriteLine(info);
                    }
                }

                else if (command == "load")
                {
                    string filePath = (argument.Length == 2) ? argument[1] : defaultFile;
                    Loader(filePath);
                }

                else if (command == "list")
                {
                    if (dictionary.Count > 0)
                    {
                        foreach (SweEngGloss glossary in dictionary)
                        {
                            Console.WriteLine($"{glossary.word_swe,-10}  - {glossary.word_eng,-10}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The dictionary is empty.");
                    }
                }

                else if (command == "new")
                {
                    if (argument.Length == 3)
                    {
                        string swedish = argument[1];
                        string english = argument[2];
                        dictionary.Add(new SweEngGloss(swedish, english));
                    }
                    else if (argument.Length == 1)
                    {
                        string swedish = ReadWord("Enter word in Swedish: ");
                        string english = ReadWord("Enter word in English: ");
                        dictionary.Add(new SweEngGloss(swedish, english));
                    }
                    else
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }

                else if (command == "delete")
                {
                    if (argument.Length == 3)
                    {
                        string swedish = argument[1];
                        string english = argument[2];
                        Delete(swedish, english);
                    }
                    else if (argument.Length == 2)
                    {
                        string wordToDelete = argument[1];
                        Delete(wordToDelete, wordToDelete);
                    }
                    else if (argument.Length == 1)
                    {
                        string swedish = ReadWord("Enter word in Swedish: ");
                        string english = ReadWord("Enter word in English: ");
                        Delete(swedish, english);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }

                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        string wordToTranslate = argument[1];
                        Translator(wordToTranslate);
                    }
                    else if (argument.Length == 1)
                    {
                        string wordToTranslate = ReadWord("Enter word to translate: ");
                        Translator(wordToTranslate);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }

                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
        }
    }
}
