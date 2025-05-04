namespace Forum.Application.Helpers
{
    public static class Text
    {
        public static string ToPlural(string word)
        {
            if (word.EndsWith("y"))
            {
                word = word.Substring(0, word.Length - 2) + "ies";
            }
            else
            {
                word += "s";
            }

            return word;
        }
    }
}
