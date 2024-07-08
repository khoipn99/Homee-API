CREATE DATABASE Homee;

GO
USE [Homee]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[chefs]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chefs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[address] [nvarchar](255) NULL,
	[creator_id] [int] NULL,
	[profile_picture] [nvarchar](255) NULL,
	[score] [decimal](3, 2) NULL,
	[hours] [int] NULL,
	[status] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[password] [nvarchar](255) NULL,
	[phone] [nvarchar](20) NULL,
	[money] [decimal](18, 2) NULL,
	[banking] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[comments]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[content] [text] NOT NULL,
	[sent_date] [datetime] NOT NULL,
	[order_id] [int] NULL,
	[star] [int] NULL,
	[status] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[foods]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[foods](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[image] [nvarchar](255) NULL,
	[food_type] [nvarchar](255) NULL,
	[price] [decimal](10, 2) NOT NULL,
	[sell_price] [decimal](10, 2) NOT NULL,
	[category_id] [int] NULL,
	[chef_id] [int] NULL,
	[sell_count] [int] NULL,
	[status] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_details]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[food_id] [int] NULL,
	[price] [decimal](10, 2) NOT NULL,
	[quantity] [int] NOT NULL,
	[order_id] [int] NULL,
	[status] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[chef_id] [int] NULL,
	[delivery_address] [nvarchar](255) NULL,
	[order_price] [decimal](10, 2) NOT NULL,
	[quantity] [int] NOT NULL,
	[user_id] [int] NULL,
	[status] [nvarchar](255) NULL,
	[order_date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NULL,
	[payment_date] [datetime] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
	[payment_type] [nvarchar](255) NULL,
	[discount] [decimal](5, 2) NULL,
	[user_id] [int] NULL,
	[status] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[top_up_requests]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[top_up_requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ChefId] [int] NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[ApprovalDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[first_name] [nvarchar](255) NULL,
	[last_name] [nvarchar](255) NULL,
	[password] [nvarchar](255) NOT NULL,
	[phone] [nvarchar](20) NULL,
	[address] [nvarchar](255) NULL,
	[dob] [date] NULL,
	[gender] [nvarchar](10) NULL,
	[avatar] [nvarchar](255) NULL,
	[role_id] [int] NULL,
	[status] [nvarchar](255) NULL,
	[money] [decimal](10, 2) NULL,
	[discount] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[vouchers]    Script Date: 7/7/2024 11:43:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vouchers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[discount] [decimal](5, 2) NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[categories] ON 

INSERT [dbo].[categories] ([id], [name]) VALUES (1, N'Ð? an nhanh')
INSERT [dbo].[categories] ([id], [name]) VALUES (2, N'Món tráng mi?ng')
INSERT [dbo].[categories] ([id], [name]) VALUES (3, N'Món chính')
INSERT [dbo].[categories] ([id], [name]) VALUES (4, N'Ð? u?ng')
INSERT [dbo].[categories] ([id], [name]) VALUES (5, N'Món chay')
SET IDENTITY_INSERT [dbo].[categories] OFF
GO
SET IDENTITY_INSERT [dbo].[chefs] ON 

INSERT [dbo].[chefs] ([id], [name], [address], [creator_id], [profile_picture], [score], [hours], [status], [email], [password], [phone], [money], [banking]) VALUES (1, N'Nguy?n Van A', N'123 Ðu?ng ABC, Qu?n 1, TP.HCM', 1, N'avatar1.png', CAST(4.50 AS Decimal(3, 2)), 40, N'Ho?t d?ng', N'nva@example.com', N'password123', N'0123456789', CAST(1500000.00 AS Decimal(18, 2)), N'Vietcombank')
INSERT [dbo].[chefs] ([id], [name], [address], [creator_id], [profile_picture], [score], [hours], [status], [email], [password], [phone], [money], [banking]) VALUES (2, N'Tr?n Th? B', N'456 Ðu?ng XYZ, Qu?n 2, TP.HCM', 2, N'avatar2.png', CAST(4.80 AS Decimal(3, 2)), 35, N'Ho?t d?ng', N'ttb@example.com', N'password456', N'0987654321', CAST(1200000.00 AS Decimal(18, 2)), N'ACB')
INSERT [dbo].[chefs] ([id], [name], [address], [creator_id], [profile_picture], [score], [hours], [status], [email], [password], [phone], [money], [banking]) VALUES (3, N'Lê Van C', N'789 Ðu?ng PQR, Qu?n 3, TP.HCM', 3, N'avatar3.png', CAST(4.20 AS Decimal(3, 2)), 50, N'Ngh? phép', N'lvc@example.com', N'password789', N'0234567890', CAST(900000.00 AS Decimal(18, 2)), N'Techcombank')
INSERT [dbo].[chefs] ([id], [name], [address], [creator_id], [profile_picture], [score], [hours], [status], [email], [password], [phone], [money], [banking]) VALUES (4, N'Ph?m Th? D', N'101 Ðu?ng LMN, Qu?n 4, TP.HCM', 4, N'avatar4.png', CAST(4.90 AS Decimal(3, 2)), 60, N'Ho?t d?ng', N'ptd@example.com', N'password012', N'0345678901', CAST(1500000.00 AS Decimal(18, 2)), N'BIDV')
INSERT [dbo].[chefs] ([id], [name], [address], [creator_id], [profile_picture], [score], [hours], [status], [email], [password], [phone], [money], [banking]) VALUES (5, N'Hoàng Van E', N'202 Ðu?ng TUV, Qu?n 5, TP.HCM', 5, N'avatar5.png', CAST(4.70 AS Decimal(3, 2)), 45, N'Ho?t d?ng', N'hve@example.com', N'password345', N'0456789012', CAST(1100000.00 AS Decimal(18, 2)), N'Agribank')
SET IDENTITY_INSERT [dbo].[chefs] OFF
GO
SET IDENTITY_INSERT [dbo].[comments] ON 

INSERT [dbo].[comments] ([id], [user_id], [content], [sent_date], [order_id], [star], [status]) VALUES (1, 1, N'Món an r?t ngon, s? ?ng h? l?n sau.', CAST(N'2023-07-01T14:40:00.000' AS DateTime), 1, 5, N'Hi?n th?')
INSERT [dbo].[comments] ([id], [user_id], [content], [sent_date], [order_id], [star], [status]) VALUES (2, 2, N'Ph?c v? nhanh, d? an nóng h?i.', CAST(N'2023-07-02T15:10:00.000' AS DateTime), 2, 4, N'Hi?n th?')
INSERT [dbo].[comments] ([id], [user_id], [content], [sent_date], [order_id], [star], [status]) VALUES (3, 3, N'Giá c? h?p lý, món an da d?ng.', CAST(N'2023-07-03T16:55:00.000' AS DateTime), 3, 5, N'Hi?n th?')
INSERT [dbo].[comments] ([id], [user_id], [content], [sent_date], [order_id], [star], [status]) VALUES (4, 4, N'Ch?t lu?ng d?ch v? không t?t.', CAST(N'2023-07-04T18:30:00.000' AS DateTime), 4, 2, N'?n')
INSERT [dbo].[comments] ([id], [user_id], [content], [sent_date], [order_id], [star], [status]) VALUES (5, 5, N'Giao hàng nhanh, d? an ngon.', CAST(N'2023-07-05T19:20:00.000' AS DateTime), 5, 4, N'Hi?n th?')
SET IDENTITY_INSERT [dbo].[comments] OFF
GO
SET IDENTITY_INSERT [dbo].[foods] ON 

INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (1, N'Bánh mì kẹp', N'banhmi.png', N'Ðồ an nhanh', CAST(15000.00 AS Decimal(10, 2)), CAST(18000.00 AS Decimal(10, 2)), 1, 1, 100, N'Có s?n')
INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (2, N'Chè đậu xanh', N'che.png', N'Món tráng mi?ng', CAST(12000.00 AS Decimal(10, 2)), CAST(15000.00 AS Decimal(10, 2)), 2, 2, 50, N'Có s?n')
INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (3, N'Cơm tấm', N'comtam.png', N'Món chính', CAST(30000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), 3, 3, 70, N'Có s?n')
INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (4, N'Trà sữa', N'trasua.png', N'Ðồ uống', CAST(25000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), 4, 4, 200, N'Có s?n')
INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (5, N'Gỏi cuốn chay', N'goicuonchay.png', N'Món chay', CAST(20000.00 AS Decimal(10, 2)), CAST(25000.00 AS Decimal(10, 2)), 5, 5, 80, N'Có s?n')
INSERT [dbo].[foods] ([id], [name], [image], [food_type], [price], [sell_price], [category_id], [chef_id], [sell_count], [status]) VALUES (6, N'Canh chua', N'123', N'Món chính', CAST(30000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), 3, 1, 20, N'Có sẵn')
SET IDENTITY_INSERT [dbo].[foods] OFF
GO
SET IDENTITY_INSERT [dbo].[order_details] ON 

INSERT [dbo].[order_details] ([id], [food_id], [price], [quantity], [order_id], [status]) VALUES (1, 1, CAST(18000.00 AS Decimal(10, 2)), 2, 1, N'Ðang giao')
INSERT [dbo].[order_details] ([id], [food_id], [price], [quantity], [order_id], [status]) VALUES (2, 2, CAST(15000.00 AS Decimal(10, 2)), 1, 2, N'Ðang chu?n b?')
INSERT [dbo].[order_details] ([id], [food_id], [price], [quantity], [order_id], [status]) VALUES (3, 3, CAST(35000.00 AS Decimal(10, 2)), 1, 3, N'Hoàn thành')
INSERT [dbo].[order_details] ([id], [food_id], [price], [quantity], [order_id], [status]) VALUES (4, 4, CAST(30000.00 AS Decimal(10, 2)), 3, 4, N'Ðã h?y')
INSERT [dbo].[order_details] ([id], [food_id], [price], [quantity], [order_id], [status]) VALUES (5, 5, CAST(25000.00 AS Decimal(10, 2)), 1, 5, N'Ðang giao')
SET IDENTITY_INSERT [dbo].[order_details] OFF
GO
SET IDENTITY_INSERT [dbo].[orders] ON 

INSERT [dbo].[orders] ([id], [chef_id], [delivery_address], [order_price], [quantity], [user_id], [status], [order_date]) VALUES (1, 1, N'123 Ðu?ng ABC, Qu?n 1, TP.HCM', CAST(180000.00 AS Decimal(10, 2)), 3, 1, N'Ðang giao', CAST(N'2023-07-01' AS Date))
INSERT [dbo].[orders] ([id], [chef_id], [delivery_address], [order_price], [quantity], [user_id], [status], [order_date]) VALUES (2, 2, N'456 Ðu?ng XYZ, Qu?n 2, TP.HCM', CAST(45000.00 AS Decimal(10, 2)), 2, 2, N'Ðang chu?n b?', CAST(N'2023-07-02' AS Date))
INSERT [dbo].[orders] ([id], [chef_id], [delivery_address], [order_price], [quantity], [user_id], [status], [order_date]) VALUES (3, 3, N'789 Ðu?ng PQR, Qu?n 3, TP.HCM', CAST(70000.00 AS Decimal(10, 2)), 1, 3, N'Hoàn thành', CAST(N'2023-07-03' AS Date))
INSERT [dbo].[orders] ([id], [chef_id], [delivery_address], [order_price], [quantity], [user_id], [status], [order_date]) VALUES (4, 4, N'101 Ðu?ng LMN, Qu?n 4, TP.HCM', CAST(120000.00 AS Decimal(10, 2)), 4, 4, N'Ðã h?y', CAST(N'2023-07-04' AS Date))
INSERT [dbo].[orders] ([id], [chef_id], [delivery_address], [order_price], [quantity], [user_id], [status], [order_date]) VALUES (5, 5, N'202 Ðu?ng TUV, Qu?n 5, TP.HCM', CAST(75000.00 AS Decimal(10, 2)), 2, 5, N'Ðang giao', CAST(N'2023-07-05' AS Date))
SET IDENTITY_INSERT [dbo].[orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([id], [order_id], [payment_date], [total_price], [payment_type], [discount], [user_id], [status]) VALUES (1, 1, CAST(N'2023-07-01T14:30:00.000' AS DateTime), CAST(180000.00 AS Decimal(10, 2)), N'Th? tín d?ng', CAST(10.00 AS Decimal(5, 2)), 1, N'Thành công')
INSERT [dbo].[Payment] ([id], [order_id], [payment_date], [total_price], [payment_type], [discount], [user_id], [status]) VALUES (2, 2, CAST(N'2023-07-02T15:00:00.000' AS DateTime), CAST(45000.00 AS Decimal(10, 2)), N'Ti?n m?t', CAST(5.00 AS Decimal(5, 2)), 2, N'Thành công')
INSERT [dbo].[Payment] ([id], [order_id], [payment_date], [total_price], [payment_type], [discount], [user_id], [status]) VALUES (3, 3, CAST(N'2023-07-03T16:45:00.000' AS DateTime), CAST(70000.00 AS Decimal(10, 2)), N'Ví di?n t?', CAST(20.00 AS Decimal(5, 2)), 3, N'Thành công')
INSERT [dbo].[Payment] ([id], [order_id], [payment_date], [total_price], [payment_type], [discount], [user_id], [status]) VALUES (4, 4, CAST(N'2023-07-04T18:20:00.000' AS DateTime), CAST(120000.00 AS Decimal(10, 2)), N'Th? tín d?ng', CAST(15.00 AS Decimal(5, 2)), 4, N'Th?t b?i')
INSERT [dbo].[Payment] ([id], [order_id], [payment_date], [total_price], [payment_type], [discount], [user_id], [status]) VALUES (5, 5, CAST(N'2023-07-05T19:10:00.000' AS DateTime), CAST(75000.00 AS Decimal(10, 2)), N'Ti?n m?t', CAST(25.00 AS Decimal(5, 2)), 5, N'Thành công')
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[roles] ON 

INSERT [dbo].[roles] ([id], [name]) VALUES (1, N'Admin')
INSERT [dbo].[roles] ([id], [name]) VALUES (2, N'User')
INSERT [dbo].[roles] ([id], [name]) VALUES (3, N'Chef')
SET IDENTITY_INSERT [dbo].[roles] OFF
GO
SET IDENTITY_INSERT [dbo].[top_up_requests] ON 

INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (1, 1, NULL, CAST(200000.00 AS Decimal(18, 2)), CAST(N'2023-07-01T09:00:00.000' AS DateTime), 1, CAST(N'2023-07-01T10:00:00.000' AS DateTime))
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (2, NULL, 2, CAST(300000.00 AS Decimal(18, 2)), CAST(N'2023-07-02T10:30:00.000' AS DateTime), 1, CAST(N'2023-07-02T11:00:00.000' AS DateTime))
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (3, 3, NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(N'2023-07-03T11:45:00.000' AS DateTime), 1, CAST(N'2023-07-03T12:00:00.000' AS DateTime))
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (4, NULL, 4, CAST(250000.00 AS Decimal(18, 2)), CAST(N'2023-07-04T14:20:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (5, 5, NULL, CAST(150000.00 AS Decimal(18, 2)), CAST(N'2023-07-05T15:10:00.000' AS DateTime), 1, CAST(N'2023-07-05T16:00:00.000' AS DateTime))
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (6, NULL, 1, CAST(500000.00 AS Decimal(18, 2)), CAST(N'2024-07-07T09:24:13.150' AS DateTime), 0, NULL)
INSERT [dbo].[top_up_requests] ([Id], [UserId], [ChefId], [Amount], [RequestDate], [IsApproved], [ApprovalDate]) VALUES (7, NULL, 1, CAST(500000.00 AS Decimal(18, 2)), CAST(N'2024-07-07T09:24:15.050' AS DateTime), 1, CAST(N'2024-07-07T09:24:48.897' AS DateTime))
SET IDENTITY_INSERT [dbo].[top_up_requests] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id], [email], [first_name], [last_name], [password], [phone], [address], [dob], [gender], [avatar], [role_id], [status], [money], [discount]) VALUES (1, N'user1@example.com', N'Nguy?n', N'Van A', N'password123', N'0123456789', N'123 Ðu?ng ABC, Qu?n 1, TP.HCM', CAST(N'1990-01-01' AS Date), N'Nam', N'avatar1.png', 2, N'Ho?t d?ng', CAST(500000.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(5, 2)))
INSERT [dbo].[users] ([id], [email], [first_name], [last_name], [password], [phone], [address], [dob], [gender], [avatar], [role_id], [status], [money], [discount]) VALUES (2, N'user2@example.com', N'Tr?n', N'Th? B', N'password456', N'0987654321', N'456 Ðu?ng XYZ, Qu?n 2, TP.HCM', CAST(N'1992-02-02' AS Date), N'N?', N'avatar2.png', 2, N'Ho?t d?ng', CAST(600000.00 AS Decimal(10, 2)), CAST(20.00 AS Decimal(5, 2)))
INSERT [dbo].[users] ([id], [email], [first_name], [last_name], [password], [phone], [address], [dob], [gender], [avatar], [role_id], [status], [money], [discount]) VALUES (3, N'user3@example.com', N'Lê', N'Van C', N'password789', N'0234567890', N'789 Ðu?ng PQR, Qu?n 3, TP.HCM', CAST(N'1994-03-03' AS Date), N'Nam', N'avatar3.png', 2, N'Ho?t d?ng', CAST(700000.00 AS Decimal(10, 2)), CAST(30.00 AS Decimal(5, 2)))
INSERT [dbo].[users] ([id], [email], [first_name], [last_name], [password], [phone], [address], [dob], [gender], [avatar], [role_id], [status], [money], [discount]) VALUES (4, N'user4@example.com', N'Ph?m', N'Th? D', N'password012', N'0345678901', N'101 Ðu?ng LMN, Qu?n 4, TP.HCM', CAST(N'1996-04-04' AS Date), N'N?', N'avatar4.png', 2, N'Ho?t d?ng', CAST(800000.00 AS Decimal(10, 2)), CAST(40.00 AS Decimal(5, 2)))
INSERT [dbo].[users] ([id], [email], [first_name], [last_name], [password], [phone], [address], [dob], [gender], [avatar], [role_id], [status], [money], [discount]) VALUES (5, N'user5@example.com', N'Hoàng', N'Van E', N'password345', N'0456789012', N'202 Ðu?ng TUV, Qu?n 5, TP.HCM', CAST(N'1998-05-05' AS Date), N'Nam', N'avatar5.png', 2, N'Ho?t d?ng', CAST(900000.00 AS Decimal(10, 2)), CAST(50.00 AS Decimal(5, 2)))
SET IDENTITY_INSERT [dbo].[users] OFF
GO
SET IDENTITY_INSERT [dbo].[vouchers] ON 

INSERT [dbo].[vouchers] ([id], [name], [discount], [quantity]) VALUES (1, N'Gi?m giá 10%', CAST(10.00 AS Decimal(5, 2)), 100)
INSERT [dbo].[vouchers] ([id], [name], [discount], [quantity]) VALUES (2, N'Gi?m giá 20%', CAST(20.00 AS Decimal(5, 2)), 50)
INSERT [dbo].[vouchers] ([id], [name], [discount], [quantity]) VALUES (3, N'Gi?m giá 30%', CAST(30.00 AS Decimal(5, 2)), 20)
INSERT [dbo].[vouchers] ([id], [name], [discount], [quantity]) VALUES (4, N'Gi?m giá 50%', CAST(50.00 AS Decimal(5, 2)), 10)
INSERT [dbo].[vouchers] ([id], [name], [discount], [quantity]) VALUES (5, N'Mi?n phí giao hàng', CAST(100.00 AS Decimal(5, 2)), 200)
SET IDENTITY_INSERT [dbo].[vouchers] OFF
GO
ALTER TABLE [dbo].[top_up_requests] ADD  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[comments]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([id])
GO
ALTER TABLE [dbo].[comments]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[foods]  WITH CHECK ADD FOREIGN KEY([category_id])
REFERENCES [dbo].[categories] ([id])
GO
ALTER TABLE [dbo].[foods]  WITH CHECK ADD FOREIGN KEY([chef_id])
REFERENCES [dbo].[chefs] ([id])
GO
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD FOREIGN KEY([food_id])
REFERENCES [dbo].[foods] ([id])
GO
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([id])
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD FOREIGN KEY([chef_id])
REFERENCES [dbo].[chefs] ([id])
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([id])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[top_up_requests]  WITH CHECK ADD  CONSTRAINT [FK_TopUpRequests_Chefs] FOREIGN KEY([ChefId])
REFERENCES [dbo].[chefs] ([id])
GO
ALTER TABLE [dbo].[top_up_requests] CHECK CONSTRAINT [FK_TopUpRequests_Chefs]
GO
ALTER TABLE [dbo].[top_up_requests]  WITH CHECK ADD  CONSTRAINT [FK_TopUpRequests_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[top_up_requests] CHECK CONSTRAINT [FK_TopUpRequests_Users]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD FOREIGN KEY([role_id])
REFERENCES [dbo].[roles] ([id])
GO
ALTER TABLE [dbo].[top_up_requests]  WITH CHECK ADD  CONSTRAINT [CK_TopUpRequests_UserOrChef] CHECK  (([UserId] IS NOT NULL AND [ChefId] IS NULL OR [UserId] IS NULL AND [ChefId] IS NOT NULL))
GO
ALTER TABLE [dbo].[top_up_requests] CHECK CONSTRAINT [CK_TopUpRequests_UserOrChef]
GO
