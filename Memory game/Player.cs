public class Player: IComparable
{
    public string playerName { get; set; }
    public int playerTime { get; set; }

    public Player(string playerName, int playerTime)
    {
        this.playerName = playerName;
        this.playerTime = playerTime;
    }

    public int CompareTo(object? obj)
    {
        if(obj is Player player)
        {
            return player.playerTime.CompareTo(playerTime);
        }
        throw new Exception();
    }
}
