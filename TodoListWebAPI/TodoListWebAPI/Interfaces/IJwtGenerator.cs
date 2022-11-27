namespace TodoListWebAPI.Interfaces;

public interface IJwtGenerator
{
    string GenerateJwtToken(string userName);
}