namespace PaymentsService.Domain.Exceptions
{
    public class AccountNotExistsException(int user_id) 
        : ArgumentException($"Счет пользователя с id {user_id} не найден")
    { }
}
