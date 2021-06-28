# Ipr.Book.Dumper
Инструмент для сохранения любой доступной книги с сайта https://iprbookshop.ru/ в формате pdf

* [.net 5](https://dotnet.microsoft.com/download/dotnet/5.0) 

## Пример вызова сервиса
```
Ipr.Book.Dumper.exe --save Dump --id 101612,105704,101085,102237 --username <login> --password <password>
```

## Где 
```
--id - идентификаторы книг на сайта https://iprbookshop.ru/
--save - директория для сохранения файла
--username - имя пользователя от системы
--password - пароль от системы
```

## Полный список опций 

```
Ipr.Book.Dumper.exe --help
```

## Публикация
```
dotnet publish -c Release -o Binary/win-x64 -r win-x64 --self-contained true
dotnet publish -c Release -o Binary/linux-x64 -r linux-x64 --self-contained true
```
