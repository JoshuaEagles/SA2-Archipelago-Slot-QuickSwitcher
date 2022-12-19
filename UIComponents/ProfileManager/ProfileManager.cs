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
    ProfileSelectedSignalProvider profileSelectedSignalProvider;

    public override void _Ready()
    {
        itemList = GetNode<ItemList>(itemListPath);
        setAsActiveButton = GetNode<Button>(setAsActiveButtonPath);
        deleteButton = GetNode<Button>(deleteButtonPath);

        profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");
        profileChangedSignalProvider = GetNode<ProfileChangedSignalProvider>("/root/ProfileChangedSignalProvider");
        profileSelectedSignalProvider = GetNode<ProfileSelectedSignalProvider>("/root/ProfileSelectedSignalProvider");

        PopulateProfileList();   

        profileChangedSignalProvider.Connect(nameof(ProfileChangedSignalProvider.profile_changed), this, nameof(PopulateProfileList));

        itemList.Connect("item_selected", this, nameof(EmitProfileSelectedSignal));

        setAsActiveButton.Connect("pressed", this, nameof(SetProfileAsActive));
        deleteButton.Connect("pressed", this, nameof(DeleteProfile));

        profileSelectedSignalProvider.Connect(nameof(ProfileSelectedSignalProvider.profile_selected), this, nameof(CurrentActiveProfileButtonDisable));
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

        itemList.SortItemsByText();

        itemList.AddItem("Current Active Profile");
        int lastItemIndex = itemList.GetItemCount() - 1;

        itemList.MoveItem(lastItemIndex, 0);
    }

    void SetProfileAsActive()
    {
        if (!itemList.IsAnythingSelected())
        {
            return;
        }

        var directory = new Directory();

        string selectedProfileFilepath = GetSelectedProfileFilepath();

        directory.Copy(selectedProfileFilepath, profileStorageDirectoryProvider.GetModConfigPath());
    }

    void DeleteProfile()
    {
        if (!itemList.IsAnythingSelected())
        {
            return;
        }

        string selectedProfileFilepath = GetSelectedProfileFilepath();
        int selectedindex = itemList.GetSelectedItems()[0];

        var directory = new Directory();
        directory.Remove(selectedProfileFilepath);

        profileChangedSignalProvider.EmitSignal(nameof(ProfileChangedSignalProvider.profile_changed));

        if (itemList.Items.Count != 0)
        {
            int selectedIndex = Mathf.Min(selectedindex + 1, itemList.GetItemCount()) - 1;
            itemList.Select(selectedIndex);
            itemList.EmitSignal("item_selected", selectedIndex);
        }
    }

    string GetSelectedProfileName()
    {
        int selectedIndex = itemList.GetSelectedItems()[0];
        // Godot stores the items as an untyped array in a really awkward way
        // it's completely flat, and for each item put in the first index is the name, the second is the icon (or null), and the third is an enabled toggle
        // eg: ["myprofile", [Object:null], False, "myotherprofile", [Object:null], False]
        return itemList.Items[(selectedIndex * 3)].ToString();
    }

    string GetSelectedProfileFilepath()
    {
        return $"{profileStorageDirectoryProvider.ProfileStorageDirectory}/{GetSelectedProfileName()}.ini";
    }

    // need the method signature, even if we dont use the argument
    void EmitProfileSelectedSignal(int selectedIndex)
    {
        string selectedProfileName = GetSelectedProfileName();

        profileSelectedSignalProvider.EmitProfileSelectedSignal(selectedProfileName);
    }

    void CurrentActiveProfileButtonDisable(string profileName)
    {
        setAsActiveButton.Disabled = profileName == "Current Active Profile";
        deleteButton.Disabled = profileName == "Current Active Profile";
    }
}
