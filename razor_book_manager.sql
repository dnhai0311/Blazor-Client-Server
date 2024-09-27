
-- Nếu xài Studio thì Tools -> NuGet... -> ...Console
-- Update-Database
-- Cái này cho VS Code
-- dotnet ef database update

-- --------------------------------------------------------

--
-- Đang đổ dữ liệu cho bảng `authors`
--

INSERT INTO `authors` (`Id`, `AuthorName`) VALUES
(1, 'Dương Ngọc Hải'),
(2, 'Trần Hiếu Nghĩa'),
(3, 'Tô Hoài'),
(4, 'Tố Hữu'),
(5, 'Nguyễn Trần Phương Tứng'),
(6, 'Hồ Quỳnh Hương'),
(7, 'Tun Phạm'),
(8, 'Tuấn Trần');

-- --------------------------------------------------------

--
-- Đang đổ dữ liệu cho bảng `booksales`
--

INSERT INTO `booksales` (`Id`, `Title`, `Quantity`, `Price`, `AuthorId`) VALUES
(1, 'Đường Hầm Mùa Hạ Tập 1', 1000, 120000, 1),
(2, 'Đường Hầm Mùa Hạ Tập 2', 1500, 120000, 1),
(3, 'Đường Hầm Mùa Hạ Tập 3', 500, 120000, 1),
(4, 'Đường Hầm Mùa Hạ Tập 4', 2000, 120000, 1),
(5, 'Đường Hầm Mùa Hạ Tập 5', 1000, 120000, 1),
(6, 'Đường Hầm Mùa Hạ Tập 6', 1500, 120000, 1),
(7, 'Đường Hầm Mùa Hạ Tập 7', 500, 120000, 1),
(8, 'Đường Hầm Mùa Hạ Tập 8', 2000, 120000, 1),
(9, 'Đường Hầm Mùa Hạ Tập 9', 1000, 120000, 1),
(10, 'Đường Hầm Mùa Hạ Tập 10', 1500, 120000, 1),
(11, 'Dế Mèo Phiêu Lưu Ký Tập 1', 500, 130000, 2),
(12, 'Dế Mèo Phiêu Lưu Ký Tập 2', 1000, 130000, 2),
(13, 'Dế Mèo Phiêu Lưu Ký Tập 3', 1500, 130000, 2),
(14, 'Dế Mèo Phiêu Lưu Ký Tập 4', 2000, 130000, 2),
(15, 'Dế Mèo Phiêu Lưu Ký Tập 5', 500, 130000, 2),
(16, 'Dế Mèo Phiêu Lưu Ký Tập 6', 1000, 130000, 2),
(17, 'Dế Mèo Phiêu Lưu Ký Tập 7', 1500, 130000, 2),
(18, 'Dế Mèo Phiêu Lưu Ký Tập 8', 2000, 130000, 2),
(19, 'Dế Mèo Phiêu Lưu Ký Tập 9', 500, 130000, 2),
(20, 'Dế Mèo Phiêu Lưu Ký Tập 10', 1000, 130000, 2),
(21, 'Hành Trình Đến Bắc Cực Tập 1', 1500, 140000, 3),
(22, 'Hành Trình Đến Bắc Cực Tập 2', 2000, 140000, 3),
(23, 'Hành Trình Đến Bắc Cực Tập 3', 500, 140000, 3),
(24, 'Hành Trình Đến Bắc Cực Tập 4', 1000, 140000, 3),
(25, 'Hành Trình Đến Bắc Cực Tập 5', 1500, 140000, 3),
(26, 'Hành Trình Đến Bắc Cực Tập 6', 2000, 140000, 3),
(27, 'Hành Trình Đến Bắc Cực Tập 7', 500, 140000, 3),
(28, 'Hành Trình Đến Bắc Cực Tập 8', 1000, 140000, 3),
(29, 'Hành Trình Đến Bắc Cực Tập 9', 1500, 140000, 3),
(30, 'Hành Trình Đến Bắc Cực Tập 10', 2000, 140000, 3),
(31, 'Bỗng Dưng Muốn Khóc Tập 1', 1000, 110000, 4),
(32, 'Bỗng Dưng Muốn Khóc Tập 2', 1500, 110000, 4),
(33, 'Bỗng Dưng Muốn Khóc Tập 3', 500, 110000, 4),
(34, 'Bỗng Dưng Muốn Khóc Tập 4', 2000, 110000, 4),
(35, 'Bỗng Dưng Muốn Khóc Tập 5', 1000, 110000, 4),
(36, 'Bỗng Dưng Muốn Khóc Tập 6', 1500, 110000, 4),
(37, 'Bỗng Dưng Muốn Khóc Tập 7', 500, 110000, 4),
(38, 'Bỗng Dưng Muốn Khóc Tập 8', 2000, 110000, 4),
(39, 'Bỗng Dưng Muốn Khóc Tập 9', 1000, 110000, 4),
(40, 'Bỗng Dưng Muốn Khóc Tập 10', 1500, 110000, 4),
(41, 'Tên Của Bạn Là Tập 1', 500, 150000, 5),
(42, 'Tên Của Bạn Là Tập 2', 1000, 150000, 5),
(43, 'Tên Của Bạn Là Tập 3', 1500, 150000, 5),
(44, 'Tên Của Bạn Là Tập 4', 2000, 150000, 5),
(45, 'Tên Của Bạn Là Tập 5', 500, 150000, 5),
(46, 'Tên Của Bạn Là Tập 6', 1000, 150000, 5),
(47, 'Tên Của Bạn Là Tập 7', 1500, 150000, 5),
(48, 'Tên Của Bạn Là Tập 8', 2000, 150000, 5),
(49, 'Tên Của Bạn Là Tập 9', 500, 150000, 5),
(50, 'Tên Của Bạn Là Tập 10', 1000, 150000, 5),
(51, 'Hãy Theo Dõi Nào', 1000, 50000, 2),
(53, 'Hành Trình Đến Hạnh Phúc', 500, 100000, 2),
(56, 'Đường Hầm Mùa Hạ Tập 11', 1000, 120000, 1);

-- --------------------------------------------------------
