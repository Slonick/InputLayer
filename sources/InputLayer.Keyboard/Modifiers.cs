namespace InputLayer.Keyboard
{
    // Правильные виртуальные коды Windows (VK_*)
    public enum Modifiers : ushort
    {
        None = 0x00,
        LeftShift = 0x10, // VK_SHIFT
        RightShift = 0x10, // VK_SHIFT (правый обрабатывается через extended flag)
        LeftControl = 0x11, // VK_CONTROL
        RightControl = 0x11, // VK_CONTROL (правый обрабатывается через extended flag)
        LeftAlt = 0x12, // VK_MENU (Alt)
        RightAlt = 0x12, // VK_MENU (правый обрабатывается через extended flag)
        LeftWin = 0x5B, // VK_LWIN
        RightWin = 0x5C // VK_RWIN
    }
}