//using System;

//namespace org.ncore.ServicedApi.Exceptions
//{
//    // HACK: remove this class and reference Org.ncore.  JF
//    public class RuntimeCondition : ApplicationException 
//    {
//        public const string DEFAULT_MESSAGE = "A runtime condition has occurred.";
//        public const string DEFAULT_INSTRUCTION_TEXT = "The application encountered an unexpected runtime condition requiring user intervention.";

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        public RuntimeCondition() : base( DEFAULT_MESSAGE )
//        {
//            _instructionText = DEFAULT_INSTRUCTION_TEXT;
//        }

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="message">A string containing a debug message that describes the exception.</param>
//        public RuntimeCondition( string message ) : base( message )
//        {
//            _instructionText = DEFAULT_INSTRUCTION_TEXT;

//        }

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="message">A string containing a debug message that describes the exception.</param>
//        /// <param name="message">A string containing instruction expression to be displayed to the end user.</param>
//        public RuntimeCondition( string message, string instructionText ) : base( message )
//        {
//            _instructionText = instructionText;
//        }

//        protected string _instructionText = DEFAULT_INSTRUCTION_TEXT;
//    }
//}