namespace InputLayer.Keyboard
{
    public enum Keys : ushort
    {
        None = 0x00,

        // === MOUSE BUTTONS (для полноты, но обычно не используются в клавиатурном вводе) ===
        LButton = 0x01, // Left mouse button
        RButton = 0x02, // Right mouse button
        Cancel = 0x03, // Control-break processing
        MButton = 0x04, // Middle mouse button
        XButton1 = 0x05, // X1 mouse button
        XButton2 = 0x06, // X2 mouse button

        // === CONTROL KEYS ===
        Backspace = 0x08, // BACKSPACE key
        Tab = 0x09, // TAB key
        Clear = 0x0C, // CLEAR key
        Enter = 0x0D, // ENTER key (Return)

        // === MODIFIERS ===
        Shift = 0x10, // SHIFT key (любая)
        Control = 0x11, // CTRL key (любая)
        Alt = 0x12, // ALT key (любая) / MENU
        Pause = 0x13, // PAUSE key
        CapsLock = 0x14, // CAPS LOCK key

        // === IME KEYS (для азиатских языков) ===
        Kana = 0x15, // IME Kana mode
        Hangul = 0x15, // IME Hangul mode (тот же код)
        ImeOn = 0x16, // IME On
        Junja = 0x17, // IME Junja mode
        Final = 0x18, // IME final mode
        Hanja = 0x19, // IME Hanja mode
        Kanji = 0x19, // IME Kanji mode (тот же код)
        ImeOff = 0x1A, // IME Off

        // === SPECIAL KEYS ===
        Escape = 0x1B, // ESC key
        Convert = 0x1C, // IME convert
        NonConvert = 0x1D, // IME nonconvert
        Accept = 0x1E, // IME accept
        ModeChange = 0x1F, // IME mode change request

        Space = 0x20, // SPACEBAR

        // === NAVIGATION ===
        PageUp = 0x21, // PAGE UP key
        PageDown = 0x22, // PAGE DOWN key
        End = 0x23, // END key
        Home = 0x24, // HOME key

        // === ARROW KEYS ===
        Left = 0x25, // LEFT ARROW key
        Up = 0x26, // UP ARROW key
        Right = 0x27, // RIGHT ARROW key
        Down = 0x28, // DOWN ARROW key

        // === ADDITIONAL NAVIGATION ===
        Select = 0x29, // SELECT key
        Print = 0x2A, // PRINT key
        Execute = 0x2B, // EXECUTE key
        PrintScreen = 0x2C, // PRINT SCREEN key
        Insert = 0x2D, // INS key
        Delete = 0x2E, // DEL key
        Help = 0x2F, // HELP key

        // === NUMBER ROW (0-9) ===
        D0 = 0x30, // 0 key
        D1 = 0x31, // 1 key
        D2 = 0x32, // 2 key
        D3 = 0x33, // 3 key
        D4 = 0x34, // 4 key
        D5 = 0x35, // 5 key
        D6 = 0x36, // 6 key
        D7 = 0x37, // 7 key
        D8 = 0x38, // 8 key
        D9 = 0x39, // 9 key

        // 0x3A-0x40 зарезервированы

        // === LETTERS (A-Z) ===
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // === WINDOWS KEYS ===
        LeftWin = 0x5B, // Left Windows key
        RightWin = 0x5C, // Right Windows key
        Apps = 0x5D, // Applications key (контекстное меню)

        // 0x5E зарезервирован
        Sleep = 0x5F, // Computer Sleep key

        // === NUMERIC KEYPAD ===
        NumPad0 = 0x60, // Numeric keypad 0
        NumPad1 = 0x61, // Numeric keypad 1
        NumPad2 = 0x62, // Numeric keypad 2
        NumPad3 = 0x63, // Numeric keypad 3
        NumPad4 = 0x64, // Numeric keypad 4
        NumPad5 = 0x65, // Numeric keypad 5
        NumPad6 = 0x66, // Numeric keypad 6
        NumPad7 = 0x67, // Numeric keypad 7
        NumPad8 = 0x68, // Numeric keypad 8
        NumPad9 = 0x69, // Numeric keypad 9
        Multiply = 0x6A, // Multiply key (*)
        Add = 0x6B, // Add key (+)
        Separator = 0x6C, // Separator key
        Subtract = 0x6D, // Subtract key (-)
        Decimal = 0x6E, // Decimal key (.)
        Divide = 0x6F, // Divide key (/)

