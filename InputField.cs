using Godot;
using System;

public class InputField : MarginContainer
{
	[Export]
	NodePath lineEditPath;
	LineEdit lineEdit;

	public override void _Ready()
	{
		lineEdit = GetNode<LineEdit>(lineEditPath);
	}

	public string GetInput() 
	{
		return lineEdit?.Text;
	}
}
