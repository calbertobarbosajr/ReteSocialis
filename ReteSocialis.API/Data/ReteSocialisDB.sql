/* ===========================================================
   BANCO DE DADOS: ReteSocialisDB
   =========================================================== */
IF DB_ID('ReteSocialisDB') IS NOT NULL
BEGIN
    ALTER DATABASE ReteSocialisDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ReteSocialisDB;
END;
GO

CREATE DATABASE ReteSocialisDB;
GO
USE ReteSocialisDB;
GO

/* ===========================================================
   TABELA: Accounts (Usuários)
   =========================================================== */
CREATE TABLE Accounts (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    DisplayName NVARCHAR(100),
    Bio NVARCHAR(255),
    ProfileImageUrl NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);
GO

INSERT INTO Accounts (Username, Email, PasswordHash, DisplayName)
VALUES
('admin', 'admin@rede.com', 'HASH123', 'Administrador'),
('joaosilva', 'joao@rede.com', 'HASH123', 'João Silva'),
('maria', 'maria@rede.com', 'HASH123', 'Maria Souza');
GO

/* ===========================================================
   TABELA: Blogs (Publicações)
   =========================================================== */
CREATE TABLE Blogs (
    BlogID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Subject NVARCHAR(100),
    PageName NVARCHAR(100) NOT NULL,
    Post NVARCHAR(MAX) NOT NULL,
    IsPublished BIT DEFAULT 0,
    CreateDate DATETIME2 DEFAULT GETDATE(),
    UpdateDate DATETIME2 NULL,
    CONSTRAINT FK_Blogs_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE CASCADE
);
GO

INSERT INTO Blogs (AccountID, Title, Subject, PageName, Post, IsPublished)
VALUES
(2, 'Meu Primeiro Post', 'Introdução', 'meu-primeiro-post', 'Olá mundo! Este é meu primeiro post.', 1),
(3, 'Viagem para o interior', 'Diário', 'viagem-interior', 'Hoje compartilho minha viagem ao interior do Brasil.', 1);
GO

/* ===========================================================
   TABELA: Alerts / AlertTypes (Notificações)
   =========================================================== */
CREATE TABLE AlertTypes (
    AlertTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL
);
GO

INSERT INTO AlertTypes (TypeName)
VALUES ('Novo Blog Post'), ('Blog Atualizado'), ('Nova Amizade'), ('Comentário Recebido');
GO

CREATE TABLE Alerts (
    AlertID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT NOT NULL,
    AlertTypeID INT NOT NULL,
    Message NVARCHAR(500),
    CreateDate DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Alerts_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE CASCADE,
    CONSTRAINT FK_Alerts_Types FOREIGN KEY (AlertTypeID)
        REFERENCES AlertTypes(AlertTypeID)
);
GO

/* ===========================================================
   TABELA: Friends (Amizades)
   =========================================================== */
