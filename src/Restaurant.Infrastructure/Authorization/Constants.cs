namespace Restaurant.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasNationality = "HasNationality";
    public const string IsOfLegalAge = "IsOfLegalAge";
    public const string MinimumRestaurantCreated = "MinimumRestaurantCreated";
}

public static class ClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}