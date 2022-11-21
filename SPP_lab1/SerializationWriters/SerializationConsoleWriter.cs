namespace SPP_lab1
{
    internal class SerializationConsoleWriter : ISerializationWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