        // === FUNCTION KEYS (F1-F24) ===
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,

        // 0x88-0x8F не назначены

        // === LOCK KEYS ===
        NumLock = 0x90, // NUM LOCK key
        ScrollLock = 0x91, // SCROLL LOCK key

        // === OEM SPECIFIC ===
        // 0x92-0x96 OEM specific

        // 0x97-0x9F не назначены

        // === LEFT/RIGHT MODIFIERS (для различения левой/правой клавиши) ===
        LeftShift = 0xA0, // Left SHIFT key
        RightShift = 0xA1, // Right SHIFT key
        LeftControl = 0xA2, // Left CONTROL key
        RightControl = 0xA3, // Right CONTROL key
        LeftAlt = 0xA4, // Left MENU key (Alt)
        RightAlt = 0xA5, // Right MENU key (Alt)

        // === BROWSER KEYS ===
        BrowserBack = 0xA6, // Browser Back key
        BrowserForward = 0xA7, // Browser Forward key
        BrowserRefresh = 0xA8, // Browser Refresh key
        BrowserStop = 0xA9, // Browser Stop key
        BrowserSearch = 0xAA, // Browser Search key
        BrowserFavorites = 0xAB, // Browser Favorites key
        BrowserHome = 0xAC, // Browser Start and Home key

        // === MEDIA KEYS ===
        VolumeMute = 0xAD, // Volume Mute key
        VolumeDown = 0xAE, // Volume Down key
        VolumeUp = 0xAF, // Volume Up key
        MediaNextTrack = 0xB0, // Next Track key
        MediaPrevTrack = 0xB1, // Previous Track key
        MediaStop = 0xB2, // Stop Media key
        MediaPlayPause = 0xB3, // Play/Pause Media key

        // === LAUNCH KEYS ===
        LaunchMail = 0xB4, // Start Mail key
        LaunchMediaSelect = 0xB5, // Select Media key
        LaunchApp1 = 0xB6, // Start Application 1 key
        LaunchApp2 = 0xB7, // Start Application 2 key

        // 0xB8-0xB9 зарезервированы

        // === OEM KEYS (символьные клавиши, зависят от раскладки) ===
        OemSemicolon = 0xBA, // ;: key (US keyboard)
        Oem1 = 0xBA, // Альтернативное название

        OemPlus = 0xBB, // =+ key

        OemComma = 0xBC, // ,< key

        OemMinus = 0xBD, // -_ key

        OemPeriod = 0xBE, // .> key

        OemQuestion = 0xBF, // /? key
        Oem2 = 0xBF, // Альтернативное название

        OemTilde = 0xC0, // `~ key
        Oem3 = 0xC0, // Альтернативное название

        // 0xC1-0xD7 зарезервированы

        // 0xD8-0xDA не назначены

        OemOpenBrackets = 0xDB, // [{ key
        Oem4 = 0xDB, // Альтернативное название

        OemPipe = 0xDC, // \| key
        Oem5 = 0xDC, // Альтернативное название

        OemCloseBrackets = 0xDD, // ]} key
        Oem6 = 0xDD, // Альтернативное название

        OemQuotes = 0xDE, // '" key
        Oem7 = 0xDE, // Альтернативное название

        Oem8 = 0xDF, // Различные клавиши

        // 0xE0 зарезервирован

        // 0xE1 OEM specific

        OemBackslash = 0xE2, // <> или \| на RT 102-key keyboard
        Oem102 = 0xE2, // Альтернативное название

        // 0xE3-0xE4 OEM specific

        ProcessKey = 0xE5, // IME PROCESS key

        // 0xE6 OEM specific

        Packet = 0xE7, // Used to pass Unicode characters

        // 0xE8 не назначен

        // 0xE9-0xF5 OEM specific

        Attn = 0xF6, // Attn key
        Crsel = 0xF7, // CrSel key
        Exsel = 0xF8, // ExSel key
        EraseEof = 0xF9, // Erase EOF key
        Play = 0xFA, // Play key
        Zoom = 0xFB, // Zoom key
        NoName = 0xFC, // Reserved
        Pa1 = 0xFD, // PA1 key
        OemClear = 0xFE // Clear key
    }
}