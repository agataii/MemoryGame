using System.Text.Json;
using System.Timers;
public static class Game
{
    static Thread thread = new Thread(Time);

    private static List<Player> topPlayers = new List<Player>();
    private const string _jsonFilePath = "C:\\AGU\\topPlayers.json";
    
    static ManualResetEvent resetEvent = new ManualResetEvent(false);

    static int secondsLeft = 180;
    static System.Timers.Timer timer;

    static bool IsGameOver;
    public static void StartGame()
    {
        
        Console.WriteLine("Введите имя:");
        string playerName = Console.ReadLine();
        Console.Title = $"{playerName}";
        Console.Clear();
Restart:
        IsGameOver = true;
        Helpers.MainColor($"Начинаем новую игру, {playerName}!");
        Helpers.ColorRed("У вас есть 3 минуты");
        int rows = 4;
        int cols = 4;
        char[,] cards = new char[,] {
            { 'A', 'A', 'B', 'B' },
            { 'C', 'C', 'D', 'D' },
            { 'E', 'E', 'F', 'F' },
            { 'G', 'G', 'H', 'H' }
        };
        // Создаем массив для хранения расположения карт
        int[] positions = Enumerable.Range(0, rows * cols).ToArray();

        Shuffle(cards);

        bool[] isOpened = new bool[rows * cols];

        thread = new Thread(Time);

        thread.Start();
        while (true)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    if (isOpened[index])
                    {
                        Console.Write(cards[i, j]);
                    }
                    else
                    {
                        Console.Write("*");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Введите два номера карт (0-15): ");
            int card1 = Convert.ToInt32(Console.ReadLine());
            int card2 = Convert.ToInt32(Console.ReadLine());
            
            if (isOpened[card1] || isOpened[card2])
            {
                Console.Clear();
                continue;
            }

            if (cards[positions[card1] / cols, positions[card1] % cols] == cards[positions[card2] / cols, positions[card2] % cols])
            {
                isOpened[card1] = true;
                isOpened[card2] = true;
            }
            else
            {
                Console.Clear();
                isOpened[card1] = true;
                isOpened[card2] = true;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        int index = i * cols + j;
                        if (isOpened[index])
                        {
                            Console.Write(cards[i, j]);
                        }
                        else
                        {
                            Console.Write("*");
                        }
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Thread.Sleep(2000);
                isOpened[card1] = false;
                isOpened[card2] = false;
            }

            if (secondsLeft == 0)
            {
                resetEvent.WaitOne();
                IsGameOver = false;

                Helpers.ColorRed("Время закончился.Ты проиграл:(");
                Console.ReadKey();
                Console.Clear();

                Console.Write("Вы хотите рестарт? (да/нет): ");
                string answer = Console.ReadLine().ToLower();
                if (answer == "да")
                {
                    isOpened = new bool[cards.Length];
                    goto Restart;
                }
                else
                {
                    break;
                }
            }
            else
            {
                if (isOpened.All(x => x))
                {
                    IsGameOver = false;
                    int playerTime = secondsLeft;
                    Helpers.ColorGreen("Ты победил!");
                    Player player = new Player(playerName, playerTime);
                    topPlayers.Add(player);
                    UpdateFile(playerName, playerTime);
                    Console.ReadKey();
                    Console.Clear();

                    Console.Write("Вы хотите рестарт? (да/нет): ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "да")
                    {
                        isOpened = new bool[cards.Length];
                        goto Restart;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Console.Clear();
        }
        Console.Clear();
    }

    public static void DisplayTopPlayers()
    {
        string input = File.ReadAllText(_jsonFilePath);

        topPlayers = JsonSerializer.Deserialize<List<Player>>(input);

        foreach(var item in topPlayers)
        {
            Console.WriteLine(item.playerName + "   " + item.playerTime);
        }
    }

    private static void UpdateFile(string playerName, int playerTime)
    {
        string output = JsonSerializer.Serialize<List<Player>>(topPlayers);
        File.WriteAllText(_jsonFilePath, output);
    }
    public static void Time()
    {
        timer = new System.Timers.Timer(1000);

        timer.Elapsed += OnTimerElapsed;

        timer.Start();
        if (IsGameOver == false)
        {
            timer.Stop();
            timer.Elapsed -= OnTimerElapsed;
        }
    }

    static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        secondsLeft--;

        if (secondsLeft == 0)
        {
            timer.Stop();
            resetEvent.Set();

        }
    }

    public static void Shuffle(char[,] arr)
    {
        Random rnd = new Random();
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        for (int i = rows - 1; i >= 0; i--)
        {
            for (int j = cols - 1; j >= 0; j--)
            {
                int randomRow = rnd.Next(0, i + 1);
                int randomCol = rnd.Next(0, j + 1);
               
                char temp = arr[i, j];
                arr[i, j] = arr[randomRow, randomCol]; 
                arr[randomRow, randomCol] = temp;
            }
        }
    }
}