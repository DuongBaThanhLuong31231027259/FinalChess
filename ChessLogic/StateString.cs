using System.Text;

namespace ChessLogic
{
    // Lớp StateString tạo ra một chuỗi đại diện cho trạng thái của bàn cờ,
    // Bao gồm vị trí của các quân cờ, người chơi hiện tại, quyền nhập thành, và quyền En Passant.
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder(); // Dùng StringBuilder để xây dựng chuỗi trạng thái.

        public StateString(Player currentPlayer, Board board) // Khởi tạo đối tượng StateString với thông tin về người chơi hiện tại và bàn cờ.
        {
            AddPiecePlacement(board); // Thêm vị trí các quân cờ vào chuỗi.
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer); // Thêm thông tin người chơi hiện tại (trắng hoặc đen).
            sb.Append(' ');
            AddCastlingRights(board); // Thêm quyền nhập thành vào chuỗi.
            sb.Append(' ');
            AddEnPassant(board, currentPlayer); // Thêm quyền "en passant" vào chuỗi.
        }

        public override string ToString() // Hàm này trả về chuỗi đại diện cho trạng thái hiện tại của bàn cờ.
        {
            return sb.ToString();
        }

        private static char PieceChar(Piece piece) // Hàm chuyển đổi quân cờ thành ký tự đại diện cho nó trong chuỗi.
        {
            char c = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' ' // Nếu không phải quân cờ hợp lệ, trả về khoảng trắng.
            };

            if (piece.Color == Player.White) // Nếu quân cờ là của người chơi trắng, chuyển ký tự thành chữ hoa.
            {
                return char.ToUpper(c);
            }

            return c; // Nếu là quân đen, giữ nguyên ký tự.
        }

        private void AddRowData(Board board, int row) // Hàm này tạo ra thông tin về các quân cờ trong một hàng của bàn cờ.
        { 
            int empty = 0; // Biến đếm số ô trống trong hàng.

            for (int c = 0; c < 8; c++) // Duyệt qua từng cột trong hàng.
            {
                if (board[row, c] == null)
                {
                    empty++; // Nếu ô trống thì tăng số ô trống.
                    continue;
                }

                if (empty > 0) // Nếu có quân cờ sau khi có ô trống trước đó, thêm số ô trống vào chuỗi.
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PieceChar(board[row, c])); // Thêm ký tự của quân cờ vào chuỗi.
            }

            if (empty > 0) // Nếu còn ô trống sau cùng trong hàng, thêm số ô trống vào chuỗi.
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board) // Thêm thông tin về vị trí của các quân cờ trên bàn cờ vào chuỗi.
        {
            for (int r = 0; r < 8; r++) // Duyệt qua từng hàng trên bàn cờ.
            {
                if (r != 0) // Nếu không phải hàng đầu tiên, thêm dấu phân cách '/'.
                {
                    sb.Append('/');
                }

                AddRowData(board, r); // Thêm thông tin về một hàng của bàn cờ.
            }
        }

        private void AddCurrentPlayer(Player currentPlayer) // Thêm thông tin về người chơi hiện tại (trắng hoặc đen).
        {
            if (currentPlayer == Player.White)
            {
                sb.Append('w'); // 'w' cho người chơi trắng.

            }
            else
            {
                sb.Append('b'); // 'b' cho người chơi đen.
            }
        }

        private void AddCastlingRights(Board board) // Thêm quyền nhập thành vào chuỗi.
        {
            // Kiểm tra quyền nhập thành của người chơi trắng và đen.
            bool castleWKS = board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleRightQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            if (!(castleWKS || castleWQS || castleBKS || castleBQS)) // Nếu không có quyền nhập thành nào, thêm dấu '-' vào chuỗi.
            {
                sb.Append('-');
                return;
            }
            // Nếu có quyền nhập thành, thêm ký tự tương ứng vào chuỗi.
            if (castleWKS)
            {
                sb.Append('K');
            }
            if (castleWQS)
            {
                sb.Append('Q');
            }
            if (castleBKS)
            {
                sb.Append('k');
            }
            if (castleBQS)
            {
                sb.Append('q');
            }
        }

        private void AddEnPassant(Board board, Player currentPlayer) // Thêm thông tin về quyền "En Passant" vào chuỗi.
        {
            if (!board.CanCaptureEnPassant(currentPlayer))  // Kiểm tra xem có thể thực hiện nước đi "En Passant" hay không.
            {
                sb.Append('-'); // Nếu không thể, thêm dấu '-' vào chuỗi.
                return;
            }

            Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent()); // Lấy vị trí của quân Tốt mà có thể bị bắt "En Passant".
            char file = (char)('a' + pos.Column); // Chuyển cột sang ký tự (a-h).
            int rank = 8 - pos.Row; // Chuyển hàng sang số (1-8).
            sb.Append(file); // Thêm ký tự cột vào chuỗi.
            sb.Append(rank); // Thêm số hàng vào chuỗi.
        }
    }
}
