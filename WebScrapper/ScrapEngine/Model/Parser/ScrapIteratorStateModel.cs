using SqliteDatabase.Model;
using System.Collections.Generic;
using System;
using ScrapEngine.Interfaces;
using System.Diagnostics;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class which temporarily maintains the states during the web scrapping and parsing
    /// activity
    /// </summary>
    public class ScrapIteratorStateModel
    {
        #region Properties

        /// <summary>
        /// The stack of parameters for storing all <see cref="ScrapStateModel"/>
        /// </summary>
        public Stack<ScrapStateModel> ScrapStack { get; set; }

        /// <summary>
        /// The stack of parameters for storing all <see cref="ColumnScrapStateModel"/>
        /// </summary>
        public Stack<ColumnScrapStateModel> ColumnStack { get; set; }

        /// <summary>
        /// The stack of parameters for storing all <see cref="ManipulateStateModel"/>
        /// </summary>
        public Stack<ManipulateStateModel> ManipulateStack { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScrapIteratorStateModel()
        {
            ScrapStack = new Stack<ScrapStateModel>();
            ColumnStack = new Stack<ColumnScrapStateModel>();
            ManipulateStack = new Stack<ManipulateStateModel>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Push the Scrap state
        /// </summary>
        /// <param name="state"></param>
        public void Push(ScrapStateModel state) => ScrapStack.Push(state);

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ScrapStateModel PeekScrap() => ScrapStack.Peek();

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ScrapStateModel PopScrap() => ScrapStack.Pop();

        /// <summary>
        /// Push the Scrap state
        /// </summary>
        /// <param name="state"></param>
        public void Push(ColumnScrapStateModel state) => ColumnStack.Push(state);

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ColumnScrapStateModel PeekColumn() => ColumnStack.Peek();

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ColumnScrapStateModel PopColumn() => ColumnStack.Pop();

        /// <summary>
        /// Push the Scrap state
        /// </summary>
        /// <param name="state"></param>
        public void Push(ManipulateStateModel state) => ManipulateStack.Push(state);

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ManipulateStateModel PeekManipulate() => ManipulateStack.Peek();

        /// <summary>
        /// Peek the topmost scrap state 
        /// </summary>
        /// <returns></returns>
        public ManipulateStateModel PopManipulate() => ManipulateStack.Pop();

        #endregion Methods
    }
}
