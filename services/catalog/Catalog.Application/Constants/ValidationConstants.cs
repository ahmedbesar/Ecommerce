namespace Catalog.Application.Constants;

public static class ValidationConstants
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 500;
    public const int SummaryMaxLength = 250;
    public const int ImageFileMaxLength = 255;

    public const string MongoObjectIdPattern = "^[a-fA-F0-9]{24}$";
}
