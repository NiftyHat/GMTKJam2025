using Godot;
using System;

public partial class MainMenuTitleDisplay : Control
{ 
	
	[Signal] public delegate void FadeInAnimationStartedEventHandler();
	[Signal] public delegate void FadeInAnimationTitleShownEventHandler();
	[Signal] public delegate void FadeInAnimationFinishedEventHandler();
	[Signal] public delegate void FadeOutAnimationStartedEventHandler();
	[Signal] public delegate void FadeOutAnimationFadingEventHandler();
	[Signal] public delegate void FadeOutAnimationCompleteEventHandler();
	
	
	[Export] private Color ColorPulseA;
	[Export] private Color ColorPulseB;
	[Export] private Color ColorFlashA;
	[Export] private Color ColorFlashB;

	private TextureRect TitleImage;
	private Label TitleText;
	private PanelContainer TitleContainer;

	private Tween AnimationTween;
	private Tween TextFade;

	private enum State
	{
		FADE_IN,
		WAITING,
		FADE_OUT
	}

	private State state = State.WAITING;

	public override void _Ready()
	{
		base._Ready();
		
		TitleImage = (TextureRect)GetNode("VBoxContainer/TitleBox/TextureRect");
		TitleContainer = (PanelContainer)GetNode("VBoxContainer/LabelBox");
		TitleText = (Label)GetNode("VBoxContainer/LabelBox/Padder/Label");
		
		TextFade = CreateTween();
		TextFade.TweenProperty(TitleText, "modulate", ColorPulseB, .75f);
		TextFade.TweenProperty(TitleText, "modulate", ColorPulseA, .75f);
		TextFade.SetLoops();

		FadeInAnimation();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("ui_accept"))
		{
			switch (state)
			{
				case State.FADE_IN:
					AnimationTween.Pause();
					AnimationTween.CustomStep(3.0f);
					break;
				case State.WAITING:
					FadeOut();
					break;
				case State.FADE_OUT:
					GD.Print("?");
					break;
			}
		}
	}

	private void FadeInAnimation()
	{
		state = State.FADE_IN;
		TitleText.Modulate = ColorPulseA;
		
		TitleContainer.Modulate = Colors.Transparent;
		Modulate = Colors.Transparent;
		
		AnimationTween = CreateTween();
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeInAnimationStarted));
		AnimationTween.TweenProperty(this, "modulate", Colors.White, 1.0f);
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeInAnimationTitleShown));
		AnimationTween.TweenProperty(TitleContainer, "modulate", Colors.White, 2.0f);
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeInAnimationFinished));
		AnimationTween.Finished += OnFadedIn;
	}
	

	private void OnFadedIn()
	{
		state = State.WAITING;

		TitleContainer.Modulate = Colors.White;
		Modulate = Colors.White;
		
		TextFade.Play();
	}

	private void FadeOut()
	{
		state = State.FADE_OUT;
		TextFade.Stop();

		Tween flash = CreateTween();
		flash.TweenProperty(TitleText, "modulate", ColorFlashA, .1f);
		flash.TweenProperty(TitleText, "modulate", ColorFlashB, .1f);
		flash.SetLoops(10);

		AnimationTween = CreateTween();
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeOutAnimationStarted));
		AnimationTween.TweenSubtween(flash);
		AnimationTween.TweenProperty(TitleContainer, "modulate", Colors.Transparent, .5f);
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeOutAnimationFading));
		AnimationTween.TweenProperty(this, "modulate", Colors.Transparent, 2f);
		AnimationTween.TweenCallback(Callable.From(EmitSignalFadeOutAnimationComplete));
	}
}
