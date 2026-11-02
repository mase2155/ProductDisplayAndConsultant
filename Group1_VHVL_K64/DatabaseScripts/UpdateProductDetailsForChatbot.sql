-- Mô tả chi tiết sản phẩm phục vụ trang web và chatbot tư vấn.
-- Chạy trên database HoangHoanFurniture (UTF-8):
--   chcp 65001
--   sqlcmd -S . -d HoangHoanFurniture -E -f 65001 -i UpdateProductDetailsForChatbot.sql

UPDATE Products SET Details = N'Bàn uống nước phòng khách phong cách hiện đại, mặt gỗ nâu đậm chống trầy. Kích thước vừa, phù hợp sofa 2–3 chỗ. Hoàn thiện bóng mờ, dễ lau chùi hàng ngày.' WHERE ID = 1;
UPDATE Products SET Details = N'Bàn uống nước mặt gỗ nâu vân tự nhiên, viền bo tròn an toàn. Size vừa đặt cạnh ghế bành, tông ấm hài hòa nội thất gỗ. Chân bàn chắc chắn, chịu lực tốt.' WHERE ID = 2;
UPDATE Products SET Details = N'Kệ TV gỗ lim 4 ngăn, kết hợp kho trưng bày và giấu dây điện. Màu gỗ sang trọng, phù hợp phòng khách rộng. Ngăn rộng đặt loa, đầu phát và đồ trang trí.' WHERE ID = 3;
UPDATE Products SET Details = N'Kệ để đồ 4 ngăn màu trắng phủ gỗ, phong cách Bắc Âu gọn gàng. Dùng trang trí phòng khách hoặc hành lang. Lắp ráp đơn giản, bề mặt chống ẩm cơ bản.' WHERE ID = 4;

UPDATE Products SET Details = N'Giường châu Âu đầu giường bọc nệm, nâu đậm cao cấp. Size vừa (phổ biến 1m6), khung chắc ít rung. Phù hợp phòng ngủ master hiện đại.' WHERE ID = 5;
UPDATE Products SET Details = N'Giường gỗ tự nhiên màu nâu, thiết kế tối giản. Size vừa, sàn giường chắc chịu tải. Dễ phối tủ quần áo và bàn trang điểm cùng tông màu.' WHERE ID = 6;
UPDATE Products SET Details = N'Kệ đầu giường tiện để sách, đèn ngủ và đồ dùng hằng ngày. Màu nâu đậm đồng bộ giường ngủ. Kích thước vừa, không chiếm diện tích.' WHERE ID = 7;
UPDATE Products SET Details = N'Tủ quần áo nhiều ngăn, cánh mở rộng, màu nâu đậm. Size vừa cho phòng ngủ 15–20m². Có thanh treo áo dài và kệ gấp đồ.' WHERE ID = 8;

UPDATE Products SET Details = N'Bàn ăn 6 ghế, mặt gỗ nâu đậm chống nước nhẹ. Phù hợp gia đình 4–6 người. Chân bàn vững, dễ kết hợp ghế bọc nỉ hoặc gỗ.' WHERE ID = 9;
UPDATE Products SET Details = N'Bàn trà nhỏ gọn cho phòng bếp mở hoặc ban công. Màu nâu ấm, mặt bàn dễ vệ sinh. Dùng uống trà, ăn nhẹ hoặc làm bàn phụ.' WHERE ID = 10;
UPDATE Products SET Details = N'Bàn ăn 4 ghế size vừa, tiết kiệm diện tích căn hộ. Gỗ nâu đậm, phù hợp nhà phố. Mặt bàn chống trầy cơ bản.' WHERE ID = 11;
UPDATE Products SET Details = N'Bàn ăn 8 ghế cho nhà rộng hoặc tiệc gia đình. Thiết kế sang, mặt gỗ dày. Chân bàn chịu lực, bề mặt dễ lau sau bữa ăn.' WHERE ID = 12;

UPDATE Products SET Details = N'Bồn rửa mặt gốm men sứ, màu nâu đậm hài hòa phòng tắm. Size vừa, lỗ vòi chuẩn. Men bóng chống bám bẩn, dễ vệ sinh.' WHERE ID = 13;
UPDATE Products SET Details = N'Bồn rửa tay compact cho WC phụ hoặc phòng tắm nhỏ. Màu nâu đậm, lắp âm hoặc đặt bàn. Phù hợp căn hộ diện tích hạn chế.' WHERE ID = 14;
UPDATE Products SET Details = N'Bồn rửa đôi hai hố tiện cho gia đình đông người. Chất liệu bền, men cao cấp. Mặt rộng đặt thêm mỹ phẩm, gương soi lớn.' WHERE ID = 15;
UPDATE Products SET Details = N'Tủ âm tường phòng tắm giấu ống nước, tăng không gian lưu trữ. Màu nâu đậm chống ẩm cơ bản. Ngăn kéo và kệ mở tùy chỉnh.' WHERE ID = 16;

