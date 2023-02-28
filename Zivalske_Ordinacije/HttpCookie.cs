namespace Zivalske_Ordinacije
{
    internal class HttpCookie
    {
        private string v;

        public HttpCookie(string v)
        {
            this.v = v;
        }

        public string Value { get; internal set; }
    }
}