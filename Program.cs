/****************/
/* 202418836    */
/* Nguyễn Vũ Quang Anh */
/****************/

using System;
using System.Text;

namespace DeviceManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==================================================================");
            Console.WriteLine("   KIỂM THỬ HỆ THỐNG CẤP PHÁT THIẾT BỊ DẠY HỌC (ĐA HÌNH & GENERIC)");
            Console.WriteLine("==================================================================\n");

            // Khởi tạo 3 kho lưu trữ từ cùng một lớp tổng quát InMemoryRepository<T> (Kịch bản 11)
            IRepository<ProjectorRemote> projectorRepo = new InMemoryRepository<ProjectorRemote>();
            IRepository<AirConditionerRemote> acRepo = new InMemoryRepository<AirConditionerRemote>();
            IRepository<WirelessMicrophone> micRepo = new InMemoryRepository<WirelessMicrophone>();

            // --- KỊCH BẢN 1 & 11: THÊM THIẾT BỊ VÀO CÁC REPOSITORY TỔNG QUÁT ---
            Console.WriteLine("--- [Kịch bản 1 & 11] Thêm thiết bị vào các Repository tổng quát ---");
            // 3 điều khiển máy chiếu có mức pin khác nhau (1 chiếc pin < 20% để kiểm thử lọc sẵn sàng)
            projectorRepo.Add(new ProjectorRemote("PR01", "ĐK Máy chiếu Sony", "Tủ A1", 55));
            projectorRepo.Add(new ProjectorRemote("PR02", "ĐK Máy chiếu Panasonic", "Tủ A1", 90));
            projectorRepo.Add(new ProjectorRemote("PR03", "ĐK Máy chiếu Epson", "Tủ A2", 15));

            // 2 điều khiển điều hòa với tập phòng tương thích khác nhau
            acRepo.Add(new AirConditionerRemote("AC01", "ĐK Điều hòa Daikin", "Tủ B1", new[] { "A101", "A102" }));
            acRepo.Add(new AirConditionerRemote("AC02", "ĐK Điều hòa Mitsubishi", "Tủ B2", new[] { "A203", "A204" }));

            // 3 micro không dây có pin và chất lượng tín hiệu khác nhau
            micRepo.Add(new WirelessMicrophone("MC01", "Micro Shure 01", "Tủ C1", 60, 70));  // TB: 65 (Sẵn sàng)
            micRepo.Add(new WirelessMicrophone("MC02", "Micro Sennheiser", "Tủ C1", 90, 80)); // TB: 85 (Sẵn sàng)
            micRepo.Add(new WirelessMicrophone("MC03", "Micro Bose", "Tủ C2", 25, 50));       // Pin 25% < 30% (Không sẵn sàng)

            Console.WriteLine("Đã thêm thành công: 3 ĐK máy chiếu, 2 ĐK điều hòa, 3 Micro không dây vào 3 InMemoryRepository<T> khác nhau.\n");

            // --- KỊCH BẢN 2: THỬ THÊM HAI THIẾT BỊ TRÙNG MÃ ---
            Console.WriteLine("--- [Kịch bản 2] Thử thêm thiết bị trùng mã định danh ---");
            try
            {
                projectorRepo.Add(new ProjectorRemote("PR01", "ĐK Máy chiếu Trùng Mã", "Tủ A3", 80));
            }
            catch (DuplicateIdException ex)
            {
                Console.WriteLine("-> Bắt ngoại lệ thành công [" + ex.GetType().Name + "]: " + ex.Message + "\n");
            }

            // --- KỊCH BẢN 3: TÌM THIẾT BỊ THEO MÃ ---
            Console.WriteLine("--- [Kịch bản 3] Tìm thiết bị theo mã ---");
            var foundProjector = projectorRepo.FindById("PR02");
            Console.WriteLine("Tìm mã 'PR02': " + (foundProjector != null ? foundProjector.ToString() : "Không tìm thấy"));
            var notFoundProjector = projectorRepo.FindById("PR99");
            Console.WriteLine("Tìm mã 'PR99': " + (notFoundProjector != null ? notFoundProjector.ToString() : "Không tìm thấy (null)") + "\n");

            // --- KỊCH BẢN 4: LỌC CÁC THIẾT BỊ SẴN SÀNG ---
            Console.WriteLine("--- [Kịch bản 4] Lọc các thiết bị sẵn sàng (IsReadyForUse == true) ---");
            Console.WriteLine("* ĐK Máy chiếu sẵn sàng (Pin >= 20% & Available):");
            foreach (var item in projectorRepo.FindAll(d => d.IsReadyForUse()))
            {
                Console.WriteLine("  " + item);
            }
            Console.WriteLine("* Micro không dây sẵn sàng (Pin >= 30%, Tín hiệu >= 40% & Available):");
            foreach (var item in micRepo.FindAll(d => d.IsReadyForUse()))
            {
                Console.WriteLine("  " + item);
            }
            Console.WriteLine();

            // --- KỊCH BẢN 5 & 10: CHỌN ĐK MÁY CHIẾU CÓ ĐIỂM CAO NHẤT & CHỨNG MINH CHUYỂN SANG BORROWED ---
            Console.WriteLine("--- [Kịch bản 5 & 10] Chọn ĐK máy chiếu có điểm ưu tiên cao nhất ---");
            var projectorStrategy = new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore());
            var projectorService = new AllocationService<ProjectorRemote>(projectorRepo, projectorStrategy);

            ProjectorRemote selectedProjector = projectorService.Allocate(d => true);
            Console.WriteLine("Thiết bị được chọn: " + selectedProjector);
            Console.WriteLine("-> Kiểm tra trạng thái sau cấp phát: Status = " + selectedProjector.Status + " (Đã chuyển sang Borrowed).\n");

            // --- KỊCH BẢN 6: CHỌN ĐK ĐIỀU HÒA TƯƠNG THÍCH VỚI MỘT PHÒNG ---
            Console.WriteLine("--- [Kịch bản 6] Chọn ĐK điều hòa tương thích với phòng A203 ---");
            var acStrategy = new FirstAvailableStrategy<AirConditionerRemote>();
            var acService = new AllocationService<AirConditionerRemote>(acRepo, acStrategy);

            AirConditionerRemote selectedAc = acService.Allocate(d => d.IsCompatibleWith("A203"));
            Console.WriteLine("Thiết bị được cấp phát cho phòng A203: " + selectedAc + "\n");

            // --- KỊCH BẢN 7: CHỌN MICRO ĐẦU TIÊN SẴN SÀNG ---
            Console.WriteLine("--- [Kịch bản 7] Chọn Micro không dây đầu tiên sẵn sàng (FirstAvailableStrategy) ---");
            var micService = new AllocationService<WirelessMicrophone>(
                micRepo,
                new FirstAvailableStrategy<WirelessMicrophone>());

            WirelessMicrophone firstMic = micService.Allocate(d => true);
            Console.WriteLine("Micro đầu tiên được chọn: " + firstMic);
            // Trả lại MC01 về kho để phục vụ so sánh khi đổi chiến lược ở Kịch bản 8
            firstMic.Return();
            Console.WriteLine("-> Đã gọi Return() trả [" + firstMic.Id + "] về trạng thái: " + firstMic.Status + "\n");

            // --- KỊCH BẢN 8: ĐỔI CHIẾN LƯỢC TRONG THỜI GIAN CHẠY VÀ QUAN SÁT KẾT QUẢ ---
            Console.WriteLine("--- [Kịch bản 8] Đổi chiến lược của dịch vụ Micro sang HighestPriorityStrategy lúc chạy ---");
            micService.ChangeStrategy(new HighestPriorityStrategy<WirelessMicrophone>(d => d.GetDefaultPriorityScore()));
            WirelessMicrophone bestMic = micService.Allocate(d => true);
            Console.WriteLine("Micro được chọn sau khi đổi sang HighestPriorityStrategy: " + bestMic + "\n");

            // --- KỊCH BẢN 9: THỬ CẤP PHÁT KHI KHÔNG CÓ ỨNG VIÊN PHÙ HỢP ---
            Console.WriteLine("--- [Kịch bản 9] Thử cấp phát khi không có ứng viên phù hợp ---");
            try
            {
                // Yêu cầu điều khiển điều hòa cho phòng B999 (không có thiết bị nào tương thích)
                acService.Allocate(d => d.IsCompatibleWith("B999"));
            }
            catch (NoSuitableItemException ex)
            {
                Console.WriteLine("-> Bắt ngoại lệ thành công [" + ex.GetType().Name + "]: " + ex.Message);
            }
        }
    }
}