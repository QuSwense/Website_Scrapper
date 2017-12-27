using ScrapEngine.Factory;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using ScrapEngine.Model.Scrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// This class handles the common parser logic
    /// </summary>
    public class ConfigParserTemplate
    {
        #region Public Properties

        /// <summary>
        /// Performance measurement for debug
        /// </summary>
        public PerformanceMeasure Performance { get; set; }

        /// <summary>
        /// Stores the state of the parsers
        /// </summary>
        public ScrapIteratorStateModel StateModel { get; set; }

        /// <summary>
        /// A factory property which maps the config element to the parser logic class
        /// </summary>
        private ConfigParserFactory configParserFactory;

        #endregion Public Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigParserTemplate()
        {
            Performance = new PerformanceMeasure();
            StateModel = new ScrapIteratorStateModel();
            configParserFactory = new ConfigParserFactory();
        }
        
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Loop through the list of children and activates parsing
        /// </summary>
        /// <param name="configElement"></param>
        public void ParseChildren(IConfigElement configElement)
        {
            if (configElement == null) return;

            for (int i = 0; i < configElement.Children.Count; i++)
            {
                IConfigElement configChildElement = configElement.Children[i];

                // Start performance
                Performance.Start(configChildElement, i);

                // Save in the State model
                StateModel.Push(configChildElement);

                // Process the element config
                configParserFactory.Parser(configChildElement).Process();

                // Clear in the State model
                StateModel.Pop(configChildElement);

                // Stop performance
                Performance.Stop(configChildElement, i);
            }
        }

        #endregion Public Methods
    }
}