UPDATE Products SET Details = N'Bàn học trẻ em mặt rộng, ngăn kéo đựng sách vở. Màu nâu đậm dễ phối phòng bé. Chiều cao phù hợp học sinh tiểu học.' WHERE ID = 17;
UPDATE Products SET Details = N'Giường tầng gỗ an toàn, lan can chắc, thang bám chống trượt. Tiết kiệm diện tích phòng trẻ. Size vừa, sơn chống mối mọt.' WHERE ID = 18;
UPDATE Products SET Details = N'Giường tầng 2 màu gỗ tự nhiên, thiết kế 2 giường xếp. Phù hợp anh em hoặc khách. Khung giường chịu lực, ván sàn chắc.' WHERE ID = 19;
UPDATE Products SET Details = N'Cũi trẻ em gỗ làm mộc kỹ, bo cạnh an toàn. Size vừa cho bé sơ sinh đến 2 tuổi (theo hướng dẫn sử dụng). Có thể hạ nệm điều chỉnh độ cao.' WHERE ID = 20;

UPDATE Products SET Details = N'Ghế văn phòng lưng cao, tựa đầu thoải mái. Màu nâu đậm phối bàn làm việc. Chân xoay 360°, có thể nâng hạ chiều cao.' WHERE ID = 21;
UPDATE Products SET Details = N'Bàn làm việc gọn, mặt gỗ chống trầy. Size vừa đặt laptop và tài liệu. Phù hợp home office hoặc phòng ngủ.' WHERE ID = 22;
UPDATE Products SET Details = N'Bàn làm việc 2 mặt rộng hơn, hộc kéo bên hông. Gỗ nâu đậm sang trọng. Dành cho dân văn phòng cần không gian đặt màn hình kép.' WHERE ID = 23;
UPDATE Products SET Details = N'Giá sách nhiều tầng, chịu lực sách nặng. Màu nâu đậm hài hòa nội thất. Dùng phòng làm việc hoặc phòng khách.' WHERE ID = 24;

UPDATE Products SET Details = N'Cầu thang gỗ mẫu 1: tay vịn cong, bậc phẳng chống trơn. Màu nâu đậm bảo hành thi công theo số đo thực tế. Liên hệ shop đo đạc tại nhà.' WHERE ID = 25;
UPDATE Products SET Details = N'Cầu thang gỗ mẫu 2: thiết kế chữ L tiết kiệm diện tích. Gỗ xử lý chống mối, bề mặt bóng mờ. Thi công theo yêu cầu công trình.' WHERE ID = 26;
UPDATE Products SET Details = N'Cầu thang gỗ mẫu 3: phong cách hiện đại, lan can kính hoặc gỗ. Màu nâu ấm. Báo giá sau khảo sát hiện trạng.' WHERE ID = 27;
UPDATE Products SET Details = N'Cầu thang gỗ mẫu 4: sang trọng cho biệt thự, bậc rộng thoải mái. Gỗ tự nhiên chọn lọc. Shop hỗ trợ thiết kế 3D sơ bộ.' WHERE ID = 28;

UPDATE Products SET Details = N'Gương treo tường 1, khung gỗ nâu, kích thước vừa phòng khách. Soi rộng, tạo cảm giác không gian thoáng. Dễ treo, vít kèm theo.' WHERE ID = 29;
UPDATE Products SET Details = N'Gương treo tường 2, viền mỏng hiện đại. Phù hợp hành lang hoặc phòng ngủ. Mặt gương sáng, ít méo hình.' WHERE ID = 30;
UPDATE Products SET Details = N'Ghế đôn bọc nỉ cao cấp, màu nâu đậm. Dùng phòng khách hoặc cuối giường. Mềm êm, chân gỗ chắc.' WHERE ID = 31;
UPDATE Products SET Details = N'Đèn chùm trang trí phòng khách, ánh sáng ấm. Thiết kế nhiều bóng, tạo điểm nhấn trần nhà. Lắp đặt theo hướng dẫn điện an toàn.' WHERE ID = 32;
