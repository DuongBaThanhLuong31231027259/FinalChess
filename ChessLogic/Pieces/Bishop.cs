namespace ChessLogic
{
    // Lớp Bishop kế thừa từ lớp Piece, đại diện cho quân "Tượng" trong cờ vua.
    public class Bishop : Piece
    {
        // Thuộc tính Type trả về loại quân cờ (Bishop).
        public override PieceType Type => PieceType.Bishop;

        // Thuộc tính Color trả về màu của quân cờ (do lớp "Piece" kế thừa từ "Player").
        public override Player Color { get; }

        // Mảng các hướng di chuyển của quân Tượng (diagonal: chéo), gồm 4 hướng.
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthWest,  // Di chuyển về phía Tây-Bắc
            Direction.NorthEast,  // Di chuyển về phía Đông-Bắc
            Direction.SouthWest,  // Di chuyển về phía Tây-Nam
            Direction.SouthEast   // Di chuyển về phía Đông-Nam
        };

        // Constructor khởi tạo quân Tượng với màu sắc (Player).
        public Bishop(Player color)
        {
            Color = color;  // Gán màu của quân cờ cho thuộc tính Color.
        }

        // Phương thức sao chép (copy) quân cờ Tượng, tạo một đối tượng mới sao chép thông tin từ quân cũ.
        public override Piece Copy()
        {
            // Tạo bản sao quân Tượng mới với cùng màu sắc.
            Bishop copy = new Bishop(Color);
            // Sao chép trạng thái đã di chuyển của quân cờ (HasMoved).
            copy.HasMoved = HasMoved;
            return copy;  // Trả về bản sao.
        }

        // Phương thức trả về các nước đi hợp lệ của quân Tượng từ vị trí "from" trên bàn cờ.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Gọi phương thức MovePositionsInDirs để lấy tất cả các vị trí di chuyển hợp lệ trong các hướng đã định nghĩa.
            // Sau đó, tạo ra các nước đi bình thường (NormalMove) từ vị trí "from" đến các vị trí có thể di chuyển.
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
