using Modules.PlayerTapController;

public class SpeedBoosterBehaviour : ABoosterBehaviour
{
    private float _speedModifier;
    private int _durationS;
    
    public override void SetupValues(float floatValue, int intValue)
    {
        _durationS = intValue;
        _speedModifier = floatValue;
    }
    
    protected override void UseBooster(IPlayer player)
    {
        player.ApplySpeedChange(player.Speed * _speedModifier, _durationS);
        Destroy(gameObject);
    }
}
