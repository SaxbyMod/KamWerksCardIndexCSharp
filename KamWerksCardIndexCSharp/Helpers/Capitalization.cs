namespace KamWerksCardIndexCSharp.Helpers
{
	public static class Capitalization
	{
		// This function comes from Stack Overflow - javadch
		public static string Capitalize(this string word)
		{
			return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
		}
		
		// This function admitedly was made a long while ago by GPT to capitalize sentances that contain spaces [Could probably make my own now, but lazy]
		public static string CapitalizeWithSpaces(this string sentence)
		{
			string[] words = sentence.Split(' ');
			for (int i = 0; i < words.Length; i++)
			{
				words[i] = words[i].Capitalize();
			}

			return string.Join(" ", words);
		}
	}
}