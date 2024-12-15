using ChessLogic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// Màn hình hiển thị kết quả sau khi ván cờ kết thúc
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        // Định nghĩa sự kiện khi người chơi chọn một tùy chọn (Restart hoặc Exit)
        public event Action<Option> OptionSelected;

        // Constructor khởi tạo màn hình GameOver với trạng thái trò chơi hiện tại
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            // Lấy kết quả trò chơi từ gameState và hiển thị người thắng và lý do kết thúc
            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);  // Hiển thị người thắng cuộc
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);  // Hiển thị lý do kết thúc trò chơi

            // Lưu kết quả trò chơi vào file đã tạo
            SaveGameResult(gameState);
        }

        // Phương thức để lấy dòng thông báo về người thắng cuộc
        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "WHITE WINS!",  // Nếu người thắng là trắng
                Player.Black => "BLACK WINS!",  // Nếu người thắng là đen
                _ => "IT'S A DRAW"  // Nếu hòa
            };
        }

        // Phương thức để chuyển đổi người chơi thành chuỗi văn bản ("WHITE" hoặc "BLACK")
        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "WHITE",  // Trả về "WHITE" nếu người chơi là trắng
                Player.Black => "BLACK",  // Trả về "BLACK" nếu người chơi là đen
                _ => ""  // Trả về chuỗi rỗng trong trường hợp khác
            };
        }

        // Phương thức để lấy lý do kết thúc trò chơi (Stalemate, Checkmate, v.v.)
        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE",  // Nếu là thế cờ hòa
                EndReason.Checkmate => $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE",  // Nếu là chiếu hết
                EndReason.FiftyMoveRule => "FIFTY-MOVE RULE",  // Nếu là do quy tắc 50 nước
                EndReason.InsufficientMaterial => "INSUFFICIENT MATERIAL",  // Nếu là do thiếu quân để tiếp tục chơi
                EndReason.ThreefoldRepetition => "THREEFOLD REPETITION",  // Nếu là do tái lập thế cờ 3 lần
                _ => ""  // Trả về chuỗi rỗng nếu không có lý do cụ thể
            };
        }

        // Phương thức xử lý khi người chơi chọn "Restart"
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            // Gửi sự kiện lựa chọn Restart
            OptionSelected?.Invoke(Option.Restart);
        }

        // Phương thức xử lý khi người chơi chọn "Exit"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Gửi sự kiện lựa chọn Exit
            OptionSelected?.Invoke(Option.Exit);
        }

        private void SaveGameResult(GameState gameState)
        {
            string directoryPath = @"C:\Chess Game Result"; // Đường dẫn thư mục lưu kết quả
            string filePath = Path.Combine(directoryPath, "GameResults.txt"); // Ghép đường dẫn thư mục và tên file
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Result result = gameState.Result;

            // Kiểm tra thư mục có tồn tại hay không, nếu không thì tạo mới
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Tạo thư mục nếu chưa tồn tại
            }

            // Kiểm tra file có tồn tại hay không, nếu không thì tạo file mới
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close(); // Tạo file và đóng file handle ngay lập tức
            }

            string resultMessage = result.Winner switch
            {
                Player.White => $"Match {GetMatchNumber(filePath)}: Player White won, Finished at: {timestamp}.",
                Player.Black => $"Match {GetMatchNumber(filePath)}: Player Black won, Finished at: {timestamp}.",
                _ => $"Match {GetMatchNumber(filePath)}: draw, Finished at: {timestamp}."
            };

            // Ghi nội dung kết quả vào file, mỗi kết quả trên một dòng
            File.AppendAllText(filePath, resultMessage + Environment.NewLine);
        }

        // Phương thức hỗ trợ: Đếm số trận đấu để đánh số thứ tự
        private int GetMatchNumber(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Nếu file không tồn tại, trả về số 1 (trận đầu tiên)
                return 1;
            }
            // Đếm số dòng trong file để xác định số thứ tự trận tiếp theo
            return File.ReadAllLines(filePath).Length + 1;
        }
    }
}
