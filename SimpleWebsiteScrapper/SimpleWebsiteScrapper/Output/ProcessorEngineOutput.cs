using SimpleWebsiteScrapper.ParseTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleWebsiteScrapper.Output
{
    /// <summary>
    /// The abstract class which contains basic logic to output data into different formats.
    /// All other classes are inherited from this class
    /// </summary>
    public abstract class ProcessorEngineOutput
    {
        /// <summary>
        /// The file to write the output
        /// </summary>
        protected TextWriter fileWriter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessorEngineOutput() { }

        /// <summary>
        /// The method used to generate the output
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="processorNode"></param>
        public abstract void Output(string fullPath, ScrapWebpageProcessorNode processorNode);

        /// <summary>
        /// Write a line to file
        /// </summary>
        protected void WriteLine(string format)
        {
            if (GlobalSettings.UseMinimisedProcessorFileOutputMode)
                fileWriter.Write(format);
            else
                fileWriter.WriteLine(format);
        }

        /// <summary>
        /// Write a line to file
        /// </summary>
        protected void WriteLine(string format, params string[] args)
        {
            if (GlobalSettings.UseMinimisedProcessorFileOutputMode)
                fileWriter.Write(format, args);
            else
                fileWriter.WriteLine(format, args);
        }

        /// <summary>
        /// Write a line to file
        /// </summary>
        protected void WriteLine(int level, string format)
        {
            WriteLine(new String(GlobalSettings.WhitespaceFileOutputMode, level) + format);
        }

        /// <summary>
        /// Write a line to file
        /// </summary>
        protected void WriteLine(int level, string format, params string[] args)
        {
            WriteLine(new String(GlobalSettings.WhitespaceFileOutputMode, level) + format, args);
        }

        /// <summary>
        /// Write to file
        /// </summary>
        protected void Write(string format)
        {
            if (GlobalSettings.UseMinimisedProcessorFileOutputMode)
                fileWriter.Write(format);
            else
                fileWriter.WriteLine(GlobalSettings.WhitespaceFileOutputMode + format);
        }

        /// <summary>
        /// Write to file
        /// </summary>
        protected void Write(string format, params string[] args)
        {
            if (GlobalSettings.UseMinimisedProcessorFileOutputMode)
                fileWriter.Write(format, args);
            else
                fileWriter.WriteLine(GlobalSettings.WhitespaceFileOutputMode + format, args);
        }

        /// <summary>
        /// Write to file
        /// </summary>
        protected void Write(int level, string format)
        {
            Write(new String(GlobalSettings.WhitespaceFileOutputMode, level) + format);
        }

        /// <summary>
        /// Write to file
        /// </summary>
        protected void Write(int level, string format, params string[] args)
        {
            Write(new String(GlobalSettings.WhitespaceFileOutputMode, level) + format, args);
        }
    }
}
