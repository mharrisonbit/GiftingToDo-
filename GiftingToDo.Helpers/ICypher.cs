namespace GiftingToDo.Helpers
{
    public interface ICypher
    {
        string Base64Decode(string base64EncodedData);
        string Base64Encode(string plainText);
    }
}