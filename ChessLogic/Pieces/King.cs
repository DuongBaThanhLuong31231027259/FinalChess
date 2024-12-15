namespace ChessLogic
{
    // Lớp King kế thừa từ lớp Piece, đại diện cho quân "King" (Vua) trong cờ vua.
    public class King : Piece
    {
        // Thuộc tính Type trả về loại quân cờ (King).
        public override PieceType Type => PieceType.King;

        // Thuộc tính Color trả về màu của quân cờ (do lớp "Piece" kế thừa từ "Player").
        public override Player Color { get; }

        // Mảng các hướng di chuyển của quân Vua. Vua có thể di chuyển 1 ô theo tất cả các hướng (dọc, ngang, chéo).
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,         // Bắc
            Direction.South,         // Nam
            Direction.East,          // Đông
            Direction.West,          // Tây
            Direction.NorthWest,     // Tây Bắc
            Direction.NorthEast,     // Đông Bắc
            Direction.SouthWest,     // Tây Nam
            Direction.SouthEast      // Đông Nam
        };

        // Constructor khởi tạo quân Vua với màu sắc (Player).
        public King(Player color)
        {
            Color = color;  // Gán màu của quân cờ cho thuộc tính Color.
        }

        // Phương thức kiểm tra xem một quân xe ở vị trí "pos" có thể tham gia nhập thành hay không (chưa di chuyển).
        private static bool IsUnmovedRook(Position pos, Board board)
        {
            if (board.IsEmpty(pos))
            {
                return false;  // Nếu vị trí trống, trả về false.
            }

            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;  // Kiểm tra nếu là quân xe và chưa di chuyển.
        }

        // Phương thức kiểm tra xem tất cả các vị trí trong danh sách "positions" đều trống.
        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));  // Kiểm tra tất cả các vị trí có phải đều trống hay không.
        }

        // Phương thức kiểm tra xem quân Vua có thể nhập thành bên phải (King's Side) hay không.
        private bool CanCastleKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;  // Nếu quân Vua đã di chuyển, không thể nhập thành.
            }

            // Vị trí của quân xe bên phải quân Vua (King's Side).
            Position rookPos = new Position(from.Row, 7);
            // Các vị trí giữa quân Vua và quân xe (các ô 5 và 6 trên cùng hàng).
            Position[] betweenPositions = new Position[] { new(from.Row, 5), new(from.Row, 6) };

            // Kiểm tra nếu quân xe chưa di chuyển và các ô giữa trống.
            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        // Phương thức kiểm tra xem quân Vua có thể nhập thành bên trái (Queen's Side) hay không.
        private bool CanCastleQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;  // Nếu quân Vua đã di chuyển, không thể nhập thành.
            }

            // Vị trí của quân xe bên trái quân Vua (Queen's Side).
            Position rookPos = new Position(from.Row, 0);
            // Các vị trí giữa quân Vua và quân xe (các ô 1, 2 và 3 trên cùng hàng).
            Position[] betweenPositions = new Position[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };

            // Kiểm tra nếu quân xe chưa di chuyển và các ô giữa trống.
            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        // Phương thức sao chép quân Vua, tạo một quân Vua mới với cùng màu sắc và trạng thái di chuyển.
        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;  // Sao chép trạng thái di chuyển.
            return copy;  // Trả về bản sao.
        }

        // Phương thức tính các vị trí hợp lệ quân Vua có thể di chuyển đến từ một vị trí "from".
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            // Lặp qua tất cả các hướng di chuyển của quân Vua.
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;

                // Nếu vị trí "to" ngoài phạm vi bàn cờ, tiếp tục kiểm tra hướng khác.
                if (!Board.IsInside(to))
                {
                    continue;
                }

                // Nếu ô "to" trống hoặc có quân đối phương, quân Vua có thể di chuyển tới đó.
                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;  // Trả về vị trí hợp lệ.
                }
            }
        }

        // Phương thức trả về các nước đi hợp lệ của quân Vua từ một vị trí trên bàn cờ.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Lặp qua tất cả các vị trí hợp lệ có thể di chuyển.
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);  // Tạo nước đi bình thường từ vị trí "from" đến "to".
            }

            // Kiểm tra nếu quân Vua có thể nhập thành bên phải (King's Side), nếu có, thêm nước đi nhập thành vào danh sách.
            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKS, from);  // Thêm nước đi nhập thành King's Side.
            }

            // Kiểm tra nếu quân Vua có thể nhập thành bên trái (Queen's Side), nếu có, thêm nước đi nhập thành vào danh sách.
            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQS, from);  // Thêm nước đi nhập thành Queen's Side.
            }
        }

        // Phương thức kiểm tra xem quân Vua có thể bắt quân Vua đối phương hay không.
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Kiểm tra tất cả các vị trí quân Vua có thể di chuyển đến và kiểm tra nếu vị trí đó có quân Vua đối phương.
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;  // Nếu có quân Vua đối phương tại vị trí "to", trả về true.
            });
        }
    }
}
