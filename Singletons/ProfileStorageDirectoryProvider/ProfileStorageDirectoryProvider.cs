using Godot;
using System;

public class ProfileStorageDirectoryProvider : Node
{
	[Signal]
	public delegate void mod_directory_updated(string directory);

    string _profileStorageDirectory = null;
    public string ProfileStorageDirectory 
    { 
        get 
        {
            if (_profileStorageDirectory != null)
            {
                return _profileStorageDirectory;
            }

            File file = new File();   
            if (file.FileExists(modDirectoryStoragePath))
            {
                file.Open(modDirectoryStoragePath, File.ModeFlags.Read);
                _profileStorageDirectory = file.GetAsText();
                return _profileStorageDirectory;
            }

            return null;
        }
    }

    const string modDirectoryStoragePath = "user://mod_directory.txt";
    const string profileStorageDirectoryName = "QuickSwitchProfiles";

	public bool IsModDirectorySaved()
	{
		return new File().FileExists(modDirectoryStoragePath);
	}

	public void SaveProfileStorageDirectory(string modDirectory)
	{
        var profileStorageDirectory = $"{modDirectory}/{profileStorageDirectoryName}";

		File file = new File();
		file.Open("user://mod_directory.txt", File.ModeFlags.Write);
		file.StoreString(profileStorageDirectory);
		file.Close();

        _profileStorageDirectory = profileStorageDirectory;

		CreateProfileStorageDirectory(modDirectory);

        GetNode("/root/ProfileChangedSignalProvider").EmitSignal("profile_changed");
	}

	void CreateProfileStorageDirectory(string modDirectory)
	{
		Directory gdDirectory = new Directory();

		gdDirectory.MakeDir($"{modDirectory}/{profileStorageDirectoryName}");
	}

    public string GetModConfigPath()
    {
        return $"{ProfileStorageDirectory}/../config.ini";
    }
}
