using Modules.PlayerTapController;

public class IDDQDBoosterBehaviour : ABoosterBehaviour
{
    private float _durationS;
    
    public override void SetupValues(float floatValue, int intValue)
    {
        _durationS = floatValue;
    }
    
    protected override void UseBooster(IPlayer player)
    {
        player.ApplyIDDQDMode(_durationS);
        Destroy(gameObject);
    }
}
