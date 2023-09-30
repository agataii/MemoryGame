Helpers.MainColor("Добро пожаловать в игру Memory-game!");

while (true)
{
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("1. Создать новую игру");
    Console.WriteLine("2. Показать топ игроков");
    Console.WriteLine("3. Выйти из игры");

    int choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 1:
            Console.Clear();
            Game.StartGame();
            break;
        case 2:
            Console.Clear();
            Game.DisplayTopPlayers();
            Console.ReadKey();
            Console.Clear();
            break;
        case 3:
            Console.Clear();
            Helpers.MainColor("До свидания!");
            return;
        default:
            Console.Clear();
            Helpers.ColorRed("Ошибка! Некорректный ввод.");
            Console.ReadKey();
            Console.Clear();
            break;
    }
}