namespace ChessLogic
{
    // Lớp Board quản lý trạng thái của bàn cờ, bao gồm các quân cờ và logic liên quan.
    public class Board
    {
        // Mảng 2D lưu trữ quân cờ, mỗi ô có thể chứa một quân cờ hoặc null.
        private readonly Piece[,] pieces = new Piece[8, 8];

        // Mỗi người chơi (Player) sẽ được gán một vị trí cụ thể hoặc null nếu không có nước đi En Passant khả dụng.
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.White, null },  // Người chơi trắng: ban đầu không có vị trí En Passant khả dụng.
            { Player.Black, null }   // Người chơi đrắng: ban đầu không có vị trí En Passant khả dụng.
        };

        // Truy cập quân cờ tại vị trí (row, col)
        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }  // Trả về quân cờ tại vị trí (row, col)
            set { pieces[row, col] = value; } // Cập nhật quân cờ tại vị trí (row, col)
        }

        // Truy cập quân cờ tại vị trí được xác định bởi đối tượng Position
        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }  // Trả về quân cờ tại vị trí pos
            set { this[pos.Row, pos.Column] = value; } // Cập nhật quân cờ tại vị trí pos
        }

        // Phương thức để lấy vị trí mà quân Tốt của một người chơi có thể bỏ qua (En Passant).
        public Position GetPawnSkipPosition(Player player) 
        {
            return pawnSkipPositions[player]; // Trả về vị trí En Passant khả dụng cho người chơi.
        }

        // Phương thức để thiết lập vị trí mà quân Tốt của một người chơi có thể bỏ qua (En Passant).
        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos; // Cập nhật vị trí En Passant cho người chơi.
        }

        // Hàm khởi tạo bàn cờ ban đầu với các quân cờ ở vị trí bắt đầu
        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces(); // Thêm các quân cờ vào bàn cờ
            return board;
        }

        // Thêm các quân cờ vào bàn cờ theo vị trí bắt đầu
        private void AddStartPieces()
        {
            // Thêm các quân cờ đen vào các vị trí đầu tiên
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            // Thêm các quân cờ trắng vào các vị trí cuối cùng
            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            // Thêm quân Tốt đen vào hàng 2 và quân Tốt trắng vào hàng 7
            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.White);
            }
        }

        // Kiểm tra xem vị trí có hợp lệ trên bàn cờ hay không (từ 0 đến 7 cho hàng và cột)
        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        // Kiểm tra xem một vị trí có trống hay không
        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        // Trả về danh sách tất cả các vị trí có quân cờ trên bàn cờ
        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);

                    // Nếu có quân cờ tại vị trí, trả về vị trí đó
                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        // Trả về danh sách các vị trí có quân cờ của người chơi
        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        // Kiểm tra xem quân vua của người chơi có bị chiếu hay không
        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this); // Kiểm tra xem quân cờ có thể bắt vua đối phương không
            });
        }

        // Tạo bản sao của bàn cờ hiện tại
        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy(); // Copy mỗi quân cờ
            }

            return copy;
        }

        // Đếm số lượng các quân cờ theo loại và màu
        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type); // Tăng số lượng quân cờ theo màu và loại
            }

            return counting;
        }

        // Kiểm tra xem có đủ quân cờ để chiến thắng không
        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVKing(counting) || IsKingBishopVKing(counting) ||
                   IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
        }

        // Kiểm tra có đủ quân để chiến thắng theo các tình huống đặc biệt (Vua vs Vua, Vua vs Tượng, etc.)
        private static bool IsKingVKing(Counting counting)
        {
            return counting.TotalCount == 2; // Vua vs Vua
        }

        private static bool IsKingBishopVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1); // Vua vs Vua và Tượng
        }

        private static bool IsKingKnightVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1); // Vua vs Vua và Mã
        }

        private bool IsKingBishopVKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor(); // Kiểm tra nếu Tượng cùng màu
        }

        // Tìm vị trí của quân cờ theo màu và loại
        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        // Kiểm tra nếu quân Vua và Xe chưa di chuyển để có thể thực hiện nhập thành
        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos)) // Nếu vị trí của Vua hoặc Xe đang trống, không thể thực hiện nhập thành.
            {
                return false;
            }
            // Lấy quân cờ tại vị trí của Vua và Xe.
            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook && // Kiểm tra xem quân cờ có phải là Vua và Xe
                   !king.HasMoved && !rook.HasMoved; // Kiểm tra Vua và Xe chưa di chuyển
        }

        // Kiểm tra quyền nhập thành bên cánh vua (Kingside).
        public bool CastleRightKS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)), // Người chơi trắng: kiểm tra Vua ở ô(7, 4) và Xe ở ô (7, 7).
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)), // Người chơi đen: kiểm tra Vua ở ô (0, 4) và Xe ở ô (0, 7).
                _ => false
            };
        }

        // Kiểm tra quyền nhập thành bên cánh hậu (Queenside).
        public bool CastleRightQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)), // Người chơi trắng: kiểm tra Vua ở ô (7, 4) và Xe ở ô (7, 0).
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)), // Người chơi đen: kiểm tra Vua ở ô (0, 4) và Xe ở ô (0, 0).
                _ => false // Trường hợp mặc định: không có quyền nhập thành.
            };
        }

        // Kiểm tra xem quân Tốt có thể thực hiện nước đi En Passant hay không
        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(IsInside)) // Duyệt qua danh sách các vị trí của quân Tốt có thể kiểm tra.
            {
                Piece piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn) // Bỏ qua nếu ô trống, quân cờ không thuộc người chơi hoặc không phải là Tốt.
                {
                    continue;
                }

                EnPassant move = new EnPassant(pos, skipPos); // Tạo nước đi "en passant" giả định và kiểm tra tính hợp lệ của nó.
                if (move.IsLegal(this))
                {
                    return true; // Nếu hợp lệ, trả về true.
                }
            }

            return false; // Không có nước đi hợp lệ.
        }

        // Kiểm tra xem người chơi có thể thực hiện nước đi En Passant hay không
        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());  // Lấy vị trí mà quân Tốt của đối phương có thể bị bắt theo quy tắc "En Passant".

            if (skipPos == null) // Nếu không có vị trí "En Passant" hợp lệ, trả về false.

            {
                return false;
            }

            Position[] pawnPositions = player switch // Xác định các vị trí quân Tốt của người chơi hiện tại có thể kiểm tra.
            {
                // Nếu người chơi là Trắng, kiểm tra hai ô chéo về phía Nam của vị trí "skip".
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                // Nếu người chơi là Đen, kiểm tra hai ô chéo về phía Bắc của vị trí "skip".
                Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
                _ => Array.Empty<Position>() // Trường hợp mặc định: không có vị trí nào.
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}
