using Godot;

public partial class HudScene : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	public override void _Ready()
	{
		UpdateScore(0);
		GetNode<Label>("ScoreLabel").Hide();
	}
	
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	public void ShowMessage(string message)
	{
		var messageLabel = GetNode<Label>("Message");
		messageLabel.Text = message;
		messageLabel.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}
	
	public async void ShowGameOver()
	{
		ShowMessage("GAME OVER");
		
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);
		
		var messageLabel = GetNode<Label>("Message");
		messageLabel.Text = "DODGE THE CREEPS";
		messageLabel.Show();
		
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}
	
	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("ScoreLabel").Show();
		EmitSignal(SignalName.StartGame);
	}
	
	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
}
