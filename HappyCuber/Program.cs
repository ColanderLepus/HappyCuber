namespace HappyCuber;

internal static class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, Happy Cuber!");

        Piece piece = new(0b0001_0010_0100_1000);
        
        Console.WriteLine(piece);
        Console.WriteLine($"Top Edge: {Convert.ToString(piece.TopEdge, 2).PadLeft(5, '0')}");
    }
}