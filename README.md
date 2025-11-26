# BookingAPI
Описание
BookingAPI реализует CRUD-функционал для работы с бронированиями:
  создание
  получение списка
  получение конкретного бронирования
  обновление статуса
  удаление
Сервис разработан как часть распределённой микросервисной архитектуры и может взаимодействовать с внешними сервисами.

# Технологии
  .NET 6
  ASP.NET Web API
  Entity Framework Core
  REST API + JSON

# API эндпоинты
GET /api/booking
Возвращает список всех бронирований.
Пример ответа:

[
  {
    "id": 1,
    "customerName": "John Doe",
    "startDate": "2025-11-12T00:00:00",
    "endDate": "2025-11-15T00:00:00",
    "status": "Pending"
  }
]

GET /api/booking/{id}
Получить бронирование по ID.
Ответ:
{
  "id": 5,
  "customerName": "Anna",
  "status": "Approved"
}

POST /api/booking
Создаёт бронирование.
Требуется заголовок:
Content-Type: application/json
Тело запроса:
{
  "customerName": "Anton",
  "startDate": "2025-12-01",
  "endDate": "2025-12-05",
  "basePrice": 120
}

PUT /api/booking/{id}/status
Обновляет статус бронирования (Admin).
Тело запроса:
{
  "status": "Completed"
}
