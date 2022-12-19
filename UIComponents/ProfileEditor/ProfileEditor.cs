using Godot;
using System;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Model.Formatting;
using IniParser.Parser;
using System.Text;

public class ProfileEditor : VBoxContainer
{
    [Export]
    NodePath saveButtonPath;
    Button saveButton;

    [Export]
    NodePath profileNamePath;
    [Export]
    NodePath serverIPPath;
    [Export]
    NodePath playerNamePath;
    [Export]
    NodePath serverPasswordPath;

    [Export]
    NodePath messageDisplayCountPath;
    [Export]
    NodePath messageDisplayDurationPath;
    [Export]
    NodePath messageFontSizePath;
    [Export]
    NodePath messageColorRedPath;
    [Export]
    NodePath messageColorGreenPath;
    [Export]
    NodePath messageColorBluePath;

    ProfileStorageDirectoryProvider profileStorageDirectoryProvider;

    public override void _Ready()
    {
        saveButton = GetNode<Button>(saveButtonPath);
        saveButton.Connect("pressed", this, nameof(SaveProfileToFile));

        profileStorageDirectoryProvider = GetNode<ProfileStorageDirectoryProvider>("/root/ProfileStorageDirectoryProvider");

        var profileSelectedSignalProvider = GetNode<ProfileSelectedSignalProvider>("/root/ProfileSelectedSignalProvider");
        profileSelectedSignalProvider.Connect("profile_selected", this, nameof(LoadProfileFromFile));

        profileSelectedSignalProvider.Connect("profile_selected", this, nameof(CurrentActiveProfileSaveDisable));
        GetNode(profileNamePath).Connect("text_changed", this, nameof(CurrentActiveProfileSaveDisable));
    }

    public void LoadProfileFromFile(string profileName)
    {
        var fileParser = new FileIniDataParser();

        IniData profileData;
        if (profileName == "Current Active Profile")
        {
            profileData = fileParser.ReadFile($"{profileStorageDirectoryProvider.ProfileStorageDirectory}/../config.ini");
        }
        else
        {
            profileData = fileParser.ReadFile($"{profileStorageDirectoryProvider.ProfileStorageDirectory}/{profileName}.ini");
        }

        profileData.Configuration = GetParserConfiguration();

        GetNode<InputField>(profileNamePath).Text = profileName;
        GetNode<InputField>(serverIPPath).Text = profileData["AP"]["IP"];
        GetNode<InputField>(playerNamePath).Text = profileData["AP"]["PlayerName"];
        GetNode<InputField>(serverPasswordPath).Text = profileData["AP"]["Password"];

        GetNode<InputField>(messageDisplayCountPath).Text = profileData["General"]["MessageDisplayCount"];
        GetNode<InputField>(messageDisplayDurationPath).Text = profileData["General"]["MessageDisplayDuration"];
        GetNode<InputField>(messageFontSizePath).Text = profileData["General"]["MessageFontSize"];
        GetNode<InputField>(messageColorRedPath).Text = profileData["General"]["MessageColorR"];
        GetNode<InputField>(messageColorGreenPath).Text = profileData["General"]["MessageColorG"];
        GetNode<InputField>(messageColorBluePath).Text = profileData["General"]["MessageColorB"];
    }

    void SaveProfileToFile() 
    {
        string profileName = GetNode<InputField>(profileNamePath).Text;

        IniData profileData = new IniData();
        profileData.Configuration = GetParserConfiguration();

        SaveOnlyIfExists(profileData, "AP", "IP", GetNode<InputField>(serverIPPath).Text);
        SaveOnlyIfExists(profileData, "AP", "PlayerName", GetNode<InputField>(playerNamePath).Text);
        SaveOnlyIfExists(profileData, "AP", "Password", GetNode<InputField>(serverPasswordPath).Text);

        SaveOnlyIfExists(profileData, "General", "MessageDisplayCount", GetNode<InputField>(messageDisplayCountPath).Text);
        SaveOnlyIfExists(profileData, "General", "MessageDisplayDuration", GetNode<InputField>(messageDisplayDurationPath).Text);
        SaveOnlyIfExists(profileData, "General", "MessageFontSize", GetNode<InputField>(messageFontSizePath).Text);
        SaveOnlyIfExists(profileData, "General", "MessageColorR", GetNode<InputField>(messageColorRedPath).Text);
        SaveOnlyIfExists(profileData, "General", "MessageColorG", GetNode<InputField>(messageColorGreenPath).Text);
        SaveOnlyIfExists(profileData, "General", "MessageColorB", GetNode<InputField>(messageColorBluePath).Text);

        var profilePath = $"{profileStorageDirectoryProvider.ProfileStorageDirectory}/{profileName}.ini";

        byte[] profileDataByteArray = profileData.ToString().ToUTF8();
        // byte[] profileDataByteArrayNoBOM = new byte[profileDataByteArray.Length - 2];
        // // The first two bytes are a BOM, get rid of them
        // Array.Copy(profileDataByteArray, 2, profileDataByteArrayNoBOM, 0, profileDataByteArrayNoBOM.Length);

        var file = new File();
        file.Open(profilePath, File.ModeFlags.Write);
        file.StoreBuffer(profileDataByteArray);
        file.Close();

        var profileChangedSignalProvider = GetNode<ProfileChangedSignalProvider>("/root/ProfileChangedSignalProvider");
        profileChangedSignalProvider.EmitSignal(nameof(ProfileChangedSignalProvider.profile_changed));
    }

    IniParserConfiguration GetParserConfiguration()
    {
        var parserConfiguration = new IniParserConfiguration();

        parserConfiguration.AssigmentSpacer = "";

        return parserConfiguration;
    }

    void SaveOnlyIfExists(IniData profileData, string section, string key, string value)
    {
        if (value == "")
        {
            return;
        }

        profileData[section][key] = value;
    }

    void CurrentActiveProfileSaveDisable(string profileName)
    {
        saveButton.Disabled = profileName == "Current Active Profile";
    }
}
