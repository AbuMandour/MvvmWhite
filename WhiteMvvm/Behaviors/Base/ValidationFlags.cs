using System;

namespace WhiteMvvm.Behaviors.Base
{
    [Flags]
    public enum ValidationFlags
    {
        None = 0,
        ValidateOnAttaching = 1,
        ValidateOnFocusing = 2,
        ValidateOnUnfocusing = 4,
        ValidateOnValueChanging = 8,
        ForceMakeValidWhenFocused = 16
    }
}