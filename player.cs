using Godot;
using System;

public partial class player : RigidBody3D
{
    private const string MOVE_FORWARD = "move_forward";
    private const string MOVE_RIGHT = "move_right";
    private const string MOVE_LEFT = "move_left";
    private const string MOVE_BACKFORWARD = "move_backward";

    private const string UI_CANCEL = "ui_cancel";

    private float _mouseSensitivity = 0.001f;
    private float _twistInput = 0.0f;
    private float _pitchInput = 0.0f;

    private Node3D _twistPivot;
    private Node3D _pitchPivot;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _twistPivot = GetNode<Node3D>("TwistPivot");
        _pitchPivot = _twistPivot.GetNode<Node3D>("PitchPivot");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var input = Vector3.Zero;

        input.X = Input.GetAxis(MOVE_LEFT, MOVE_RIGHT);
        input.Z = Input.GetAxis(MOVE_FORWARD, MOVE_BACKFORWARD);

        ApplyCentralForce((float)delta * 1200 * input * _twistPivot.Basis.Transposed());

        if (Input.IsActionJustPressed(UI_CANCEL))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        _twistPivot.RotateY(_twistInput);
        _pitchPivot.RotateX(_pitchInput);

        var rotation = new Vector3()
        {
            X = (float)Mathf.Clamp(_pitchPivot.Rotation.X, -0.5, 0.5)
        };

        _pitchPivot.Rotation = rotation;
        _twistInput = 0.0f;
        _pitchInput = 0.0f;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                var inpEvent = @event as InputEventMouseMotion;
                _twistInput = - inpEvent.Relative.X * _mouseSensitivity;
                _pitchInput = - inpEvent.Relative.Y * _mouseSensitivity;
            }
        }
    }
}
