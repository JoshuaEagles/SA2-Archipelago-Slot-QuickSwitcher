using Godot;
using System;

public class LayoutSwitcher : Node
{
    bool isTabbedLayout = false;

    const string isTabbedLayoutStoragePath = "user://is-tabbed-layout";

    [Export]
    PackedScene singleScreenLayoutScene;
    [Export]
    PackedScene tabbedLayoutScene;

    public override void _Ready()
    {
        LoadIsTabbedLayoutFromFile();   

        if (isTabbedLayout)
        {
            SwitchToTabbedLayout();
        }
        else
        {
            SwitchToSingleScreenLayout();
        }
    }

    public void SwitchToSingleScreenLayout()
    {
        isTabbedLayout = false;

        SaveIsTabbedLayoutToFile(isTabbedLayout);

        GetTree().ChangeSceneTo(singleScreenLayoutScene);
    }

    public void SwitchToTabbedLayout()
    {
        isTabbedLayout = true;

        SaveIsTabbedLayoutToFile(isTabbedLayout);

        GetTree().ChangeSceneTo(tabbedLayoutScene);
    }

    void SaveIsTabbedLayoutToFile(bool isTabbedLayout)
    {
        var file = new File();

        file.Open(isTabbedLayoutStoragePath, File.ModeFlags.Write);

        file.StoreString(isTabbedLayout.ToString());

        file.Close();
    }

    void LoadIsTabbedLayoutFromFile()
    {
        var file = new File();

        if (!file.FileExists(isTabbedLayoutStoragePath))
        {
            return;
        }

        file.Open(isTabbedLayoutStoragePath, File.ModeFlags.Read);
        isTabbedLayout = file.GetAsText() == "True";

        file.Close();
    }
}
