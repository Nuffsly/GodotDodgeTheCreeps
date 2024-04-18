using Godot;

public partial class mainScene : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	
	private int _score;

	// Main does not utilize _Ready as NewGame is called from UI logic.
	public override void _Ready(){}

	public void GameOver()
	{
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HudScene>("HUD").ShowGameOver();
	}
	
	public void NewGame()
	{
		_score = 0;
		
		GetNode<AudioStreamPlayer>("Music").Play();
		
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		
		var hud = GetNode<HudScene>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("GET READY!");
		var player = GetNode<PlayerScene>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
	}
	
	private void OnScoreTimerTimeout()
	{
		_score++;
		var hud = GetNode<HudScene>("HUD");
		hud.UpdateScore(_score);
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
	private void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<mobScene>();
		
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		
		mob.Position = mobSpawnLocation.Position;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		AddChild(mob);
	}
}
