using HappyCuber;

namespace HappyCuberTest;

public class PieceTests
{
    [Fact]
    public void Constructor_ValidInput_ShouldCreatePiece()
    {
        // Arrange
        ushort side = 0b1010_1100_1111_0000;
        
        // Act
        Piece piece = new(side);
        
        // Assert
        Assert.NotNull(piece);
    }

    [Fact]
    public void TopEdge_Getter_ShouldReturnCorrectValue()
    {
        // Arrange
        ushort side = 0b0001_0010_0100_1000; // Top edge is 0b01000 (8)
        Piece piece = new(side);
        
        // Act
        byte topEdge = piece.TopEdge;
        
        // Assert
        Assert.Equal(0b01000, topEdge);
    }

    [Fact]
    public void TopEdge_Setter_ShouldUpdateValue()
    {
        // Arrange
        ushort side = 0b0000_0000_0000_0000;
        Piece piece = new(side);
        
        // Act
        piece.TopEdge = 0b11111;
        byte topEdge = piece.TopEdge;
        
        // Assert
        Assert.Equal(0b11111, topEdge);
        Assert.Equal(0b0000_0000_0001_1111, Convert.ToUInt16(piece.ToString(), 2));
    }

    [Fact]
    public void RightEdge_Getter_ShouldReturnCorrectValue()
    {
        // Arrange
        ushort side = 0b0001_0010_0100_1000; // Right edge is 0b00100
        Piece piece = new(side);
        
        // Act
        byte rightEdge = piece.RightEdge;
        
        // Assert
        Assert.Equal(0b00100, rightEdge);
    }

    [Fact]
    public void RightEdge_Setter_ShouldUpdateValue()
    {
        // Arrange
        ushort side = 0b0000_0000_0000_0000;
        Piece piece = new(side);
        
        // Act
        piece.RightEdge = 0b11111;
        byte rightEdge = piece.RightEdge;
        
        // Assert
        Assert.Equal(0b11111, rightEdge);
        Assert.Equal(0b0000_0001_1111_0000, Convert.ToUInt16(piece.ToString(), 2));
    }

    [Fact]
    public void BottomEdge_Getter_ShouldReturnCorrectValue()
    {
        // Arrange
        ushort side = 0b0001_0010_0100_1000; // Bottom edge is 0b10010 (4)
        Piece piece = new(side);
        
        // Act
        byte bottomEdge = piece.BottomEdge;
        
        // Assert
        Assert.Equal(0b10010, bottomEdge);
    }

    [Fact]
    public void BottomEdge_Setter_ShouldUpdateValue()
    {
        // Arrange
        ushort side = 0b0000_0000_0000_0000;
        Piece piece = new(side);
        
        // Act
        piece.BottomEdge = 0b11111;
        byte bottomEdge = piece.BottomEdge;
        
        // Assert
        Assert.Equal(0b11111, bottomEdge);
        Assert.Equal(0b0001_1111_0000_0000, Convert.ToUInt16(piece.ToString(), 2));
    }

    [Theory]
    [InlineData(0b0000_0000_0000_0000, 0b00000)]
    [InlineData(0b0001_0000_0000_0000, 0b00001)]
    [InlineData(0b0010_0000_0000_0000, 0b00010)]
    [InlineData(0b0100_0000_0000_0000, 0b00100)]
    [InlineData(0b1000_0000_0000_0000, 0b01000)]
    [InlineData(0b0000_0000_0000_0001, 0b10000)]
    public void LeftEdge_Getter_ShouldReturnCorrectValue(ushort side, byte expectedEdge)
    {
        // Arrange
        Piece piece = new(side);
        
        // Act
        byte leftEdge = piece.LeftEdge;
        
        // Assert
        Assert.Equal(expectedEdge, leftEdge);
    }

    [Theory]
    [InlineData(0b00001, 0b0001_0000_0000_0000)]
    [InlineData(0b00010, 0b0010_0000_0000_0000)]
    [InlineData(0b00100, 0b0100_0000_0000_0000)]
    [InlineData(0b01000, 0b1000_0000_0000_0000)]
    [InlineData(0b10000, 0b0000_0000_0000_0001)]
    public void LeftEdge_Setter_ShouldUpdateValue(byte leftEdge, ushort expectedSide)
    {
        // Arrange
        Piece piece = new(0b0000_0000_0000_0000);
        
        // Act
        piece.LeftEdge = leftEdge;
        
        // Assert
        Assert.Equal(leftEdge, piece.LeftEdge);
        Assert.Equal(expectedSide, Convert.ToUInt16(piece.ToString(), 2));
    }

    [Fact]
    public void RotateClockwise_ShouldRotateEdgesCorrectly()
    {
        // Arrange
        // Initial edges:
        // Top    = 0b00001 (1)
        // Right  = 0b00010 (2)
        // Bottom = 0b00100 (4)
        // Left   = 0b11000 (24) (notice the 1 bit from the top edge wraps around to the MSB of the left edge)
        Piece piece = new(0b1000_0100_0010_0001);

        // Act
        piece.RotateClockwise();
        
        // Assert
        Assert.Equal(0b11000, piece.TopEdge);    // New Top    = Old Left   - value 0b11000 (24)
        Assert.Equal(0b00001, piece.RightEdge);  // New Right  = Old Top    - value 0b00001 (1)
        Assert.Equal(0b00010, piece.BottomEdge); // New Bottom = Old Right  - value 0b00010 (2)
        Assert.Equal(0b00100, piece.LeftEdge);   // New Left   = Old Bottom - value 0b00100 (4)
    }

    [Theory]
    [InlineData(0b0000_0000_0000_0000, "0000000000000000")]
    [InlineData(0b1111_1111_1111_1111, "1111111111111111")]
    [InlineData(0b1010_1010_1010_1010, "1010101010101010")]
    [InlineData(0b0101_0101_0101_0101, "0101010101010101")]
    public void ToString_ShouldReturnBinaryRepresentation(ushort side, string expectedString)
    {
        // Arrange
        Piece piece = new(side);
        
        // Act
        string binaryString = piece.ToString();
        
        // Assert
        Assert.Equal(expectedString, binaryString);
    }

    [Fact]
    public void FlipVertical_ShouldFlipEdgesCorrectly()
    {
        // Arrange
        // Initial edges:
        // Top    = 0b00001 (1)
        // Right  = 0b00010 (2)
        // Bottom = 0b00100 (4)
        // Left   = 0b11000 (24) (notice the 1 bit from the top edge wraps around to the MSB of the left edge)
        Piece piece                = new(0b_0110_1001_1010_0110);
        Piece expectedFlippedPiece = new(0b_1011_0010_1100_1100);

        // Act
        piece.FlipVertical();
        
        // Assert
        Assert.Equal(expectedFlippedPiece.ToString(), piece.ToString());

        

    }
}