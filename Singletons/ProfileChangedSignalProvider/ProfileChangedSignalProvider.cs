using Godot;
using System;

public class ProfileChangedSignalProvider : Node
{
    [Signal]
    public delegate void profile_changed();
}
