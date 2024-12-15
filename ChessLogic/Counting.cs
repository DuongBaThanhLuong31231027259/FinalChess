namespace ChessLogic
{
    public class Counting     // Lớp Counting dùng để đếm số lượng các quân cờ của mỗi loại (PieceType)
                              // Trên bàn cờ cho cả hai người chơi (White và Black), hỗ trợ kiểm tra điều kiện "Insufficient Material".
    {
        private readonly Dictionary<PieceType, int> whiteCount = new(); // Từ điển lưu số lượng từng loại quân cờ của người chơi Trắng.
        private readonly Dictionary<PieceType, int> blackCount = new(); // Từ điển lưu số lượng từng loại quân cờ của người chơi Đen.
         
        public int TotalCount { get; private set; }  // Tổng số quân cờ còn lại trên bàn cờ.

        public Counting() // Constructor khởi tạo lớp Counting.
        { 
            foreach (PieceType type in Enum.GetValues(typeof(PieceType))) // Lặp qua tất cả các loại quân cờ (PieceType) để khởi tạo giá trị ban đầu là 0.
            {
                whiteCount[type] = 0; // Đặt số lượng từng loại quân cờ của trắng ban đầu là 0.
                blackCount[type] = 0; // Đặt số lượng từng loại quân cờ của đen ban đầu là 0.
            }
        }

        public void Increment(Player color, PieceType type) // Hàm tăng số lượng của một quân cờ (PieceType) dựa trên màu sắc của người chơi (Player).
        { 
            if (color == Player.White) // Nếu quân cờ thuộc người chơi trắng.
            {
                whiteCount[type]++; // Tăng số lượng quân cờ tương ứng của trắng.
            }
            else if (color == Player.Black) // Nếu quân cờ thuộc người chơi đen.
            {
                blackCount[type]++; // Tăng số lượng quân cờ tương ứng của đrắng.
            }

            TotalCount++;  // Tăng tổng số quân cờ còn lại trên bàn cờ.
        }

        public int White(PieceType type) // Hàm trả về số lượng quân cờ của loại (PieceType) được chỉ định của người chơi Trắng.
        {
            return whiteCount[type];
        }

        public int Black(PieceType type) // Hàm trả về số lượng quân cờ của loại (PieceType) được chỉ định của người chơi Đen.
        {
            return blackCount[type];
        }
    }
}
