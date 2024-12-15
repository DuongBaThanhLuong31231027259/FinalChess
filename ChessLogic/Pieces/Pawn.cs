namespace ChessLogic
{
    // Định nghĩa lớp Pawn kế thừa từ lớp Piece
    public class Pawn : Piece
    {
        // Ghi đè thuộc tính Type từ lớp cha để trả về PieceType.Pawn
        // Xác định rằng đây là quân Tốt (Pawn).
        public override PieceType Type => PieceType.Pawn;

        // Ghi đè thuộc tính Color từ lớp cha
        // Thuộc tính này trả về màu sắc (Player) của quân Tốt, chẳng hạn White hoặc Black.
        public override Player Color { get; }

        // Khai báo một biến readonly tên là forward, dùng để lưu hướng di chuyển của quân Tốt.
        // Hướng này phụ thuộc vào màu sắc của quân cờ (Lên trên với quân Tốt trắng, xuống dưới với quân Tốt đen).
        private readonly Direction forward;

        // Constructor của lớp Pawn, nhận tham số color để xác định màu sắc của quân cờ.
        public Pawn(Player color)
        {
            // Gán giá trị màu sắc cho thuộc tính Color.
            Color = color;

            // Kiểm tra màu sắc của quân Tốt và xác định hướng di chuyển:
            // Nếu quân Tốt màu trắng (White), hướng di chuyển là lên trên (North).
            if (color == Player.White)
            {
                forward = Direction.North;
            }
            // Nếu quân Tốt màu đen (Black), hướng di chuyển là xuống dưới (South).
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }

        // Ghi đè phương thức Copy từ lớp cha để tạo ra một bản sao quân Tốt.
        public override Piece Copy()
        {
            // Tạo một bản sao của quân Tốt, giữ nguyên màu sắc (Color) và trạng thái đã di chuyển (HasMoved).
            Pawn copy = new Pawn(Color);

            // Sao chép trạng thái HasMoved từ quân cờ gốc sang bản sao.
            copy.HasMoved = HasMoved;

            // Trả về bản sao mới.
            return copy;
        }

        // Phương thức kiểm tra một ô có hợp lệ để di chuyển (trống và trong phạm vi bàn cờ).
        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        // Phương thức kiểm tra một ô có thể bị bắt bởi quân Tốt hay không.
        private bool CanCaptureAt(Position pos, Board board)
        {
            // Kiểm tra xem ô có nằm trong phạm vi bàn cờ và không trống.
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }

            // Kiểm tra nếu ô có quân của đối phương (màu sắc khác với quân Tốt).
            return board[pos].Color != Color;
        }

        // Phương thức trả về các nước đi để thăng cấp quân Tốt khi đạt đến hàng cuối.
        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight); // Thăng cấp thành Mã
            yield return new PawnPromotion(from, to, PieceType.Bishop); // Thăng cấp thành Tượng
            yield return new PawnPromotion(from, to, PieceType.Rook);   // Thăng cấp thành Xe
            yield return new PawnPromotion(from, to, PieceType.Queen);  // Thăng cấp thành Hậu
        }

        // Phương thức kiểm tra các nước đi tiến thẳng của quân Tốt (1 hoặc 2 ô).
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            // Kiểm tra nước đi 1 ô về phía trước.
            Position oneMovePos = from + forward;

            if (CanMoveTo(oneMovePos, board)) // Nếu ô này trống và hợp lệ
            {
                // Nếu quân Tốt đi đến hàng cuối (hàng 0 hoặc 7), thực hiện thăng cấp.
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    // Nếu không phải đến hàng cuối, quân Tốt đi bình thường.
                    yield return new NormalMove(from, oneMovePos);
                }

                // Kiểm tra nước đi 2 ô về phía trước (chỉ khi đây là nước đi đầu tiên của quân Tốt).
                Position twoMovesPos = oneMovePos + forward;

                if (!HasMoved && CanMoveTo(twoMovesPos, board)) // Nếu quân Tốt chưa di chuyển
                {
                    yield return new DoublePawn(from, twoMovesPos);
                }
            }
        }

        // Phương thức kiểm tra các nước đi chéo của quân Tốt (để bắt quân đối phương).
        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            // Kiểm tra 2 hướng di chuyển chéo (trái và phải).
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                // Tính vị trí mới sau khi di chuyển chéo.
                Position to = from + forward + dir;

                // Kiểm tra nếu quân đối phương vừa di chuyển để thực hiện nước đi en passant.
                if (to == board.GetPawnSkipPosition(Color.Opponent()))
                {
                    yield return new EnPassant(from, to);
                }
                // Kiểm tra nếu quân Tốt có thể bắt quân đối phương tại ô này.
                else if (CanCaptureAt(to, board))
                {
                    // Nếu quân Tốt đi đến hàng cuối, thực hiện thăng cấp.
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        // Nếu không, chỉ đơn giản là bắt quân đối phương.
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        // Phương thức trả về tất cả các nước đi hợp lệ của quân Tốt, bao gồm cả di chuyển tiến thẳng và chéo.
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        // Kiểm tra xem quân Tốt có thể bắt được vua của đối phương không.
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Kiểm tra các nước đi chéo để xem có quân vua của đối phương không.
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King; // Nếu là quân vua
            });
        }
    }
}
