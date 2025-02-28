create database Gearup;
use Gearup;

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incremented User ID
    Username NVARCHAR(50) NOT NULL,         -- Username (50 characters max)
    Password NVARCHAR(255) NOT NULL         -- Password (hashed or plain text, 255 characters max)
);

INSERT INTO Users (Username, Password)
VALUES ('sampleUser', 'samplePassword');

select * from Users;
