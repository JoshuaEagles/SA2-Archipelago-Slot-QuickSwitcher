using Godot;
using System;

public class ModPathEntry : Control
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

		acceptDialog.Connect("confirmed", this, "OpenFileDialog");
		fileDialog.Connect("dir_selected", this, "SaveModDirectory");
	}

	public void PromptForPathEntry()
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
		file.Open("user://modpath.txt", File.ModeFlags.Write);
		file.StoreString(directory);
		file.Close();
	}
}
