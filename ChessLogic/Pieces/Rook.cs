namespace ChessLogic
{
    // Lớp Rook kế thừa từ lớp Piece, đại diện cho quân "Rook" (Xe) trong cờ vua.
    public class Rook : Piece
    {
        // Thuộc tính Type trả về loại quân cờ (Rook).
        public override PieceType Type => PieceType.Rook;

        // Thuộc tính Color trả về màu của quân cờ (do lớp "Piece" kế thừa từ "Player").
        public override Player Color { get; }

        // Mảng các hướng di chuyển của quân Xe (hướng thẳng theo các trục).
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,  // Di chuyển lên phía Bắc (tăng theo trục y).
            Direction.South,  // Di chuyển xuống phía Nam (giảm theo trục y).
            Direction.East,   // Di chuyển sang phía Đông (tăng theo trục x).
            Direction.West    // Di chuyển sang phía Tây (giảm theo trục x).
        };

        // Constructor khởi tạo quân Xe với màu sắc (Player).
        public Rook(Player color)
        {
            Color = color;  // Gán màu của quân cờ cho thuộc tính Color.
        }

        // Phương thức sao chép (copy) quân cờ Xe, tạo một đối tượng mới sao chép thông tin từ quân cũ.
        public override Piece Copy()
        {
            // Tạo bản sao quân Xe mới với cùng màu sắc.
            Rook copy = new Rook(Color);
            // Sao chép trạng thái đã di chuyển của quân cờ (HasMoved).
            copy.HasMoved = HasMoved;
            return copy;  // Trả về bản sao.
        }

        // Phương thức trả về các nước đi hợp lệ của quân Xe từ vị trí "from" trên bàn cờ.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Gọi phương thức MovePositionsInDirs để lấy tất cả các vị trí di chuyển hợp lệ trong các hướng đã định nghĩa.
            // Sau đó, tạo ra các nước đi bình thường (NormalMove) từ vị trí "from" đến các vị trí có thể di chuyển.
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
