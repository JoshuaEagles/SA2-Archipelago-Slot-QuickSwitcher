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

		modDirectoryEntry.Connect(nameof(ModDirectoryEntry.mod_directory_updated), this, nameof(CreateProfileStorageDirectory));

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
		modDirectoryEntry.PromptForDirectoryEntry();
	}

	void CreateProfileStorageDirectory(string directory)
	{
		Directory gdDirectory = new Directory();

		gdDirectory.MakeDir($"{directory}/QuickSwitchProfiles");
	}
}
