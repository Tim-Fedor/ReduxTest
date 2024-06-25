using Modules.PlayerTapController;

public class LivesBoosterBehaviour : ABoosterBehaviour
{
    private int _livesCount = 3;
    
    public override void SetupValues(float floatValue, int intValue)
    {
        _livesCount = intValue;
    }
    
    protected override void UseBooster(IPlayer player)
    {
        player.ApplyHealthChange(player.Lives + _livesCount);
        Destroy(gameObject);
    }


}
