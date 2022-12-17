using Godot;
using System;

public class SA2APSlotQuickSwitch : Control
{
	[Export]
	NodePath modDirectoryEntryPath;

	public override void _Ready()
	{
		if (!IsModDirectorySaved())
		{
			PromptForModDirectory();
		}
	}

	bool IsModDirectorySaved()
	{
		return new File().FileExists("user://mod_directory.txt");
	}

	void PromptForModDirectory()
	{
		ModDirectoryEntry modDirectoryEntry = GetNode<ModDirectoryEntry>(modDirectoryEntryPath);
		modDirectoryEntry.PromptForDirectoryEntry();
	}
}
