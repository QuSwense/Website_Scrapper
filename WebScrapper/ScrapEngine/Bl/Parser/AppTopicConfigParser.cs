using ScrapEngine.Interfaces;
using System.Collections.Generic;
using System;
using System.Collections;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The base class for the scrap xml app topic engine
    /// </summary>
    public class AppTopicConfigParser : IInnerBaseParser
    {
        #region Properties

        /// <summary>
        /// The config parser template
        /// </summary>
        protected ConfigParserTemplate configParserTemplate;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AppTopicConfigParser() { }

        #endregion Constructor

        #region Parser Methods
        
        /// <summary>
        /// The template method for processing
        /// </summary>
        public virtual void Process()
        {
            PreProcess();
            IList listResult = GetDataListIterator();

            if (listResult != null && listResult.Count > 0)
            {
                PreLoopProcess(listResult);

                foreach (var item in listResult)
                {
                    LoopProcess(item);
                }

                PostLoopProcess(listResult);
            }

            PostProcess();
        }

        protected virtual void PostLoopProcess(IList listResult)
        {
            throw new NotImplementedException();
        }

        protected virtual void LoopProcess<T>(T item)
        {
            StartInLoopProcess();

            configParserTemplate.ParseChildren(
                configParserTemplate.StateModel.ConfigStack.Peek());

            EndInLoopProcess();
        }

        protected virtual void EndInLoopProcess()
        {
            throw new NotImplementedException();
        }

        protected virtual void StartInLoopProcess()
        {
            throw new NotImplementedException();
        }

        protected virtual void PreLoopProcess(IList listResult)
        {
            throw new NotImplementedException();
        }

        protected virtual IList GetDataListIterator()
        {
            throw new NotImplementedException();
        }

        protected virtual void PreProcess()
        {
            throw new NotImplementedException();
        }

        protected virtual void PostProcess()
        {
            throw new NotImplementedException();
        }

        #endregion Parser Methods
    }
}
