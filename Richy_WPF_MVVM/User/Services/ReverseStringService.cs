namespace Richy_WPF_MVVM.User.Services
{
    public class ReverseStringService
    {
        public ReverseStringService() { }

        public string ReverseString(string str)
        {
            if(string.IsNullOrEmpty(str)) return string.Empty;

            char[] chars = new char[str.Length];
            for (int i = str.Length -1, j = 0; i >= 0; i--, j++)
            {
                chars[j] = str[i];
            }

            return new string(chars);
        }
    }
}
