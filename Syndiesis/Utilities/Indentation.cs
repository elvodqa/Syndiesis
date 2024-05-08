﻿namespace Syndiesis.Utilities;

public record struct Indentation(char Character, int Width)
{
    public static readonly Indentation FourSpaces = new(WhitespaceFacts.Space, 4);
    public static readonly Indentation EightSpaces = new(WhitespaceFacts.Space, 8);
    public static readonly Indentation SingleTab = new(WhitespaceFacts.Tab, 1);
}
