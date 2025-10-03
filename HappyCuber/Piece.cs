using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyCuber;

public class Piece(ushort side)
{
    private const byte EdgeBitMask = 0b11111; // 5 bits set to 1

    private ushort Side { get; set; } = side;

    public byte TopEdge
    {
        get => (byte)(Side & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~EdgeBitMask);
            Side = (ushort)((Side & clearMask) | value);
        }
    }

    public byte RightEdge
    {
        get => (byte)((Side >> 4) & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~(EdgeBitMask << 4));
            Side = (ushort)((Side & clearMask) | (value << 4));
        }
    }

    public byte BottomEdge
    {
        get => (byte)((Side >> 8) & EdgeBitMask);
        set
        {
            ushort clearMask = unchecked((ushort)~(EdgeBitMask << 8));
            Side = (ushort)((Side & clearMask) | (value << 8));
        }
    }

    public byte LeftEdge
    {
        get
        {
            byte l0 = (byte)(Side & 1);
            byte l4 = (byte)((Side >> 12) & 0b1111);

            return (byte)(l0 << 4 | l4);
        }
        set
        {
            // This is now much simpler as there are only two contiguous blocks to set!

            // 1. Set the S0 bit from the L4 (MSB) of 'value'
            //    (value bit 4 is 0b10000)
            ushort s0_value = (ushort)((value & 0b10000) >> 4); // Get L4
            Side = (ushort)((Side & unchecked((ushort)~1)) | s0_value); // Set S0

            // 2. Set the S12-S15 block from the L0-L3 (LSBs) of 'value'
            //    (value bits 0-3 are 0b1111)
            ushort s12_to_s15_value = (ushort)(value & 0b1111); // Get L0-L3
            ushort clearMask = unchecked((ushort)~(0b1111 << 12)); // Clear S12-S15
            Side = (ushort)((Side & clearMask) | (s12_to_s15_value << 12));
        }
    }

    public void RotateClockwise()
    {
        Side = (ushort)((Side << 4) | (Side >> 12));
    }

    public void FlipVertical()
    {
        // Step 1: Reverse the entire 16-bit field.
        ushort reversed = Reverse16Bits(Side);

        // Step 2: Cyclically shift the reversed result 5 positions to the right.
        // k=5, so 16-k = 11.
        ushort shifted = (ushort)((reversed >> 5) | (reversed << 11));

        Side = shifted;
    }

    public override string ToString()
    {
        return Convert.ToString(Side, 2).PadLeft(16, '0');
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
}