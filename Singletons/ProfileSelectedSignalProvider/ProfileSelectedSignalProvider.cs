using Godot;
using System;

public class ProfileSelectedSignalProvider : Node
{
    [Signal]
    public delegate void profile_selected(string profileName);

    public void EmitProfileSelectedSignal(string profileName)
    {
        EmitSignal(nameof(profile_selected), profileName);
    }
}
