using Godot;
using System;

public class SetModDirectoryButton : Button
{
	[Export]
	NodePath modDirectoryEntryPath;

	public override void _Ready()
	{
		ModDirectoryEntry modDirectoryEntry = GetNode<ModDirectoryEntry>(modDirectoryEntryPath);

		Connect("pressed", modDirectoryEntry, "PromptForPathEntry");
	}
}