CREATE TABLE Friends (
    FriendshipID INT IDENTITY(1,1) PRIMARY KEY,
    RequesterID INT NOT NULL,
    ReceiverID INT NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Friends_Requester FOREIGN KEY (RequesterID)
        REFERENCES Accounts(AccountID) ON DELETE CASCADE,
    CONSTRAINT FK_Friends_Receiver FOREIGN KEY (ReceiverID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION
);
GO

INSERT INTO Friends (RequesterID, ReceiverID, Status)
VALUES (2, 3, 'Accepted');
GO

/* ===========================================================
   TABELA: Comments (Comentários)
   =========================================================== */
CREATE TABLE Comments (
    CommentID INT IDENTITY(1,1) PRIMARY KEY,
    BlogID INT NOT NULL,
    AccountID INT NOT NULL,
    Content NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Comments_Blogs FOREIGN KEY (BlogID)
        REFERENCES Blogs(BlogID) ON DELETE CASCADE,
    CONSTRAINT FK_Comments_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION
);
GO

INSERT INTO Comments (BlogID, AccountID, Content)
VALUES (1, 3, 'Parabéns pelo post, João! Ficou excelente!');
GO

/* ===========================================================
   TABELA: Conversations (Chats)
   =========================================================== */
CREATE TABLE Conversations (
    ConversationID INT IDENTITY(1,1) PRIMARY KEY,
    User1ID INT NOT NULL,
    User2ID INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastMessageAt DATETIME2 NULL,
    CONSTRAINT FK_Conversations_User1 FOREIGN KEY (User1ID)
        REFERENCES Accounts(AccountID) ON DELETE CASCADE,
    CONSTRAINT FK_Conversations_User2 FOREIGN KEY (User2ID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION
);
GO

/* ===========================================================
   TABELA: Messages (Mensagens)
   =========================================================== */
CREATE TABLE Messages (
    MessageID INT IDENTITY(1,1) PRIMARY KEY,
    ConversationID INT NOT NULL,
    SenderID INT NOT NULL,
    MessageText NVARCHAR(1000) NOT NULL,
    SentAt DATETIME2 DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0,
    CONSTRAINT FK_Messages_Conversation FOREIGN KEY (ConversationID)
        REFERENCES Conversations(ConversationID) ON DELETE CASCADE,
    CONSTRAINT FK_Messages_Sender FOREIGN KEY (SenderID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION
);
GO

INSERT INTO Conversations (User1ID, User2ID)
VALUES (2, 3);

DECLARE @convId INT = SCOPE_IDENTITY();

INSERT INTO Messages (ConversationID, SenderID, MessageText)
VALUES (@convId, 2, 'Oi Maria, tudo bem?');
GO

/* ===========================================================
   TABELAS: Likes
   =========================================================== */
CREATE TABLE BlogLikes (
    BlogLikeID INT IDENTITY(1,1) PRIMARY KEY,
    BlogID INT NOT NULL,
    AccountID INT NOT NULL,
    LikedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_BlogLikes_Blogs FOREIGN KEY (BlogID)
        REFERENCES Blogs(BlogID) ON DELETE CASCADE,
    CONSTRAINT FK_BlogLikes_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION,
    CONSTRAINT UQ_BlogLike UNIQUE (BlogID, AccountID)
);
GO

CREATE TABLE CommentLikes (
    CommentLikeID INT IDENTITY(1,1) PRIMARY KEY,
    CommentID INT NOT NULL,
    AccountID INT NOT NULL,
    LikedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_CommentLikes_Comments FOREIGN KEY (CommentID)
        REFERENCES Comments(CommentID) ON DELETE CASCADE,
    CONSTRAINT FK_CommentLikes_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE NO ACTION,
    CONSTRAINT UQ_CommentLike UNIQUE (CommentID, AccountID)
);
GO

INSERT INTO BlogLikes (BlogID, AccountID)
VALUES (1, 3);
GO

/* ===========================================================
   TABELA: Feed (Timeline)
   =========================================================== */
CREATE TABLE Feed (
    FeedID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT NOT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    ReferenceID INT NULL,
    ReferenceTable NVARCHAR(50) NULL,
    Message NVARCHAR(255) NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Feed_Accounts FOREIGN KEY (AccountID)
        REFERENCES Accounts(AccountID) ON DELETE CASCADE
);
GO

INSERT INTO Feed (AccountID, ActionType, ReferenceID, ReferenceTable, Message)
VALUES (2, 'Postou', 1, 'Blogs', 'João Silva publicou um novo post: "Meu Primeiro Post"');
GO

/* ===========================================================
   VIEWS (com TOP 100 PERCENT)
   =========================================================== */
CREATE OR ALTER VIEW vw_LatestBlogs AS
SELECT TOP 100 PERCENT
    b.BlogID, b.Title, b.Subject, b.PageName, b.Post,
    b.CreateDate, b.UpdateDate, b.IsPublished,
    a.Username, a.DisplayName
FROM Blogs b
INNER JOIN Accounts a ON b.AccountID = a.AccountID
WHERE b.IsPublished = 1
ORDER BY b.UpdateDate DESC, b.CreateDate DESC;
GO

CREATE OR ALTER VIEW vw_AlertsFeed AS
SELECT TOP 100 PERCENT
    al.AlertID, a.Username AS FromUser, at.TypeName AS AlertType,
    al.Message, al.CreateDate
FROM Alerts al
INNER JOIN Accounts a ON al.AccountID = a.AccountID
INNER JOIN AlertTypes at ON al.AlertTypeID = at.AlertTypeID
ORDER BY al.CreateDate DESC;
GO

CREATE OR ALTER VIEW vw_UserFeed AS
SELECT TOP 100 PERCENT
    f.FeedID, a.Username, a.DisplayName,
    f.ActionType, f.ReferenceID, f.ReferenceTable,
    f.Message, f.CreatedAt
FROM Feed f
INNER JOIN Accounts a ON f.AccountID = a.AccountID
ORDER BY f.CreatedAt DESC;
GO

/* ===========================================================
   STORED PROCEDURES
   =========================================================== */
CREATE OR ALTER PROCEDURE sp_GetBlogsByAccountID
    @AccountID INT
AS
BEGIN
    SELECT 
        BlogID, Title, Subject, PageName, Post,
        IsPublished, CreateDate, UpdateDate
    FROM Blogs
    WHERE AccountID = @AccountID
    ORDER BY CreateDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddNewBlogPostAlert
    @AccountID INT,
    @BlogTitle NVARCHAR(200)
AS
BEGIN
    DECLARE @AlertMessage NVARCHAR(500) = CONCAT('Novo post publicado: ', @BlogTitle);
    INSERT INTO Alerts (AccountID, AlertTypeID, Message)
    VALUES (@AccountID, 1, @AlertMessage);
END;
GO

CREATE OR ALTER PROCEDURE sp_AddFeedEvent
    @AccountID INT,
    @ActionType NVARCHAR(50),
    @ReferenceID INT,
    @ReferenceTable NVARCHAR(50),
    @Message NVARCHAR(255)
AS
BEGIN
    INSERT INTO Feed (AccountID, ActionType, ReferenceID, ReferenceTable, Message)
    VALUES (@AccountID, @ActionType, @ReferenceID, @ReferenceTable, @Message);
END;
GO
