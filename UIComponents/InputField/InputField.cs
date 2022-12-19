using Godot;
using System;

public class InputField : MarginContainer
{
	[Signal]
	public delegate void text_changed(string newText);

	[Export]
	NodePath lineEditPath;
	LineEdit lineEdit;

	public string Text 
	{
		get
		{
			return lineEdit.Text;
		}
		set
		{
			lineEdit.Text = value;
		}
	}

	public override void _Ready()
	{
		lineEdit = GetNode<LineEdit>(lineEditPath);

		lineEdit.Connect("text_changed", this, nameof(EmitTextChanged));
	}

	void EmitTextChanged(string newText)
	{
		EmitSignal(nameof(text_changed), newText);
	}
}
