using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleWebsiteScrapper.ParseTree;
using System.IO;

namespace SimpleWebsiteScrapper.Output
{
    /// <summary>
    /// The class which outputs the processor data in Json format
    /// </summary>
    public class ProcessorJsonOutput : ProcessorEngineOutput
    {
        /// <summary>
        /// Overriden method to output in json format.
        /// This uses a 64 kB buffer
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="processorNode"></param>
        public override void Output(string fullPath, ScrapWebpageProcessorNode processorNode)
        {
            using (fileWriter = new StreamWriter(File.OpenWrite(fullPath), Encoding.UTF8, 1024 * 64))
            {
                int level = 0;
                GenerateRecursive(processorNode, level);
            }
        }

        /// <summary>
        /// A recursive function to generate the json file
        /// </summary>
        /// <param name="processorNode"></param>
        /// <param name="level"></param>
        private void GenerateRecursive(ScrapWebpageProcessorNode processorNode, int level)
        {
            WriteLine(level, "{");

            if(processorNode.Copyrights != null && processorNode.Copyrights.Count > 0)
            {
                WriteBaseNodeList(processorNode.Copyrights, "Copyrights", level + 1);
            }

            WriteLine(level, "}");
        }

        private void WriteBaseNodeList(ScrapBaseProcessorNodeList list, string rootName, int level)
        {
            Write(level, rootName); Write(":"); WriteLine("[");

            for (int indx = 0; indx < list.Count; ++indx)
            {
                ScrapBaseProcessorNode baseNode = list[indx];

                // Id
                Write(level, "UserFeature"); Write(level, ":"); Write(level, ":");
                Write(level, "UserFeature"); Write(level, ":"); WriteLine(level, "{");

                Write("Text");
                Write(":");
                Write

                WriteLine("}");
            }

            WriteLine("]");
        }
    }
}
