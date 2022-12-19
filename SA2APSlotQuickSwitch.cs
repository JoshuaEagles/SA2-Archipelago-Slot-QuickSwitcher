using Godot;
using System;

public class SA2APSlotQuickSwitch : Control
{
	[Export]
	NodePath modDirectoryEntryPath;
	ModDirectoryEntry modDirectoryEntry;

	public override void _Ready()
	{
		modDirectoryEntry = GetNode<ModDirectoryEntry>(modDirectoryEntryPath);

		ProfileStorageDirectoryProvider profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");
		if (!profileStorageDirectoryProvider.IsModDirectorySaved())
		{
			PromptForModDirectory();
		}
	}

	void PromptForModDirectory()
	{
		modDirectoryEntry.PromptForDirectoryEntry();
	}
}
