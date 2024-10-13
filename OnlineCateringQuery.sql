create database OnlineCatering

use OnlineCatering


create table UserInfo(

	UserId int primary key identity(1,1),
	UserName varchar(55),
	UserImage varchar(Max),
	UserEmail varchar(55) unique,
	UserPassword varchar(55),
	UserAddress varchar(55),
	UserPhoneNo bigint,
	UserRole varchar(55),
)

create table Venue(
	VenueID int primary key identity(1,1),
	VenueName varchar(55),
	CatererVenue int
	foreign key (CatererVenue) references UserInfo(UserId),
)

create table Menu(
	MenuID int primary key identity(1,1),
	MenuName varchar(55),
	MenuDescription varchar(55),
	MenuImage varchar(Max),
	MenuPrice int,
	CatererMenu int
	foreign key (CatererMenu) references UserInfo(UserId),
	MenuCat int
	foreign key (MenuCat) references Category(CategoryID),
)

create table Booking(
	BookingID int primary key identity(1,1),
	BookingCaterer int
	foreign key (BookingCaterer) references UserInfo(UserId),
	BookingVenue int
	foreign key (BookingVenue) references Venue(VenueID),
	BookingMenu int
	foreign key (BookingMenu) references Menu(MenuID),
	BookingPrice int,
	TotalGuest int,
	BookingDate Date,
)


create table CardInfo(
	CardID int primary key identity(1,1),
	CardNumber bigint,
	CardName varchar(55),
	CardEx_MM int,
	CardEx_YY int,
	CardCVV int,
)

--Messages will be saved in Session Storage
--Message box me kese pata chalega ke konsa caterer hai??
--question 5 messages wala sir se puchna hai.

--question 7 ka c puchna hai.

create table Category(
	CategoryID int primary key identity(1,1),
	CategoryName varchar(55),
	Category varchar(Max),
)



