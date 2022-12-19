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

    ProfileStorageDirectoryProvider profileStorageDirectoryProvider;
    ProfileChangedSignalProvider profileChangedSignalProvider;

    public override void _Ready()
    {
        itemList = GetNode<ItemList>(itemListPath);
        setAsActiveButton = GetNode<Button>(setAsActiveButtonPath);
        deleteButton = GetNode<Button>(deleteButtonPath);

        profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");
        profileChangedSignalProvider = GetNode<ProfileChangedSignalProvider>("/root/ProfileChangedSignalProvider");

        PopulateProfileList();   

        profileChangedSignalProvider.Connect(nameof(ProfileChangedSignalProvider.profile_changed), this, nameof(PopulateProfileList));

        setAsActiveButton.Connect("pressed", this, nameof(SetProfileAsActive));
        deleteButton.Connect("pressed", this, nameof(DeleteProfile));
    }

    void PopulateProfileList()
    {
        Directory directory = new Directory();

        itemList.Clear();

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

    void SetProfileAsActive()
    {
        if (!itemList.IsAnythingSelected())
        {
            return;
        }
    }

    void DeleteProfile()
    {
        if (!itemList.IsAnythingSelected())
        {
            return;
        }

        int selectedindex = itemList.GetSelectedItems()[0];
        // Godot stores the items as an untyped array in a really awkward way
        // it's completely flat, and for each item put in the first index is the name, the second is the icon (or null), and the third is an enabled toggle
        // eg: ["myprofile", [Object:null], False, "myotherprofile", [Object:null], False]
        string selectedProfileFilename = itemList.Items[(selectedindex * 3)].ToString();
        selectedProfileFilename += ".ini";

        var directory = new Directory();
        directory.Remove($"{profileStorageDirectoryProvider.ProfileStorageDirectory}/{selectedProfileFilename}");

        profileChangedSignalProvider.EmitSignal(nameof(ProfileChangedSignalProvider.profile_changed));

        GD.Print(itemList.Items.Count);
        if (itemList.Items.Count != 0)
        {
            itemList.Select(Mathf.Min(selectedindex, itemList.Items.Count / 3) - 1);
        }
    }
}
