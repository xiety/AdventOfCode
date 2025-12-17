using System.CodeDom.Compiler;

static class IndentedTextWriterExtensions
{
    extension(IndentedTextWriter writer)
    {
        internal void WriteIndentedRaw(string raw)
        {
            foreach (var line in raw.Split('\n'))
            {
                var trimmed = line.TrimEnd();

                if (String.IsNullOrEmpty(trimmed))
                    writer.WriteLineNoTabs(String.Empty);
                else
                    writer.WriteLine(trimmed);
            }
        }
    }
}
