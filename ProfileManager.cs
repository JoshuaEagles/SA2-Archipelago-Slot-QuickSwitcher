using Godot;
using System;

public class ProfileManager : VBoxContainer
{
    [Export]
    NodePath itemListPath;
    ItemList itemList;

    [Export]
    NodePath setAsActiveButtonPath;
    Button setAsActiveButton;

    [Export]
    NodePath deleteButtonPath;
    Button deleteButton;

    public override void _Ready()
    {
        itemList = GetNode<ItemList>(itemListPath);
        setAsActiveButton = GetNode<Button>(setAsActiveButtonPath);
        deleteButton = GetNode<Button>(deleteButtonPath);

        PopulateProfileList();   

        var profileChangedSignalProvider = GetNode<ProfileChangedSignalProvider>("/root/ProfileChangedSignalProvider");
        profileChangedSignalProvider.Connect(nameof(ProfileChangedSignalProvider.profile_changed), this, nameof(PopulateProfileList));
    }

    void PopulateProfileList()
    {
        Directory directory = new Directory();

        var profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");
        
        itemList.Items.Clear();

        directory.Open(profileStorageDirectoryProvider.ProfileStorageDirectory);
        directory.ListDirBegin(skipNavigational:true);

        string filename = directory.GetNext();
        while (filename != "")
        {
            if (filename.EndsWith(".ini"))
            {
                string filenameWithoutExtension = filename.Replace(".ini", "");
                itemList.AddItem(filenameWithoutExtension);
            }

            filename = directory.GetNext();
        }
    }
}
