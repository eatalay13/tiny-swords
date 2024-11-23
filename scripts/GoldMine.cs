using Godot;
using TinySwords.scripts.Prefabs;

namespace TinySwords.scripts;

public partial class GoldMine : Sprite2D
{
    [Export] private Texture2D _goldMineInActive;
    [Export] private Texture2D _goldMineActive;
    [Export] private PackedScene _goldScene;

    private Timer _gSpawnTimer;

    private bool _isActive = true;

    private int _goldSpawnStartPosition = -72;
    
    private byte _goldSpawnCounter;
    private byte _goldSpawnRateMax = 5;

    private bool IsFull => _goldSpawnCounter >= _goldSpawnRateMax;

    private bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            Texture = value ? _goldMineActive : _goldMineInActive;

            if (value)
                _gSpawnTimer.Start();
            else
                _gSpawnTimer.Stop();
        }
    }

    public override void _Ready()
    {
        _gSpawnTimer = GetNode<Timer>("GSpawnTimer");
        _gSpawnTimer.Timeout += SpawnGold;

        IsActive = true;
    }

    private void Reset()
    {
        _isActive = true;
        _goldSpawnCounter = 0;
    }

    private void FullCapacity()
    {
        IsActive = false;
        _goldSpawnCounter = _goldSpawnRateMax;
    }

    private void SpawnGold()
    {
        if (!IsActive)
            return;

        var gold = _goldScene.Instantiate<Gold>();
        GetParent().AddChild(gold);

        gold.GlobalPosition = GlobalPosition + new Vector2(_goldSpawnStartPosition + (_goldSpawnCounter * 32), 48);

        _goldSpawnCounter++;

        if (IsFull)
            FullCapacity();
    }
}