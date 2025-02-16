CryptoWallet API

Описание.
CryptoWallet API позволяет регистрировать пользователей, пополнять баланс и снимать средства.

Установка.
Клонируйте репозиторий:
git clone https://github.com/rev2nge/CryptoWallet.git

Перейдите в папку проекта и установите зависимости:
cd CryptoWallet
dotnet restore

Конфигурация:
Файл appsettings.json должен содержать строку подключения к базе данных.

Миграция:
Создайте БД с помощью команды Update-Migration 

Запустите проект:
dotnet run

API Эндпоинты
POST
/api/user/register
Регистрация пользователя

GET
/api/user/{userId}/balance
Получение баланса

POST
/api/user/{userId}/deposit
Пополнение баланса

POST
/api/user/{userId}/withdraw
Снятие средств
