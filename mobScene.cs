using Godot;

public partial class mobScene : RigidBody2D
{
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}
	
	private void OnVisibleOnScreenNotifier2dScreenExited()
	{
		QueueFree();
	}
}
