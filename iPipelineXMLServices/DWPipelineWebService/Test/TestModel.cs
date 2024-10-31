namespace DWPipelineWebService.Test
{
    public static class TestModel
    {

        public static string Policy_NO = string.Empty;

        public static string GenerateRandomString(this string sourceAlphabet, int length)
        {
            var chars = new char[length];
            Random Random = new();
            // reuse local 'length' var for iteration
            while (--length >= 0)
                chars[length] = sourceAlphabet[Random.Next(sourceAlphabet.Length)];

            return new string(chars);
        }

    }
}
