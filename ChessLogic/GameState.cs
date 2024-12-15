namespace ChessLogic
{
    public class GameState
    {
        // Thuộc tính chỉ đọc công khai (public readonly property) có tên là Board.
        // Thuộc tính này chứa thông tin về bàn cờ (Board), có thể là trạng thái hiện tại của bàn cờ.
        public Board Board { get; }

        // Thuộc tính công khai có tên là CurrentPlayer, chỉ có thể được thiết lập (set) 
        // trong phạm vi của lớp này. Thuộc tính này đại diện cho người chơi hiện tại.
        public Player CurrentPlayer { get; private set; }

        // Thuộc tính kết quả của ván cờ (Result), có thể là Win hoặc Draw, mặc định là null (chưa có kết quả).
        public Result Result { get; private set; } = null;

        // Biến đếm số nước đi mà không có quân cờ bị bắt hay quân Tốt di chuyển (dùng cho quy tắc Fifty-Move).
        private int noCaptureOrPawnMoves = 0;

        // Biến lưu trữ chuỗi trạng thái của trò chơi (dùng để theo dõi trạng thái cờ vua).
        private string stateString;

        // Lịch sử các trạng thái của trò chơi, lưu trữ số lần một trạng thái đã xảy ra.
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        // Phương thức khởi tạo (constructor) của lớp GameState.
        // Phương thức này được gọi khi một đối tượng GameState mới được tạo.
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;   // Xác định người chơi hiện tại.
            Board = board;            // Gán bàn cờ hiện tại.

            // Khởi tạo chuỗi trạng thái cờ vua hiện tại (dùng để kiểm tra ba lần lặp lại trạng thái).
            stateString = new StateString(CurrentPlayer, board).ToString();

            // Lưu trạng thái cờ vua vào lịch sử với số lần xuất hiện là 1.
            stateHistory[stateString] = 1;
        }

        // Phương thức này trả về tất cả các nước đi hợp pháp của một quân cờ tại vị trí pos.
        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            // Nếu vị trí không có quân cờ hoặc quân cờ không phải của người chơi hiện tại, trả về danh sách rỗng.
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            // Lấy quân cờ tại vị trí pos và lấy tất cả các nước đi của quân cờ đó.
            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);

            // Trả về các nước đi hợp pháp.
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // Phương thức này thực hiện một nước đi và cập nhật trạng thái của trò chơi.
        public void MakeMove(Move move)
        {
            // Xóa thông tin về vị trí quân Tốt bị bỏ qua (nếu có).
            Board.SetPawnSkipPosition(CurrentPlayer, null);

            // Thực hiện nước đi và kiểm tra nếu có quân bị bắt hoặc quân Tốt di chuyển.
            bool captureOrPawn = move.Execute(Board);

            // Nếu có quân bị bắt hoặc quân Tốt di chuyển, đặt lại số nước đi không bắt quân hay di chuyển Tốt.
            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
                stateHistory.Clear();  // Xóa lịch sử trạng thái.
            }
            else
            {
                noCaptureOrPawnMoves++;  // Tăng số nước đi không bắt quân hoặc không di chuyển Tốt.
            }

            // Thay đổi lượt chơi cho người chơi đối thủ.
            CurrentPlayer = CurrentPlayer.Opponent();

            // Cập nhật chuỗi trạng thái sau mỗi nước đi.
            UpdateStateString();

            // Kiểm tra nếu ván cờ đã kết thúc hay chưa.
            CheckForGameOver();
        }

        // Phương thức này trả về tất cả các nước đi hợp pháp của một người chơi.
        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            // Lấy tất cả các nước đi hợp pháp của các quân cờ trên bàn cờ của người chơi.
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            // Trả về các nước đi hợp pháp.
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // Kiểm tra nếu ván cờ đã kết thúc và cập nhật kết quả.
        private void CheckForGameOver()
        {
            // Nếu người chơi hiện tại không còn nước đi hợp pháp nào.
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                // Nếu người chơi hiện tại bị chiếu, đối thủ thắng.
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    // Nếu không có nước đi hợp pháp nhưng không bị chiếu, hòa do bế tắc (Stalemate).
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            // Kiểm tra nếu không đủ quân để thực hiện chiếu Vua (Insufficient Material).
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            // Kiểm tra nếu đã đạt đủ 50 nước đi không bắt quân hoặc không di chuyển Tốt (Fifty-Move Rule).
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            // Kiểm tra nếu ba lần lặp lại trạng thái (Threefold Repetition).
            else if (ThreefoldRepetition())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }
        }

        // Phương thức trả về true nếu ván cờ đã kết thúc (có kết quả).
        public bool IsGameOver()
        {
            return Result != null;
        }

        // Kiểm tra quy tắc 50 nước đi không bắt quân hoặc không di chuyển Tốt.
        private bool FiftyMoveRule()
        {
            int fullMoves = (int)Math.Floor((double)noCaptureOrPawnMoves / 2);
            return fullMoves == 50;
        }

        // Cập nhật chuỗi trạng thái của ván cờ sau mỗi nước đi.
        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();

            // Nếu trạng thái chưa tồn tại trong lịch sử, thêm mới vào lịch sử với số lần là 1.
            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                // Nếu trạng thái đã có, tăng số lần xuất hiện.
                stateHistory[stateString]++;
            }
        }

        // Kiểm tra quy tắc ba lần lặp lại trạng thái.
        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }
    }
}
