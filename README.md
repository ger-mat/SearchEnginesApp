# SearchEnginesApp

Web-приложение, которое ищет заданные пользователем слова в трёх поисковых системах:
Yandex, Google, Bing.
Тот результат, который приходит первым, 
записывается в базу данных и выводится на страницу.
Ответы от остальных сервисов игнорируются.
Результат состоит из первых 10-ти значений, выданных сервисом.

Также добавлена отдельная страница со строкой поиска, но только уже по сохраненным в БД записям.

Список поисковиков дополняем и конфигурируем в плане реализации.
Например, если потребуется имплементировать логику для еще одного поисковика, 
это не должно составить проблем.

Реализовано с использованием ASP.NET Core, EF Core, MS SQL Server.
Код приложения покрыт unit-тестами. 

### Конфигурация поисковых сервисов и строки подключения к БД
Необходимо заменить ключи вида в **appsettings.json/appsettings.Development.json**
* База данных: DefaultConnection.
* Yandex: User, Key.
* Google: ApiKey, Cx.
* Bing: AccessKey.

### Скрипт для создания БД и ее объектов (Code First)
*(Возможная альтерантива: расскоментировать Database.EnsureCreated() в **SearchContext.cs**)*
```sql
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [SearchResults] (
    [Id] int NOT NULL IDENTITY,
    [Query] nvarchar(1024) NOT NULL,
    [EngineName] nvarchar(30) NOT NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_SearchResults] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [FoundItems] (
    [Id] int NOT NULL IDENTITY,
    [SearchResultId] int NOT NULL,
    [Title] nvarchar(1024) NOT NULL,
    [Url] nvarchar(1024) NOT NULL,
    [Snippet] nvarchar(1024) NOT NULL,
    CONSTRAINT [PK_FoundItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FoundItems_SearchResults_SearchResultId] FOREIGN KEY ([SearchResultId]) REFERENCES [SearchResults] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_FoundItems_SearchResultId] ON [FoundItems] ([SearchResultId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200228_Initial', N'3.1.2');

GO


```
