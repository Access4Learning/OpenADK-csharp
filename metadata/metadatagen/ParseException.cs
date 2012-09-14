namespace Edustructures.Metadata
{
    using System;

    /// <summary>  Signals a parsing error.
    /// *
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// 
    /// </version>
    public class ParseException : Exception
    {
        public ParseException()
            : base() {}

        public ParseException( String msg )
            : base( msg ) {}

        public ParseException( String msg,
                               Exception innerException )
            : base( msg, innerException ) {}
    }
}