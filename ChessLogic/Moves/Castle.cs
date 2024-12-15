namespace ChessLogic
{
    // Class Castle đại diện cho nước đi nhập thành (Castle) trong cờ vua.
    public class Castle : Move
    {
        public override MoveType Type { get; } // Thuộc tính ghi đè để xác định loại nước đi (CastleKS hoặc CastleQS).
        public override Position FromPos { get; } // Vị trí ban đầu của quân vua.
        public override Position ToPos { get; } // Vị trí đích của quân vua sau khi nhập thành.

        private readonly Direction kingMoveDir; // Hướng di chuyển của vua (Direction.East hoặc Direction.West).
        private readonly Position rookFromPos; // Vị trí ban đầu của quân xe tham gia nhập thành.
        private readonly Position rookToPos; // Vị trí mới của quân xe sau khi nhập thành.

        // Constructor nhận vào loại nhập thành (CastleKS hoặc CastleQS) và vị trí của vua.
        public Castle(MoveType type, Position kingPos)
        {
            Type = type;
            FromPos = kingPos;

            if (type == MoveType.CastleKS) // Nếu loại nhập thành là CastleKS (nhập thành gần - King Side).
            {
                kingMoveDir = Direction.East; // Hướng di chuyển của vua là về phía Đông.
                ToPos = new Position(kingPos.Row, 6); // Vị trí mới của vua (cột 6).
                rookFromPos = new Position(kingPos.Row, 7); // Vị trí ban đầu của xe (cột 7).
                rookToPos = new Position(kingPos.Row, 5); // Vị trí mới của xe (cột 5).
            }
            else if (type == MoveType.CastleQS) // Nếu loại nhập thành là CastleQS (nhập thành xa - Queen Side).
            {
                kingMoveDir = Direction.West; // Hướng di chuyển của vua là về phía Tây.
                ToPos = new Position(kingPos.Row, 2); // Vị trí mới của vua (cột 2).
                rookFromPos = new Position(kingPos.Row, 0); // Vị trí ban đầu của xe (cột 0).
                rookToPos = new Position(kingPos.Row, 3); // Vị trí mới của xe (cột 3).
            }
        }

        public override bool Execute(Board board) // Thực hiện nước đi nhập thành trên bàn cờ.
        { 
            new NormalMove(FromPos, ToPos).Execute(board); // Di chuyển vua từ vị trí ban đầu tới vị trí đích.
            new NormalMove(rookFromPos, rookToPos).Execute(board); // Di chuyển xe từ vị trí ban đầu tới vị trí mới.

            return false; // Trả về false để chỉ ra đây không phải một nước đi thông thường (không tính điểm).
        }

        public override bool IsLegal(Board board) // Kiểm tra xem nước đi nhập thành có hợp lệ không.
        { 
            Player player = board[FromPos].Color; // Lấy người chơi hiện tại từ màu của quân vua.

            if (board.IsInCheck(player)) // Nếu vua đang bị chiếu, không thể nhập thành.
            {
                return false;
            }

            Board copy = board.Copy(); // Tạo một bản sao của bàn cờ để kiểm tra giả lập nước đi nhập thành.
            Position kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++) // Kiểm tra xem vua có đi qua hoặc dừng lại tại một ô đang bị chiếu không.
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(copy); // Di chuyển vua một ô theo hướng nhập thành.
                kingPosInCopy += kingMoveDir;

                if (copy.IsInCheck(player))  // Nếu vua bị chiếu tại bất kỳ ô nào, nước đi không hợp lệ.
                {
                    return false;
                }
            }

            return true; // Nếu tất cả các điều kiện đều hợp lệ, trả về true.
        }
    }
}
