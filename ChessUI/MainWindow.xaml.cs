using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using ChessLogic;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    // Dùng từ khóa "partial" để chỉ ra rằng lớp MainWindow này có thể được định nghĩa ở nhiều tệp khác nhau.
    public partial class MainWindow : Window
    {
        // Khai báo mảng hai chiều chứa hình ảnh của các quân cờ. Dùng "readonly" để đảm bảo mảng này không thể bị gán lại.
        private readonly Image[,] pieceImages = new Image[8, 8];

        // Khai báo mảng hai chiều cho các ô đánh dấu (highlight) khi chọn các nước đi hợp lệ.
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];

        // Dùng Dictionary để lưu các nước đi hợp lệ từ mỗi ô, giúp nhanh chóng truy xuất khi chọn ô.
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        // Trạng thái của ván cờ và vị trí được chọn hiện tại.
        private GameState gameState;
        private Position selectedPos = null;

        // Phương thức Import từ thư viện Windows API để cấp phát Console (dùng cho Debugging).
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        // Khai báo Timer để theo dõi thời gian ván cờ.
        private DispatcherTimer _timer;
        private DateTime _startTime;

        // Constructor (phương thức khởi tạo) của lớp MainWindow.
        public MainWindow()
        {
            // Khởi tạo giao diện người dùng (gọi InitializeComponent() để thiết lập giao diện trong file XAML).
            InitializeComponent();

            // Cập nhật trạng thái của trò chơi và in ra Console.
            UpdateGameStatus("THE GAME IS RUNNING");

            // Mở cửa sổ yêu cầu người chơi nhập tên.
            NameEntryWindow nameEntryWindow = new NameEntryWindow();
            if (nameEntryWindow.ShowDialog() == true)
            {
                // Cập nhật tên người chơi vào TextBlock.
                PlayerWhiteSide.Text = "Player 1: " + nameEntryWindow.Player1Name;
                PlayerBlackSide.Text = "Player 2: " + nameEntryWindow.Player2Name;
            }

            // Thiết lập timer, mỗi giây sẽ cập nhật đồng hồ.
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            // Bắt đầu đồng hồ khi bắt đầu ván cờ.
            _startTime = DateTime.Now;
            _timer.Start();

            // Khởi tạo bàn cờ và các ô chứa hình ảnh quân cờ.
            InitializeBoard();

            // Khởi tạo trạng thái trò chơi (tạo bảng cờ mới và bắt đầu với người chơi trắng).
            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);

            // Đặt con trỏ tùy thuộc vào lượt chơi của người chơi (trắng hay đen).
            SetCursor(gameState.CurrentPlayer);
        }

        // Cập nhật trạng thái của trò chơi, hiển thị trên giao diện và in ra Console.
        private void UpdateGameStatus(string message)
        {
            State.Text = message;
            Console.WriteLine(message);
        }

        // Phương thức xử lý sự kiện Timer_Tick, được gọi mỗi giây để cập nhật đồng hồ.
        private void Timer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _startTime;
            Clock.Text = elapsed.ToString(@"mm\:ss");  // Hiển thị thời gian đã trôi qua theo định dạng phút:giây.
        }

        // Phương thức khởi tạo bàn cờ, tạo ra các ô (squares) và gán hình ảnh cho các quân cờ.
        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // Tạo đối tượng Image cho mỗi ô cờ.
                    Image image = new Image();
                    pieceImages[r, c] = image;  // Lưu vào mảng để truy xuất sau.
                    PieceGrid.Children.Add(image);  // Thêm vào giao diện để hiển thị.

                    // Tạo và lưu đối tượng Rectangle dùng để tô màu ô khi chọn nước đi hợp lệ.
                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }

        // Phương thức vẽ lại bàn cờ với các quân cờ hiện tại.
        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);  // Lấy hình ảnh quân cờ và gán vào mỗi ô.
                }
            }
        }

        // Xử lý sự kiện khi người dùng nhấn chuột xuống trên bảng cờ.
        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                return;
            }

            // Lấy vị trí nhấn chuột và chuyển đổi thành vị trí ô cờ (Position).
            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            // Kiểm tra xem người chơi đang chọn ô nào, nếu chưa chọn ô thì chọn ô xuất phát, nếu đã chọn ô thì chọn ô đích.
            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        // Chuyển đổi tọa độ của chuột thành vị trí ô trên bàn cờ.
        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);
        }

        // Xử lý sự kiện khi người chơi chọn ô xuất phát.
        private void OnFromPositionSelected(Position pos)
        {
            // Lấy danh sách các nước đi hợp lệ từ vị trí đã chọn.
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            // Nếu có nước đi hợp lệ, lưu vị trí đã chọn và hiển thị các ô hợp lệ.
            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);  // Lưu các nước đi hợp lệ vào bộ nhớ cache.
                ShowHighlights();  // Hiển thị các ô hợp lệ.
            }
        }

        // Xử lý sự kiện khi người chơi chọn ô đích để thực hiện nước đi.
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();  // Ẩn các ô hợp lệ khi đã chọn xong.

            // Kiểm tra nếu có nước đi từ ô xuất phát sang ô đích.
            if (moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    // Nếu là nước đi thăng chức quân tốt, xử lý thăng chức.
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                {
                    // Thực hiện di chuyển bình thường.
                    HandleMove(move);
                }
            }
        }

        // Xử lý thăng chức quân tốt.
        private void HandlePromotion(Position from, Position to)
        {
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            // Mở menu để người chơi chọn quân thay thế khi thăng chức.
            PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
            MenuContainer.Content = promMenu;

            promMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);  // Thực hiện nước đi thăng chức.
            };
        }

        // Xử lý một nước đi (di chuyển quân cờ).
        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);  // Cập nhật trạng thái trò chơi với nước đi mới.
            DrawBoard(gameState.Board);  // Vẽ lại bàn cờ sau khi di chuyển.
            SetCursor(gameState.CurrentPlayer);  // Cập nhật con trỏ tùy theo lượt chơi của người chơi.

            // Kiểm tra nếu ván cờ kết thúc.
            if (gameState.IsGameOver())
            {
                ShowGameOver();  // Hiển thị menu kết thúc trò chơi.
            }
        }

        // Lưu các nước đi hợp lệ vào bộ nhớ cache để sử dụng sau này.
        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();  // Xóa dữ liệu cũ trong cache.

            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;  // Lưu nước đi vào cache.
            }
        }

        // Hiển thị các ô hợp lệ mà người chơi có thể di chuyển đến.
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);  // Đổi màu các ô hợp lệ.
            }
        }

        // Ẩn các ô hợp lệ đã hiển thị.
        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;  // Đặt màu trong suốt cho các ô.
            }
        }

        // Cập nhật con trỏ chuột tùy thuộc vào người chơi hiện tại (trắng hoặc đen).
        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        // Kiểm tra xem menu có đang hiển thị trên màn hình hay không.
        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        // Hiển thị menu kết thúc trò chơi khi ván cờ kết thúc.
        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState); // Tạo một menu kết thúc trò chơi với trạng thái trò chơi hiện tại.
            MenuContainer.Content = gameOverMenu; // Hiển thị menu kết thúc trò chơi này lên màn hình người chơi.

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart) // Gắn một sự kiện (event handler) để xử lý khi người dùng chọn một tùy chọn từ menu.
                {
                    MenuContainer.Content = null; // Xóa nội dung menu hiện tại để quay lại trạng thái trò chơi ban đầu.
                    RestartGame();  // Khởi động lại trò chơi nếu người chơi chọn chơi lại.
                }
                else
                {
                    Application.Current.Shutdown();  // Đóng ứng dụng nếu người chơi chọn thoát.
                }
            };
        }

        // Phương thức khởi động lại trò chơi, thiết lập lại tất cả các biến và vẽ lại bàn cờ.
        private void RestartGame()
        {
            selectedPos = null;
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Player.White, Board.Initial());     // Đặt lại trạng thái trò chơi về trạng thái ban đầu:
                                                                          // Người chơi đầu tiên là Trắng (Player.White).
                                                                          // Bàn cờ được khởi tạo theo vị trí mặc định (Board.Initial()).
            DrawBoard(gameState.Board); // Vẽ lại giao diện bàn cờ dựa trên trạng thái mới của nó.
            SetCursor(gameState.CurrentPlayer); // Đặt con trỏ (cursor) cho người chơi hiện tại (ở đây là Trắng).
        }

        // Xử lý sự kiện khi người dùng nhấn phím Escape để mở menu tạm dừng trò chơi.
        private void Window_KeyDown(object sender, KeyEventArgs e) 
        {
            if (!IsMenuOnScreen() && e.Key == Key.Escape)  // Kiểm tra xem menu tạm dừng có đang hiển thị không và phím nhấn có phải là Escape không.
            {
                ShowPauseMenu(); // Hiện Menu dừng trò chơi nếu người chơi bấm nút Escape
            }
        }

        // Hiển thị menu tạm dừng trò chơi.
        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            MenuContainer.Content = pauseMenu; // Đặt nội dung của `MenuContainer` (vùng hiển thị menu) thành menu tạm dừng.

            pauseMenu.OptionSelected += option => // Người dùng chọn một tùy chọn từ menu.
            {
                MenuContainer.Content = null; // Loại bỏ menu khỏi màn hình sau khi người chơi chọn một tùy chọn.

                if (option == Option.Restart) // Kiểm tra nếu tùy chọn được chọn là "Khởi động lại" (Restart).
                {
                    RestartGame(); // Gọi phương thức để khởi động lại trò chơi.
                }
            };
        }
    }

}
