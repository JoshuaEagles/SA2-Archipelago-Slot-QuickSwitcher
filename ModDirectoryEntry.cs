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

	[Signal]
	public delegate void mod_directory_updated(string directory);

	public override void _Ready()
	{
		acceptDialog = GetNode<AcceptDialog>(acceptDialogPath);
		fileDialog = GetNode<FileDialog>(fileDialogPath);

		acceptDialog.Connect("confirmed", this, nameof(OpenFileDialog));
		fileDialog.Connect("dir_selected", this, nameof(SaveModDirectory));
	}

	public void PromptForDirectoryEntry()
	{
		acceptDialog.PopupCentered();	
	}

	void OpenFileDialog()
	{
		fileDialog.PopupCentered();
	}

	void SaveModDirectory(string directory)
	{
		File file = new File();
		file.Open("user://mod_directory.txt", File.ModeFlags.Write);
		file.StoreString(directory);
		file.Close();

		EmitSignal(nameof(mod_directory_updated), directory);
	}
}
