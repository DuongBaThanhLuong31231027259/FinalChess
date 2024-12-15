namespace ChessLogic
{
    public abstract class Piece
    {
        // Định nghĩa một thuộc tính trừu tượng (abstract) có tên là Type
        // Thuộc tính này trả về kiểu quân cờ (PieceType) tương ứng với quân cờ hiện tại.
        public abstract PieceType Type { get; }

        // Định nghĩa một thuộc tính trừu tượng (abstract) có tên là Color
        // Thuộc tính này trả về màu sắc của quân cờ (Player), ví dụ: White hoặc Black.
        public abstract Player Color { get; }

        // Định nghĩa một thuộc tính thông thường (non-abstract) có tên là HasMoved
        // Thuộc tính này kiểm tra hoặc đặt trạng thái quân cờ đã di chuyển hay chưa.
        // Giá trị mặc định của thuộc tính này là false, nghĩa là quân cờ chưa di chuyển.
        public bool HasMoved { get; set; } = false;

        // Định nghĩa một phương thức trừu tượng (abstract) có tên là Copy
        // Phương thức này sẽ trả về một bản sao của quân cờ hiện tại.
        // Cụ thể, mỗi lớp con kế thừa lớp này sẽ triển khai logic tạo bản sao phù hợp.
        public abstract Piece Copy();


        // Phương thức trừu tượng để lấy các nước đi hợp lệ từ vị trí "from" trên bàn cờ.
        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        // Phương thức bảo vệ để lấy các vị trí mà quân cờ có thể di chuyển trong một hướng cụ thể.
        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            // Bắt đầu từ vị trí "from", và duyệt qua các ô trong hướng "dir" miễn là ô đó nằm trong bàn cờ.
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                // Nếu ô tiếp theo trống, quân cờ có thể di chuyển tới đó, trả về vị trí.
                if (board.IsEmpty(pos))
                {
                    yield return pos;  // Trả về vị trí này và tiếp tục tìm vị trí tiếp theo.
                    continue;
                }

                // Nếu ô chứa quân cờ, kiểm tra xem quân đó có phải là quân đối phương hay không.
                Piece piece = board[pos];

                // Nếu quân đối phương đang chiếm ô này, quân cờ có thể chiếm ô đó (di chuyển hoặc ăn quân đối phương).
                if (piece.Color != Color)
                {
                    yield return pos;  // Trả về vị trí để chiếm quân đối phương.
                }

                // Nếu quân cờ không thể di chuyển hay ăn quân đối phương, dừng vòng lặp.
                yield break;
            }
        }

        // Phương thức bảo vệ để lấy các vị trí mà quân cờ có thể di chuyển trong nhiều hướng (từ các hướng đã cho).
        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            // Duyệt qua các hướng và gọi "MovePositionsInDir" để lấy các vị trí di chuyển hợp lệ từ các hướng khác nhau.
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }

        // Phương thức để kiểm tra nếu quân cờ có thể ăn được quân vua của đối phương từ vị trí "from".
        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Lấy tất cả các nước đi hợp lệ từ "from" và kiểm tra xem có nước đi nào có thể ăn được quân vua không.
            return GetMoves(from, board).Any(move =>
            {
                // Lấy quân cờ tại vị trí đích của nước đi.
                Piece piece = board[move.ToPos];

                // Nếu quân cờ là vua đối phương, trả về true, nghĩa là có thể ăn quân vua.
                return piece != null && piece.Type == PieceType.King;
            });
        }

    }
}
