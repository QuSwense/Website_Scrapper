using QWWebScrap.OModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QWCommonDST.Helper;
using System.IO;

namespace QWScrapEngine
{
    /// <summary>
    /// The base class used to output the parsed Website scrapper data.
    /// It outputs the dataset <see cref="ScrapWebData"/> into the required file
    /// </summary>
    public class BaseOutputEngine
    {
        /// <summary>
        /// The reference to the processed and scrapped webpage data
        /// </summary>
        private List<ScrapWebData> ProcessedWebData { get; set; }

        /// <summary>
        /// The stack of files
        /// </summary>
        private Stack<TextWriter> stackFiles;

        /// <summary>
        /// The root folder path where all the files are stored
        /// </summary>
        private string folderPath;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseOutputEngine() { }

        /// <summary>
        /// Constructor by parameter
        /// </summary>
        /// <param name="processedWebData"></param>
        public BaseOutputEngine(List<ScrapWebData> processedWebData, Type engineType)
        {
            this.ProcessedWebData = processedWebData;

            // Caculate the folder
            engineType.FullName.Split(new char[] { '.' })
                .ForLoop((name, i) => { if(i > 0) folderPath += name + "\\"; });
            Directory.CreateDirectory(folderPath);
        }

        /// <summary>
        /// The main public method to output the data
        /// The logic should follow post traversal
        /// </summary>
        public void Output()
        {
            try
            {
                Initialize();

                // Generate the Output dataset loop
                ProcessedWebData.ForLoop((scrapWebData, indx) =>
                {
                    GenerateWebData(scrapWebData, indx);

                    // Generate Referecnes web data
                    GenerateMetadata(scrapWebData, scrapWebData.References, "References");

                    // Generate Referecnes web data
                    GenerateMetadata(scrapWebData, scrapWebData.Copyrights, "Copyrights");
                });
            }
            finally
            {
                Cleanup();
            }
        }

        private void GenerateMetadata(ScrapWebData scrapWebData, List<ScrapMetadata> metadataList, string type)
        {
            string altTextFile = string.Format("{0}_{1}", scrapWebData.id, type);

            if(metadataList != null && metadataList.Count > 0)
            {
                string fullName = Path.Combine(folderPath, altTextFile + ".csv");
                using (TextWriter txtWriter = new StreamWriter(fullName))
                {
                    txtWriter.WriteLine(GetColumnForNodes(metadataList));
                    metadataList.ForLoop((metadata, indx) =>
                    {
                        txtWriter.WriteLine(string.Format("\"{0}\",", GenerateMetadata(metadata, indx)));
                    });
                }
            }
        }

        /// <summary>
        /// A Output generator for <see cref="ScrapWebData"/> dataset
        /// </summary>
        /// <param name="scrapWebData"></param>
        protected string GenerateWebData(ScrapWebData scrapWebData, int indx)
        {
            string altTextFile = string.Format("{0}_{1}", scrapWebData.id, indx);

            // Generate Referecnes web data
            GenerateMetadata(scrapWebData, scrapWebData.References, "References");

            // Generate Referecnes web data
            GenerateMetadata(scrapWebData, scrapWebData.Copyrights, "Copyrights");

            // If the node contains children then create a file and save it on stack
            if (scrapWebData.Nodes != null)
            {
                string fullName = Path.Combine(folderPath, altTextFile + ".csv");
                using (TextWriter txtWriter = new StreamWriter(fullName))
                {
                    txtWriter.WriteLine(GetColumnForNodes(scrapWebData.Nodes[indx]));
                    scrapWebData.Nodes.ForLoop((scrapWebDataRows, indxRow) =>
                    {
                        scrapWebDataRows.ForLoop((scrapWebDataChild, indxChild) =>
                        {
                            txtWriter.Write(string.Format("\"{0}\",", GenerateWebData(scrapWebDataChild, indxChild)));
                        });
                        txtWriter.WriteLine();
                    });
                }
            }
            else
            {
                if (scrapWebData.Text != null && !string.IsNullOrEmpty(scrapWebData.Text.Text))
                    return scrapWebData.Text.Text;
                else
                    return altTextFile;
            }

            return altTextFile;
        }

        protected string GetColumnForNodes(List<ScrapWebData> scrapWebDataRows)
        {
            string columnHeader = "";
            scrapWebDataRows.ForLoop((scrapWebDataChild, indxChild) => {
                columnHeader += string.Format("\"{0}\",", scrapWebDataChild.id);
            });

            return columnHeader;
        }

        protected string GetColumnForNodes(List<ScrapMetadata> scrapWebDataRows)
        {
            string columnHeader = "";
            scrapWebDataRows.ForLoop((scrapWebDataChild, indxChild) => {
                columnHeader += string.Format("\"{0}\",", scrapWebDataChild.Id);
            });

            return columnHeader;
        }

        protected string GenerateMetadata(ScrapMetadata reference, int refIndx)
        {
            string altTextFile = string.Format("{0}_{1}", reference.Id, refIndx);

            // If the node contains children then create a file and save it on stack
            if (reference.Nodes != null && reference.Nodes.Count > 0)
            {
                string fullName = Path.Combine(folderPath, altTextFile + ".csv");
                using (TextWriter txtWriter = new StreamWriter(fullName))
                {
                    txtWriter.WriteLine(GetColumnForNodes(reference.Nodes[refIndx]));
                    reference.Nodes.ForLoop((scrapMetadataRows, indxRow) =>
                    {
                        scrapMetadataRows.ForLoop((scrapMetadataChild, indxChild) =>
                        {
                            txtWriter.Write(string.Format("\"{0}\",", GenerateMetadata(scrapMetadataChild, indxChild)));
                        });
                        txtWriter.WriteLine();
                    });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(reference.Text))
                    return reference.Text;
                else
                    return altTextFile;
            }

            return altTextFile;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Cleanup()
        {
            stackFiles.Clear();
            stackFiles = null;
        }

        /// <summary>
        /// Initialize any data structures required for processing / outputing dataset to files
        /// </summary>
        private void Initialize()
        {
            stackFiles = new Stack<TextWriter>();
        }
    }
}
