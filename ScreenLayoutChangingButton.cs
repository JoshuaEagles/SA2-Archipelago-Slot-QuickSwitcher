using Godot;
using System;

public class ScreenLayoutChangingButton : Button
{
    [Export]
    bool isChangeToTabbed = false;

    public override void _Ready()
    {
        Connect("pressed", this, nameof(ChangeScreenLayout)); 
    }

    void ChangeScreenLayout()
    {
        var layoutSwitcher = GetNode<LayoutSwitcher>("/root/LayoutSwitcher");

        if (isChangeToTabbed)
        {
            layoutSwitcher.SwitchToTabbedLayout();
        }
        else
        {
            layoutSwitcher.SwitchToSingleScreenLayout();
        }
    }
}
