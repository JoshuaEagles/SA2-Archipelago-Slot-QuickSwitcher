using Godot;
using System;

public class SetModDirectoryButton : Button
{
	[Export]
	NodePath modPathEntryPath;

	public override void _Ready()
	{
		ModPathEntry modPathEntry = GetNode<ModPathEntry>(modPathEntryPath);

		Connect("pressed", modPathEntry, "PromptForPathEntry");
	}
}
