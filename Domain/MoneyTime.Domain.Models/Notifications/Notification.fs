
namespace MoneyTime.Domain.Models

open System

type OrderItem = {
    ProductId: Guid;
    Price: Decimal;
}