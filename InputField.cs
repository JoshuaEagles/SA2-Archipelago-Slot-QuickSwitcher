using Godot;
using System;

public class InputField : MarginContainer
{
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
	}
}
