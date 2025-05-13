namespace Restaurant.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} with id: {resourceIdentifier} not found")
{
}