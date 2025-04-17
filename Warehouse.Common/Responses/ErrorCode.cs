namespace Warehouse.Common.Responses
{
    public enum ErrorCode
    {
        None = 0,
        NotFound = 1,
        InvalidInput = 2,
        ValidationError = 3,
        ServiceUnavailable = 4,
        Unauthorized = 5,
        GenericError = 6,
        Conflict = 7,
        BadRequest = 8,
        InternalError = 9,
        InvalidCredentials = 10,
        Duplicate = 11
    }
}