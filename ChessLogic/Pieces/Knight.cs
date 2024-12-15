namespace ChessLogic
{
    // Lớp Knight kế thừa từ lớp Piece, đại diện cho quân "Knight" (Mã) trong cờ vua.
    public class Knight : Piece
    {
        // Thuộc tính Type trả về loại quân cờ (Knight).
        public override PieceType Type => PieceType.Knight;

        // Thuộc tính Color trả về màu của quân cờ (do lớp "Piece" kế thừa từ "Player").
        public override Player Color { get; }

        // Constructor khởi tạo quân Mã với màu sắc (Player).
        public Knight(Player color)
        {
            Color = color;  // Gán màu của quân cờ cho thuộc tính Color.
        }

        // Phương thức sao chép (copy) quân cờ Mã, tạo một đối tượng mới sao chép thông tin từ quân cũ.
        public override Piece Copy()
        {
            // Tạo bản sao quân Mã mới với cùng màu sắc.
            Knight copy = new Knight(Color);
            // Sao chép trạng thái đã di chuyển của quân cờ (HasMoved).
            copy.HasMoved = HasMoved;
            return copy;  // Trả về bản sao.
        }

        // Phương thức tính các vị trí có thể di chuyển của quân Mã từ vị trí "from".
        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            // Lặp qua các hướng di chuyển theo chiều dọc (North và South).
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                // Lặp qua các hướng di chuyển theo chiều ngang (West và East).
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    // Tính toán và trả về các vị trí có thể di chuyển theo công thức "2 bước theo chiều dọc + 1 bước theo chiều ngang" và ngược lại.
                    yield return from + 2 * vDir + hDir;  // Di chuyển 2 bước dọc và 1 bước ngang.
                    yield return from + 2 * hDir + vDir;  // Di chuyển 2 bước ngang và 1 bước dọc.
                }
            }
        }

        // Phương thức tính các vị trí hợp lệ có thể di chuyển của quân Mã.
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            // Gọi phương thức PotentialToPositions để lấy tất cả các vị trí tiềm năng mà quân Mã có thể di chuyển đến.
            // Lọc các vị trí hợp lệ (trong phạm vi bàn cờ và không bị quân đồng minh chiếm).
            return PotentialToPositions(from).Where(pos => Board.IsInside(pos)
                && (board.IsEmpty(pos) || board[pos].Color != Color));  // Kiểm tra nếu vị trí có quân đồng minh hoặc là trống.
        }

        // Phương thức trả về các nước đi hợp lệ của quân Mã từ vị trí "from" trên bàn cờ.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Gọi phương thức MovePositions để lấy các vị trí di chuyển hợp lệ.
            // Tạo ra các nước đi bình thường (NormalMove) từ vị trí "from" đến các vị trí hợp lệ.
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
