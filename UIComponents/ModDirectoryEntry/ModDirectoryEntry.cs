using Godot;
using System;

public class ModDirectoryEntry : Control
{
	[Export]
	NodePath acceptDialogPath;
	AcceptDialog acceptDialog;

	[Export]
	NodePath fileDialogPath;
	FileDialog fileDialog;

	public override void _Ready()
	{
		acceptDialog = GetNode<AcceptDialog>(acceptDialogPath);
		fileDialog = GetNode<FileDialog>(fileDialogPath);

		var profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");

		acceptDialog.Connect("confirmed", this, nameof(OpenFileDialog));
		fileDialog.Connect("dir_selected", profileStorageDirectoryProvider, nameof(profileStorageDirectoryProvider.SaveProfileStorageDirectory));
	}

	public void PromptForDirectoryEntry()
	{
		acceptDialog.PopupCentered();	
	}

	void OpenFileDialog()
	{
		fileDialog.PopupCentered();
	}
}
