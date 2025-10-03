namespace HappyCuber;

public class Piece(ushort side)
{
    private const byte EdgeBitMask = 0b11111; // 5 bits set to 1

    private ushort _side = side;

    public byte TopEdge
    {
        get => (byte)(_side & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~EdgeBitMask);
            _side = (ushort)((_side & clearMask) | value);
        }
    }

    public byte RightEdge
    {
        get => (byte)((_side >> 4) & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~(EdgeBitMask << 4));
            _side = (ushort)((_side & clearMask) | (value << 4));
        }
    }

    public byte BottomEdge
    {
        get => (byte)((_side >> 8) & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~(EdgeBitMask << 8));
            _side = (ushort)((_side & clearMask) | (value << 8));
        }
    }

    public byte LeftEdge
    {
        get
        {
            byte l0 = (byte)(_side & 1);
            byte l4 = (byte)((_side >> 12) & 0b1111);

            return (byte)(l0 << 4 | l4);
        }
        set
        {
            // This is now much simpler as there are only two contiguous blocks to set!

            // 1. Set the S0 bit from the L4 (MSB) of 'value'
            //    (value bit 4 is 0b10000)
            ushort s0_value = (ushort)((value & 0b10000) >> 4); // Get L4
            _side = (ushort)((_side & unchecked((ushort)~1)) | s0_value); // Set S0

            // 2. Set the S12-S15 block from the L0-L3 (LSBs) of 'value'
            //    (value bits 0-3 are 0b1111)
            ushort s12_to_s15_value = (ushort)(value & 0b1111); // Get L0-L3
            ushort clearMask = unchecked((ushort)~(0b1111 << 12)); // Clear S12-S15
            _side = (ushort)((_side & clearMask) | (s12_to_s15_value << 12));
        }
    }

    public void RotateClockwise()
    {
        _side = (ushort)((_side << 4) | (_side >> 12));
    }

    public void FlipVertical()
    {
        // Step 1: Reverse the entire 16-bit field.
        ushort reversed = Reverse16Bits(_side);

        // Step 2: Cyclically shift the reversed result 5 positions to the right.
        // k=5, so 16-k = 11.
        ushort shifted = (ushort)((reversed >> 5) | (reversed << 11));

        _side = shifted;
    }

    public override string ToString()
    {
        return Convert.ToString(_side, 2).PadLeft(16, '0');
    }

    private static ushort Reverse16Bits(ushort n)
    {
        // Sequence of parallel swaps for maximum speed on a 16-bit integer
        n = (ushort)((n >> 8) | (n << 8));
        n = (ushort)(((n & 0xF0F0) >> 4) | ((n & 0x0F0F) << 4));
        n = (ushort)(((n & 0xCCCC) >> 2) | ((n & 0x3333) << 2));
        n = (ushort)(((n & 0xAAAA) >> 1) | ((n & 0x5555) << 1));

        return n;
    }

    public static bool EdgesFit(byte edgeA, byte edgeB)
    {
        // Example: edges fit if they are bitwise complements (all bits opposite)
        return (edgeA ^ edgeB) == 0b11111;
    }
}