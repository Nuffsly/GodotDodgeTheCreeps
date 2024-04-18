using Godot;

public partial class PlayerScene : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Export]
	public int Speed {get; set; } = 400; // Player movementspeed in pixels/sec.

	public Vector2 ScreenSize; // Size of game window.

	private AnimatedSprite2D _animatedSprite;
	private Vector2 _velocity;

	public override void _Ready()
	{
		Hide();
		ScreenSize = GetViewportRect().Size;
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		Move(delta);
		Animate();
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void Move(double delta)
	{
		_velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			_velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			_velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			_velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			_velocity.Y -= 1;
		}

		_velocity = _velocity.Normalized() * Speed;
		Position += _velocity * (float)delta;
		Position = Position.Clamp(Vector2.Zero, ScreenSize);
	}

	private void Animate()
	{
		if(_velocity.Length() > 0)
		{
			_animatedSprite.Play();
		}
		else
		{
			_animatedSprite.Stop();
		}
		
		if (_velocity.X != 0)
		{
			_animatedSprite.Animation = "walk";
			_animatedSprite.FlipV = false;
			_animatedSprite.FlipH = _velocity.X < 0;
		}
		else if(_velocity.Y != 0)
		{
			_animatedSprite.Animation = "up";
			_animatedSprite.FlipV = _velocity.Y > 0;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
