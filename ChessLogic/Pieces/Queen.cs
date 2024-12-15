namespace ChessLogic
{
    // Lớp Queen kế thừa từ lớp Piece, đại diện cho quân "Queen" (Hậu) trong cờ vua.
    public class Queen : Piece
    {
        // Thuộc tính Type trả về loại quân cờ (Queen).
        public override PieceType Type => PieceType.Queen;

        // Thuộc tính Color trả về màu của quân cờ (do lớp "Piece" kế thừa từ "Player").
        public override Player Color { get; }

        // Mảng các hướng di chuyển của quân Hậu (kết hợp giữa các hướng của Xe và Tượng).
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,      // Di chuyển lên phía Bắc (tăng theo trục y).
            Direction.South,      // Di chuyển xuống phía Nam (giảm theo trục y).
            Direction.East,       // Di chuyển sang phía Đông (tăng theo trục x).
            Direction.West,       // Di chuyển sang phía Tây (giảm theo trục x).
            Direction.NorthWest,  // Di chuyển lên phía Bắc và sang phía Tây (diagonal).
            Direction.NorthEast,  // Di chuyển lên phía Bắc và sang phía Đông (diagonal).
            Direction.SouthWest,  // Di chuyển xuống phía Nam và sang phía Tây (diagonal).
            Direction.SouthEast   // Di chuyển xuống phía Nam và sang phía Đông (diagonal).
        };

        // Constructor khởi tạo quân Hậu với màu sắc (Player).
        public Queen(Player color)
        {
            Color = color;  // Gán màu của quân cờ cho thuộc tính Color.
        }

        // Phương thức sao chép (copy) quân cờ Hậu, tạo một đối tượng mới sao chép thông tin từ quân cũ.
        public override Piece Copy()
        {
            // Tạo bản sao quân Hậu mới với cùng màu sắc.
            Queen copy = new Queen(Color);
            // Sao chép trạng thái đã di chuyển của quân cờ (HasMoved).
            copy.HasMoved = HasMoved;
            return copy;  // Trả về bản sao.
        }

        // Phương thức trả về các nước đi hợp lệ của quân Hậu từ vị trí "from" trên bàn cờ.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Gọi phương thức MovePositionsInDirs để lấy tất cả các vị trí di chuyển hợp lệ trong các hướng đã định nghĩa.
            // Sau đó, tạo ra các nước đi bình thường (NormalMove) từ vị trí "from" đến các vị trí có thể di chuyển.
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
