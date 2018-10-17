namespace SerpoServer.Tests
{
    public class StructVsClass
    {
        [Fact]
        public void strCl()
        {
            var sw = new Stopwatch();
            sw.Start();
            var cl = new values();
            var stB = new StringBuilder();
            for (var i = 0; i < 1000; i++)
            {
                stB.Append(string.Concat(" Hello", i));
                cl.hello += stB.ToString();
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Restart();
            var st = new values2();
            var stB2 = new StringBuilder();
            for (var i = 0; i < 1000; i++)
            {
                stB2.Append(string.Concat(" Hello", i));
                st.hello += stB2.ToString();
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }

    public class values
    {
        public string hello;
        public int id;
    }

    public struct values2
    {
        public int id;
        public string hello;
    }
}